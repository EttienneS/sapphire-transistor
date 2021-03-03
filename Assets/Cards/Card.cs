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

        public Card(string name, CardColor color, (int x, int z) basePoint, ICardAction[,] actions, Dictionary<ResourceType, int> cost)
        {
            Name = name;
            Color = color;
            _originalActions = actions;
            _rotatedActions = _originalActions;
            _basePoint = basePoint;
            _cost = cost;
        }

        public CardColor Color { get; }
        public string Name { get; }

        public bool CanPlay(Coord anchor)
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

        public Coord GetRelativeAnchorPoint(Coord anchor)
        {
            var (x, z) = GetBasePoint();
            return new Coord(anchor.X - x, anchor.Y, anchor.Z - z);
        }

        public void Play(Coord baseCoord)
        {
            foreach (var (cardAction, coord) in GetExecutionList(baseCoord))
            {
                cardAction.ClearPreview();

                cardAction.Play(coord);
            }

            CardEventManager.CardPlayed(this, baseCoord);
        }

        public void Preview(Coord baseCoord)
        {
            ClearPreview();
            foreach (var (cardAction, coord) in GetExecutionList(baseCoord))
            {
                cardAction.Preview(coord);
            }
            CardEventManager.CardPreviewed(this, baseCoord);
        }

        public void RotateCCW()
        {
            RotateCW();
            RotateCW();
            RotateCW();
        }

        public void RotateCW()
        {
            _rotatedActions = ReverseRows(Transpose(_rotatedActions));
            CardEventManager.CardRotated(this);
        }

        private static ICardAction[,] ReverseRows(ICardAction[,] matrix)
        {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);

            var result = new ICardAction[h, w];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[i, j] = matrix[i, h - j - 1];
                }
            }

            return result;
        }

        private static ICardAction[,] Transpose(ICardAction[,] matrix)
        {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);

            ICardAction[,] result = new ICardAction[h, w];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }

            return result;
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

        private List<(ICardAction cardAction, Coord coord)> GetExecutionList(Coord coord)
        {
            var executionList = new List<(ICardAction cardAction, Coord coord)>();
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