﻿using Assets.Factions;
using Assets.Helpers;
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

            GenerateMap(mapManager, size: 5, structureFactory, factionManger);

            MakeFactionCores(mapManager, factionManger);

            var cameraController = Locate<ICameraController>();
            ConfigureCameraDefaults(mapManager, factionManger, cameraController);
        }

        private static void ConfigureCameraDefaults(IMapManager mapManager, IFactionManager factionManger, ICameraController cameraController)
        {
            cameraController.ConfigureBounds(0, mapManager.Width, 0, mapManager.Height);
            cameraController.MoveToPosition(factionManger.GetPlayerFaction().StructureManager.GetStructures()[0].GetOrigin().ToAdjustedVector3());
        }

        private void MakeFactionCores(IMapManager mapManager, IFactionManager factionManager)
        {
            var placementValidator = Locate<IPlacementValidator>();

            var faction = factionManager.GetPlayerFaction();

            var coreCell = mapManager.GetRandomCell((cell) => cell.Terrain.Type == TerrainType.Grass);
            faction.StructureManager.AddStructure(StructureType.Core, coreCell.Coord);

            var coreRect = mapManager.GetRectangle(coreCell.Coord, 2, 2);
            var roadRect = coreRect.SelectMany(c => c.NonNullNeighbors).Except(coreRect);
            foreach (var cell in roadRect)
            {
                faction.StructureManager.AddStructure(StructureType.Road, cell.Coord);
            }

            foreach (var anchorPoint in CellExtensions.GetCardinalsOutsideRectangle(roadRect))
            {
                faction.StructureManager.AddStructure(StructureType.Anchor, anchorPoint.Coord);
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