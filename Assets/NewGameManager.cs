using Assets.Cards;
using Assets.Factions;
using Assets.Helpers;
using Assets.Map;
using Assets.MapGeneration;
using Assets.ServiceLocator;
using Assets.StrategyCamera;
using Assets.Structures;
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
            var faction = factionManager.GetPlayerFaction();

            var coreCell = mapManager.GetRandomCell((cell) => cell.Terrain.Type == TerrainType.Grass);
            faction.StructureManager.AddStructure(StructureType.Core, coreCell.Coord);

            var coreRect = mapManager.GetRectangle(coreCell.Coord, 2, 2);
            var roadRect = coreRect.SelectMany(c => c.NonNullNeighbors).Except(coreRect);
            foreach (var cell in roadRect)
            {
                faction.StructureManager.AddStructure(StructureType.Road, cell.Coord);
            }

            foreach (var roadStub in CellExtensions.GetCardinalsOutsideRectangle(roadRect))
            {
                faction.StructureManager.AddStructure(StructureType.Road, roadStub.Coord);
            }

            factionManager.MoveToNextTurn();
        }

        private void RegisterFactions(IFactionManager factionManager)
        {
            var locator = GetLocator();

            var player = new PlayerFaction("Player", locator);
            DealCards(player);
            var nature = new NatureFaction("Nature", locator);
            DealCards(nature);
            var enemy = new AIFaction("Enemy", locator);
            DealCards(enemy);


            factionManager.AddFaction(player);
            factionManager.AddFaction(nature);
            factionManager.AddFaction(enemy);
        }

        private void DealCards(IFaction faction)
        {
            var cardMan = Locate<ICardManager>();
            for (int i = 0; i < 10; i++)
            {
                faction.Deck.AddCard(faction.CardLoader.Load(cardMan.GetRandomRawCard()));
            }
        }

        private void GenerateMap(IMapManager mapManager, int size, IStructureFactory structureFactory, IFactionManager factionManger)
        {
            _mapGen = new MapGenerator(mapManager, size, new DefaultTerrainDefinition());
            _mapGen.GenerateMap();
            _mapGen.PopulateMap(mapManager, structureFactory, factionManger);
        }
    }
}