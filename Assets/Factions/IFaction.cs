using Assets.Actors;
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
    }
}