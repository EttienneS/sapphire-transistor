﻿using Assets.Map;
using UnityEngine;

namespace Assets.Structures.Cards
{
    public class Card : ICard
    {
        private (int x, int z) _anchorPoint;
        private StructureType?[,] _originalStructures;
        private StructureType?[,] _rotatedStructures;
        private IPlacementValidator _structurePlacementValidator;

        public Card(IPlacementValidator structurePlacementValidator, StructureType?[,] structures)
        {
            _originalStructures = structures;
            _rotatedStructures = _originalStructures;
            _structurePlacementValidator = structurePlacementValidator;
        }

        public (int x, int z) GetAnchorPoint()
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
            RotateRight();
            RotateRight();
            RotateRight();
        }

        public void RotateRight()
        {
            _rotatedStructures = RotateMatrix(_rotatedStructures);
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

        public bool CanPlay(ICoord coord)
        {
            var matrix = _rotatedStructures;
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int z = 0; z < matrix.GetLength(1); z++)
                {
                    var placementResult = _structurePlacementValidator.CanPlace(new Coord(coord.X + x, coord.Y, coord.Z + z), matrix[x, z]);
                    if (!placementResult.CanPlace)
                    {
                        Debug.Log($"{coord} > {x}:{z} '{placementResult.Message}'");
                        return false;
                    }
                }
            }

            return true;
        }
    }
}