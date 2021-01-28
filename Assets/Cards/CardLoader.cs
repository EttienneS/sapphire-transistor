using Assets.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Cards
{
    public static class CardLoader
    {
        // example card:
        // R=Road
        // A=Anchor
        // B=Base
        // ====
        // BRRA
        // R...
        // R...
        // A...

        public static ICard FromString(string input, IPlacementValidator placementValidator)
        {
            var lenght = GetLenght(input);
            var structures = new StructureType?[lenght, lenght];

            var legend = GetLegend(input);
            var map = GetCardMapLines(input);

            for (int z = 0; z < lenght; z++)
            {
                for (int x = 0; x < lenght; x++)
                {
                    structures[x, z] = legend[map[z][x]];
                }
            }

            return new Card(placementValidator, structures);
        }

        private static List<string> GetCardMapLines(string card)
        {
            var lines = SplitCard(card);
            var mapLines = new List<string>();
            var map = false;
            foreach (var line in lines)
            {
                if (line.StartsWith("="))
                {
                    map = true;
                }
                else if (map)
                {
                    mapLines.Add(line);
                }
            }
            mapLines.Reverse();
            return mapLines;
        }

        private static Dictionary<char, StructureType?> GetLegend(string card)
        {
            var lines = SplitCard(card);
            var legend = new Dictionary<char, StructureType?>();

            foreach (var line in lines)
            {
                if (line.StartsWith("="))
                {
                    break;
                }
                else
                {
                    var parts = line.Split('=');
                    legend.Add(parts[0][0], (StructureType)Enum.Parse(typeof(StructureType), parts[1]));
                }
            }

            legend.Add('.', null);
            legend.Add('#', null);

            return legend;
        }

        private static int GetLenght(string card)
        {
            return SplitCard(card).First(line => line.StartsWith("=")).Length;
        }

        private static List<string> SplitCard(string card)
        {
            return card.Split('\n').Select(s => s.Trim()).ToList();
        }
    }
}