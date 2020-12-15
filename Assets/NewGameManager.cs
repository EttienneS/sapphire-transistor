using Assets.Factions;
using Assets.Map;
using Assets.MapGeneration;
using Assets.ServiceLocator;
using Assets.Structures;
using Assets.Structures.Behaviors;

namespace Assets
{
    public class NewGameManager : GameServiceBase
    {
        private MapGenerator _mapGen;

        public override void Initialize()
        {
            var structureFactory = Locate<StructureFactory>();
            var spawnManager = Locate<SpawnManager>();
            var mapManager = Locate<MapManager>();
            var factionManger = Locate<FactionManager>();

            GenerateMap(mapManager);

            RegisterFactions(structureFactory, spawnManager, factionManger);
            MakeFactionCores(structureFactory, mapManager, factionManger);

            var cameraController = Locate<StrategyCamera.CameraController>();
            ConfigureCameraDefaults(mapManager, factionManger, cameraController);
        }

        private static void ConfigureCameraDefaults(MapManager mapManager, FactionManager factionManger, StrategyCamera.CameraController cameraController)
        {
            cameraController.ConfigureBounds(0, mapManager.Width, 0, mapManager.Height);
            cameraController.MoveToPosition(factionManger.GetPlayerFaction().GetFactionCoreLocation().ToAdjustedVector3());
        }

        private void GenerateMap(MapManager mapManager)
        {
            _mapGen = new MapGenerator(mapManager, 30, new DefaultTerrainDefinition());
            _mapGen.GenerateMap();
        }

        private static void MakeFactionCores(StructureFactory structureFactory, MapManager mapManager, FactionManager factionManger)
        {
            var core = new StructureFacade("SettlmentCore", "Church", "The heart of this settlement", structureFactory.GetBehaviour<SettlementCore>());
            foreach (var faction in factionManger.GetFactions())
            {
                var coreCell = mapManager.GetRandomCell((cell) => cell.TravelCost > 0);
                faction.AddStructure(core, coreCell.Coord);
            }
        }

        private static void RegisterFactions(StructureFactory structureFactory, SpawnManager spawnManager, FactionManager factionManger)
        {
            factionManger.AddFaction(new PlayerFaction("Player", structureFactory, spawnManager));
            factionManger.AddFaction(new AIFaction("Enemy", structureFactory, spawnManager));
        }
    }
}