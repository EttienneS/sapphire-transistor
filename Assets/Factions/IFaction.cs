using Assets.Actors;
using Assets.Structures;
using System.Collections.Generic;

namespace Assets.Factions
{
    public interface IFaction
    {
        string GetName();

        IActor GetFactionHead();

        List<IActor> GetActors();

        void AddActor(IActor actor);

        void SetFactionHead(IActor actor);

        List<IStructure> GetStructures();

        void TakeTurn();
    }
}