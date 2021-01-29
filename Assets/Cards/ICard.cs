﻿using Assets.Map;
using Assets.Structures;

namespace Assets.Cards
{
    public interface ICard
    {
        string Name { get; }

        (int x, int z) GetBasePoint();

        StructureType?[,] GetStructures();

        bool CanPlay(ICoord coord);

        void RotateCCW();

        void RotateCW();

        ICoord GetRelativeAnchorPoint(ICoord coord);
    }
}