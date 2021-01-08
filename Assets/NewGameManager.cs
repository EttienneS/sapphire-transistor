﻿using Assets.Factions;
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

            GenerateMap(mapManager);

            RegisterFactions(factionManger);
            MakeFactionCores(structureFactory, mapManager, factionManger);

            var cameraController = Locate<ICameraController>();
            ConfigureCameraDefaults(mapManager, factionManger, cameraController);
        }

        private static void ConfigureCameraDefaults(IMapManager mapManager, IFactionManager factionManger, ICameraController cameraController)
        {
            cameraController.ConfigureBounds(0, mapManager.Width, 0, mapManager.Height);
            cameraController.MoveToPosition(factionManger.GetPlayerFaction().StructureManager.GetFactionCoreLocation().ToAdjustedVector3());
        }

        private static void MakeFactionCores(IStructureFactory structureFactory, IMapManager mapManager, IFactionManager factionManager)
        {
            var core = new StructureFacade("SettlmentCore", "BellTower", "The heart of this settlement", structureFactory.GetBehaviour<SettlementCore>());
            foreach (var faction in factionManager.GetAllFactions())
            {
                var coreCell = mapManager.GetRandomCell((cell) => cell.GetTravelCost() > 0);
                faction.StructureManager.AddStructure(core, coreCell.Coord);

                foreach (var cell in coreCell.NonNullNeighbors)
                {
                    faction.StructureManager.AddStructure(core, coreCell.Coord);
                }
            }

            factionManager.MoveToNextTurn();
        }

        private void RegisterFactions(IFactionManager factionManager)
        {
            var locator = GetLocator();
            factionManager.AddFaction(new PlayerFaction("Player", locator));
            factionManager.AddFaction(new AIFaction("Enemy", locator));
        }

        private void GenerateMap(IMapManager mapManager)
        {
            _mapGen = new MapGenerator(mapManager, 30, new DefaultTerrainDefinition());
            _mapGen.GenerateMap();
        }
    }
}