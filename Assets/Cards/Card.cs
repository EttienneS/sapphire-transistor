using Assets.Cards.Actions;
using Assets.Factions;
using Assets.Map;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Cards
{
    public class Card : ICard
    {
        private readonly Dictionary<ResourceType, int> _cost;
        private (int x, int z) _basePoint;
        private ICardAction[,] _originalActions;
        private ICardAction[,] _rotatedActions;

        public Card(string name, (int x, int z) basePoint, ICardAction[,] actions, Dictionary<ResourceType, int> cost)
        {
            Name = name;
            _originalActions = actions;
            _rotatedActions = _originalActions;
            _basePoint = basePoint;
            _cost = cost;
        }

        public string Name { get; }

        public bool CanPlay(ICoord anchor)
        {
            var matrix = _rotatedActions;

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int z = 0; z < matrix.GetLength(1); z++)
                {
                    var current = new Coord(anchor.X + x, anchor.Y, anchor.Z + z);
                    var cardAction = matrix[x, z];
                    if (cardAction?.CanPlay(current) == false)
                    {
                        Debug.Log($"{current} > {x}:{z} cant play!");
                        return false;
                    }
                }
            }

            return true;
        }

        public void ClearPreview()
        {
            foreach (var action in GetActions())
            {
                if (action != null)
                {
                    action.ClearPreview();
                }
            }
        }

        public ICardAction[,] GetActions()
        {
            return _rotatedActions;
        }

        public (int x, int z) GetBasePoint()
        {
            return _basePoint;
        }

        public Dictionary<ResourceType, int> GetCost()
        {
            return _cost;
        }

        public ICoord GetRelativeAnchorPoint(ICoord anchor)
        {
            var (x, z) = GetBasePoint();
            return new Coord(anchor.X - x, anchor.Y, anchor.Z - z);
        }

        public void Play(ICoord baseCoord)
        {
            foreach (var (cardAction, coord) in GetExecutionList(baseCoord))
            {
                cardAction.Play(coord);
                cardAction.ClearPreview();
            }

            CardEventManager.CardPlayed(this, baseCoord);
        }

        public void Preview(ICoord baseCoord)
        {
            ClearPreview();
            foreach (var (cardAction, coord) in GetExecutionList(baseCoord))
            {
                cardAction.Preview(coord);
            }
            CardEventManager.CardPreviewed(this, baseCoord);
        }

        public override string ToString()
        {
            var str = "";

            var len = _rotatedActions.GetLength(0);
            for (int z = len - 1; z >= 0; z--)
            {
                for (int x = 0; x < len; x++)
                {
                    var action = _rotatedActions[x, z];
                    if (action != null)
                    {
                        str += action.ToString().Substring(0, 1);
                    }
                    else
                    {
                        str += "_";
                    }
                }
                str += "\n";
            }

            return str;
        }

        private List<(ICardAction cardAction, ICoord coord)> GetExecutionList(ICoord coord)
        {
            var executionList = new List<(ICardAction cardAction, ICoord coord)>();
            var actions = GetActions();

            for (int x = 0; x < actions.GetLength(0); x++)
            {
                for (int z = 0; z < actions.GetLength(1); z++)
                {
                    var action = actions[x, z];
                    if (action != null)
                    {
                        var adjustedCoord = new Coord(coord.X + x, coord.Y, coord.Z + z);
                        executionList.Add((action, adjustedCoord));
                    }
                }
            }

            return executionList;
        }
    }
}