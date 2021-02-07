﻿using Assets.Map;
using Assets.Structures;
using System.Collections.Generic;

namespace Assets.Factions
{
    public interface IStructureManager
    {
        void AddStructure(StructureType type, ICoord coord);

        void RemoveStructure(IStructure structure);

        List<IStructure> GetStructures();

        void DoTurnEndActions();

        void DoTurnStartActions();

        IPlacementValidator PlacementValidator { get; }

        List<IStructure> GetStructuresLinkedTo(IStructure structure);
    }
}