using Assets.Factions;
using Assets.Map;
using Assets.ServiceLocator;
using System;
using System.Collections.Generic;

namespace Assets.Structures.Cards
{
    public class CardManager : GameServiceBase, ICardManager
    {
        private List<ICard> _options;
        private IPlacementValidator _structurePlacementValidator;
        

        public ICard GetRandomCard()
        {
            return _options[0];
        }

        public ICard FromString(string input, IPlacementValidator placementValidator)
        {
            // example:
            // R=Road
            // A=Anchor
            // =======
            // ...R...
            // ...R...
            // ...RRR.
            // ...R...
            // ...A...
            var lines = input.Split('\n');

            var legend = new Dictionary<char, StructureType>();
            var mapmode = false;

            StructureType?[,] structures = null;
            (int x, int y)? anchorPoint = default;

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

                            if (!anchorPoint.HasValue && legend[character] == StructureType.Anchor)
                            {
                                anchorPoint = (x, y);
                            }
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

            return new Card(anchorPoint.Value, placementValidator, structures);
        }

        public override void Initialize()
        {
            _structurePlacementValidator = Locate<IPlacementValidator>();
            _options = new List<ICard>()
            {
                FromString(@"R=Road
                             A=Anchor
                             =====
                             ARRRA
                             R....
                             R....
                             R....
                             A....",_structurePlacementValidator)
            };

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
                        faction.StructureManager.AddStructure(matrix[x, z].Value, adjustedCoord);
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