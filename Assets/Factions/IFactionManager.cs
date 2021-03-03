﻿using Assets.Map;
using Assets.Structures;
using System.Collections.Generic;

namespace Assets.Factions
{
    public interface IFactionManager
    {
        void AddFaction(IFaction faction);

        IFaction GetActiveFaction();

        List<IFaction> GetAllFactions();

        void MoveToNextTurn();

        PlayerFaction GetPlayerFaction();

        event FactionDelegates.OnTurnEnded OnTurnEnded;

        event FactionDelegates.OnTurnStarted OnTurnStarted;

        IFaction GetOwnerOfStructure(IStructure structure);

        bool TryGetStructureAtCoord(Coord coord, out IStructure structure);

        IFaction GetNatureFaction();
    }
}