using Assets.Map;
using Assets.Structures;
using UnityEngine;

namespace Assets.Cards
{
    public class Card : ICard
    {
        private StructureType?[,] _originalStructures;
        private StructureType?[,] _rotatedStructures;
        private IPlacementValidator _structurePlacementValidator;

        public Card(IPlacementValidator structurePlacementValidator, StructureType?[,] structures)
        {
            _originalStructures = structures;
            _rotatedStructures = _originalStructures;
            _structurePlacementValidator = structurePlacementValidator;
        }

        public (int x, int z) GetBasePoint()
        {
            for (var x = 0; x < _rotatedStructures.GetLength(0); x++)
            {
                for (var z = 0; z < _rotatedStructures.GetLength(1); z++)
                {
                    if (_rotatedStructures[x, z] == StructureType.Base)
                    {
                        return (x, z);
                    }
                }
            }
            throw new System.IndexOutOfRangeException();
        }

        public StructureType?[,] GetStructures()
        {
            return _rotatedStructures;
        }

        public void RotateLeft()
        {
            _rotatedStructures = RotateMatrix(_rotatedStructures);
        }

        public void RotateRight()
        {
            RotateLeft();
            RotateLeft();
            RotateLeft();
        }

        private static StructureType?[,] RotateMatrix(StructureType?[,] matrix)
        {
            var n = matrix.GetLength(0);
            var ret = new StructureType?[n, n];

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    ret[i, j] = matrix[n - j - 1, i];
                }
            }

            return ret;
        }

        public bool CanPlay(ICoord anchor)
        {
            var matrix = _rotatedStructures;

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int z = 0; z < matrix.GetLength(1); z++)
                {
                    var current = new Coord(anchor.X + x, anchor.Y, anchor.Z + z);
                    var placementResult = _structurePlacementValidator.CanPlace(current, matrix[x, z]);
                    if (!placementResult.CanPlace)
                    {
                        Debug.Log($"{current} > {x}:{z} '{placementResult.Message}'");
                        return false;
                    }
                }
            }

            return true;
        }

        public override string ToString()
        {
            var str = "";

            var len = _rotatedStructures.GetLength(0);
            for (int z = len-1; z >= 0; z--)
            {
                for (int x = 0; x < len; x++)
                {
                    var structure = _rotatedStructures[x, z];
                    if (structure.HasValue)
                    {
                        str += structure.Value.ToString().Substring(0, 1);
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

        public ICoord GetRelativeAnchorPoint(ICoord anchor)
        {
            var (x, z) = GetBasePoint();
            return new Coord(anchor.X - x, anchor.Y, anchor.Z - z);
        }
    }
}