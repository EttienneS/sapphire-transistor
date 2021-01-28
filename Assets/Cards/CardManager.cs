using Assets.Factions;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.Structures;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Cards
{
    public class CardManager : GameServiceBase, ICardManager
    {
        private List<string> _rawOptions;
        private IPlacementValidator _structurePlacementValidator;
        private IMapManager _mapManager;
        private IFactionManager _factionManager;

        public ICard GetRandomCard()
        {
            var opt = _rawOptions[Random.Range(0, _rawOptions.Count)];
            return CardLoader.FromString(opt, _structurePlacementValidator);
        }

        public override void Initialize()
        {
            _structurePlacementValidator = Locate<IPlacementValidator>();
            _factionManager = Locate<IFactionManager>();
            _mapManager = Locate<IMapManager>();
            _rawOptions = new List<string>();

            foreach (var cardObject in Resources.LoadAll<TextAsset>("Cards"))
            {
                Debug.Log($"Card Loaded: {cardObject.name}");
                _rawOptions.Add(cardObject.text);
            }

            CardEventManager.OnCardPlayed += HandleCardPlayed;
        }

        private void HandleCardPlayed(ICard card, IFaction faction, ICoord coord)
        {
            var matrix = card.GetStructures();
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int z = 0; z < matrix.GetLength(1); z++)
                {
                    if (matrix[x, z].HasValue)
                    {
                        var adjustedCoord = new Coord(coord.X + x, coord.Y, coord.Z + z);

                        if (_mapManager.TryGetCellAtCoord(adjustedCoord, out Cell cell))
                        {
                            if (_factionManager.TryGetStructureInCell(cell, out IStructure structure))
                            {
                                _factionManager.GetOwnerOfStructure(structure)
                                               .StructureManager
                                               .RemoveStructure(structure);
                            }
                            faction.StructureManager.AddStructure(matrix[x, z].Value, adjustedCoord);
                        }
                    }
                }
            }
        }
    }
}