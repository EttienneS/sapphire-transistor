using Assets.Factions;
using Assets.Map;
using Assets.ServiceLocator;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Structures.Cards
{
    public class CardManager : GameServiceBase, ICardManager
    {
        private List<string> _rawOptions;
        private IPlacementValidator _structurePlacementValidator;
        private IMapManager _mapManager;
        private IFactionManager _factionManager;

        public ICard GetRandomCard()
        {
            return FromString(_rawOptions[0], _structurePlacementValidator);
        }

        public ICard FromString(string input, IPlacementValidator placementValidator)
        {
            var lines = input.Split('\n');

            var legend = new Dictionary<char, StructureType>();
            var mapmode = false;

            StructureType?[,] structures = null;

            var x = 0;
            var y = 0;
            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (mapmode)
                {

                    foreach (var character in trimmedLine)
                    {
                        if (character != '.')
                        {
                            structures[x, y] = legend[character];
                        }
                        x++;
                    }
                    x = 0;
                    y++;
                }
                else
                {
                    if (trimmedLine.StartsWith("="))
                    {
                        mapmode = true;
                        structures = new StructureType?[trimmedLine.Length, trimmedLine.Length];
                    }
                    else
                    {
                        var parts = trimmedLine.Split('=');
                        legend.Add(parts[0][0], (StructureType)Enum.Parse(typeof(StructureType), parts[1]));
                    }
                }
            }

            return new Card(placementValidator, structures);
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
                Addressables.Release(cardObject);
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

        public void DealCards(IFaction faction)
        {
            faction.AddCard(GetRandomCard());
        }
    }
}