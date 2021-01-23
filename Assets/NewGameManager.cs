using Assets.Factions;
using Assets.Map;
using Assets.MapGeneration;
using Assets.ServiceLocator;
using Assets.StrategyCamera;
using Assets.Structures;
using Assets.Structures.Behaviors;
using System.Linq;

namespace Assets
{
    public class NewGameManager : GameServiceBase
    {
        private MapGenerator _mapGen;

        public override void Initialize()
        {
            var structureFactory = Locate<IStructureFactory>();
            var mapManager = Locate<IMapManager>();
            var factionManger = Locate<IFactionManager>();

            RegisterFactions(factionManger);

            GenerateMap(mapManager, size: 10, structureFactory, factionManger);

            MakeFactionCores(structureFactory, mapManager, factionManger);

            var cameraController = Locate<ICameraController>();
            ConfigureCameraDefaults(mapManager, factionManger, cameraController);
        }



        private static void ConfigureCameraDefaults(IMapManager mapManager, IFactionManager factionManger, ICameraController cameraController)
        {
            cameraController.ConfigureBounds(0, mapManager.Width, 0, mapManager.Height);
            cameraController.MoveToPosition(factionManger.GetPlayerFaction().StructureManager.GetStructures()[0].GetOrigin().ToAdjustedVector3());
        }

        private void MakeFactionCores(IStructureFactory structureFactory, IMapManager mapManager, IFactionManager factionManager)
        {
            var placementValidator = Locate<IStructurePlacementValidator>();
            var core = new StructureFacade("SettlementCore", 2, 2, "BellTower", "The heart of this settlement", structureFactory.GetBehaviour<SettlementCore>());
            var road = new StructureFacade("Road", 1, 1, "Road", "Road", structureFactory.GetBehaviour<NoBehavior>());
            foreach (var faction in factionManager.GetAllFactions())
            {
                var coreCell = mapManager.GetRandomCell((cell) => cell.Terrain.Type == TerrainType.Grass);
                faction.StructureManager.AddStructure(core, coreCell.Coord);

                var rect = mapManager.GetRectangle(coreCell.Coord, 2, 2);
                foreach (var cell in rect.SelectMany(c => c.NonNullNeighbors).Except(rect))
                {
                    faction.StructureManager.AddStructure(road, cell.Coord);
                }
            }

            factionManager.MoveToNextTurn();
        }

        private void RegisterFactions(IFactionManager factionManager)
        {
            var locator = GetLocator();

            factionManager.AddFaction(new PlayerFaction("Player", locator));
            factionManager.AddFaction(new NatureFaction("Nature", locator));
            factionManager.AddFaction(new AIFaction("Enemy", locator));
        }

        private void GenerateMap(IMapManager mapManager, int size, IStructureFactory structureFactory, IFactionManager factionManger)
        {
            _mapGen = new MapGenerator(mapManager, size, new DefaultTerrainDefinition());
            _mapGen.GenerateMap();
            _mapGen.PopulateMap(mapManager, structureFactory, factionManger);
        }
    }
}