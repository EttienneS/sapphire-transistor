using Assets.Factions;
using Assets.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Cards
{
    public class CardLoader : ICardLoader
    {
        private IFaction _owner;

        public CardLoader(IFaction owner)
        {
            _owner = owner;
        }

        // example card:
        // R=Road
        // A=Anchor
        // B=Base
        // ====
        // BRRA
        // R...
        // R...
        // A...

        public delegate ICardAction MakeCardActionDelegate();

        public ICard Load(string input)
        {
            var name = GetName(input);
            var lenght = GetLenght(input);
            var actions = new ICardAction[lenght, lenght];
            var basePoint = GetBasePoint(input);
            var legend = GetLegend(input);
            var map = GetCardMapLines(input);

            for (int z = 0; z < lenght; z++)
            {
                for (int x = 0; x < lenght; x++)
                {
                    actions[x, z] = legend[map[z][x]]?.Invoke();
                }
            }

            return new Card(name, basePoint, actions);
        }

        private (int x, int z) GetBasePoint(string input)
        {
            var value = SplitCard(input).Find(l => l.StartsWith("Base="));
            if (value == null)
            {
                return (0, 0);
            }

            var parts = value.Split('=')[0].Split(',');
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        }

        private List<string> GetCardMapLines(string card)
        {
            var lines = SplitCard(card);
            var mapLines = new List<string>();
            var mapMode = false;
            foreach (var line in lines)
            {
                if (line.StartsWith("="))
                {
                    mapMode = true;
                }
                else if (mapMode)
                {
                    mapLines.Add(line);
                }
            }
            mapLines.Reverse();
            return mapLines;
        }

        private Dictionary<char, MakeCardActionDelegate> GetLegend(string card)
        {
            var lines = SplitCard(card);
            var legend = new Dictionary<char, MakeCardActionDelegate>();

            var legendMode = false;

            foreach (var line in lines)
            {
                if (line.StartsWith("-"))
                {
                    legendMode = true;
                }
                else if (line.StartsWith("="))
                {
                    break;
                }
                else
                {
                    if (legendMode)
                    {
                        var parts = line.Split('=');
                        legend.Add(parts[0][0], () => new BuildAction((StructureType)Enum.Parse(typeof(StructureType), parts[1]), _owner));
                    }
                }
            }

            legend.Add('.', null);
            legend.Add('#', null);

            return legend;
        }

        private int GetLenght(string card)
        {
            return SplitCard(card).First(line => line.StartsWith("=")).Length;
        }

        private string GetName(string input)
        {
            return SplitCard(input).First(l => l.StartsWith("Name=")).Split('=')[1];
        }
        private List<string> SplitCard(string card)
        {
            return card.Split('\n').Select(s => s.Trim()).ToList();
        }
    }
}