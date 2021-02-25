using Assets.Cards.Actions;
using Assets.Factions;
using Assets.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.Cards
{
    public class CardLoader
    {
        private List<string> _rawOptions;
        private IFaction _owner;

        public CardLoader(IFaction owner)
        {
            _owner = owner;
            _rawOptions = new List<string>();
            foreach (var cardObject in Resources.LoadAll<TextAsset>("Cards"))
            {
                Debug.Log($"Card Loaded: {cardObject.name}");
                _rawOptions.Add(cardObject.text);
            }
        }

        public delegate ICardAction MakeCardActionDelegate();

        public ICard Load(string input, IFaction _owner)
        {
            var lenght = ParseLenght(input);
            var actions = new ICardAction[lenght, lenght];
            var basePoint = ParseBasePoint(input);
            var legend = ParseLegend(input, _owner);
            var map = GetCardMapLines(input);

            for (int z = 0; z < lenght; z++)
            {
                for (int x = 0; x < lenght; x++)
                {
                    actions[x, z] = legend[map[z][x]]?.Invoke();
                }
            }

            var name = ParseName(input);
            var color = ParseColor(input);
            var cost = ParseCost(input);

            return new Card(name, color, basePoint, actions, cost);
        }

        internal IEnumerable<ICard> GetAvailableCards()
        {
            var cards = new List<ICard>();

            foreach (var option in _rawOptions)
            {
                cards.Add(Load(option, _owner));
            }

            return cards;
        }

        public MakeCardActionDelegate ParseAction(string action, IFaction owner)
        {
            var actionParts = action.Split(new[] { ' ' }, 2);
            var verb = actionParts[0];
            var value = string.Empty;

            if (actionParts.Length > 1)
            {
                value = actionParts[1];
            }

            return (verb.ToLower()) switch
            {
                "build" => () => new BuildAction((StructureType)Enum.Parse(typeof(StructureType), value), owner),
                "remove" => () => new RemoveAction(),
                _ => throw new KeyNotFoundException($"Unkown verb: {action}"),
            };
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

        private string GetProperty(string input, string name)
        {
            return SplitCard(input).First(l => l.StartsWith($"{name}=")).Split('=')[1];
        }

        private (int x, int z) ParseBasePoint(string input)
        {
            var value = SplitCard(input).Find(l => l.StartsWith("Base="));
            if (value == null)
            {
                return (0, 0);
            }

            var parts = value.Split('=')[0].Split(',');
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        }

        private CardColor ParseColor(string input)
        {
            return (CardColor)Enum.Parse(typeof(CardColor), GetProperty(input, "Color"));
        }

        private Dictionary<ResourceType, int> ParseCost(string input)
        {
            // Cost=1G,1S
            var costString = GetProperty(input, "Cost");

            var cost = new Dictionary<ResourceType, int>();
            foreach (var part in costString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var currencyString = Regex.Match(part, @"\D+").ToString();
                var amount = int.Parse(part.Replace(currencyString, string.Empty));

                var currency = ResourceType.Gold;
                currency = (currencyString.ToUpper()) switch
                {
                    "G" => ResourceType.Gold,
                    "S" => ResourceType.Stone,
                    "F" => ResourceType.Food,
                    "W" => ResourceType.Wood,
                    _ => throw new KeyNotFoundException($"Unkown type ':{currencyString}'"),
                };
                cost.Add(currency, amount);
            }

            return cost;
        }

        private Dictionary<char, MakeCardActionDelegate> ParseLegend(string card, IFaction owner)
        {
            var lines = SplitCard(card);
            var legend = new Dictionary<char, MakeCardActionDelegate>();

            var legendMode = false;

            legend.Add('.', null);
            legend.Add('#', () => new BuildAction(StructureType.Empty, owner));

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
                        legend.Add(parts[0][0], ParseAction(parts[1], owner));
                    }
                }
            }

            return legend;
        }

        private int ParseLenght(string card)
        {
            return SplitCard(card).First(line => line.StartsWith("=")).Length;
        }

        private string ParseName(string input)
        {
            return GetProperty(input, "Name");
        }

        private List<string> SplitCard(string card)
        {
            return card.Split('\n').Select(s => s.Trim()).ToList();
        }
    }
}