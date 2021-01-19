using Assets.Factions;
using Assets.Map;
using Assets.MapGeneration;
using Assets.ServiceLocator;
using Assets.StrategyCamera;
using Assets.Structures;
using Assets.Structures.Behaviors;

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

            GenerateMap(mapManager, size: 10);

            RegisterFactions(factionManger);

            PopulateMap(mapManager, structureFactory, factionManger);

            MakeFactionCores(structureFactory, mapManager, factionManger);

            var cameraController = Locate<ICameraController>();
            ConfigureCameraDefaults(mapManager, factionManger, cameraController);
        }

        private void PopulateMap(IMapManager mapManager, IStructureFactory structureFactory, IFactionManager factionManager)
        {
            var natureFaction = factionManager.GetNatureFaction();

            var tree = new StructureFacade("Tree", 1, 1, "Tree", "Pine tree", structureFactory.GetBehaviour<NoBehavior>());

            for (int x = 0; x < mapManager.Width; x++)
            {
                for (int z = 0; z < mapManager.Height; z++)
                {
                    var cell = mapManager.GetCellAtCoord(new Coord(x, 0, z));

                    if (cell.Terrain.Name == "Forrest")
                    {
                        if (UnityEngine.Random.value > 0.5f)
                        {
                            natureFaction.StructureManager.AddStructure(tree, cell.Coord);
                        }
                    }
                }
            }
        }

        private static void ConfigureCameraDefaults(IMapManager mapManager, IFactionManager factionManger, ICameraController cameraController)
        {
            cameraController.ConfigureBounds(0, mapManager.Width, 0, mapManager.Height);
            cameraController.MoveToPosition(factionManger.GetPlayerFaction().StructureManager.GetStructures()[0].GetOrigin().ToAdjustedVector3());
        }

        private static void MakeFactionCores(IStructureFactory structureFactory, IMapManager mapManager, IFactionManager factionManager)
        {
            var core = new StructureFacade("SettlementCore", 2, 2, "BellTower", "The heart of this settlement", structureFactory.GetBehaviour<SettlementCore>());
            foreach (var faction in factionManager.GetAllFactions())
            {
                var coreCell = mapManager.GetRandomCell((cell) => cell.GetTravelCost() > 0);
                faction.StructureManager.AddStructure(core, coreCell.Coord);
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

        private void GenerateMap(IMapManager mapManager, int size)
        {
            _mapGen = new MapGenerator(mapManager, size, new DefaultTerrainDefinition());
            _mapGen.GenerateMap();
        }
    }
}