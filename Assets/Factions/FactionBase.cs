using Assets.Actors;
using System.Collections.Generic;

namespace Assets.Factions
{

    public abstract class FactionBase : IFaction
    {
        private string _name;

        public FactionBase(string name)
        {
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }

        public void AddActor(IActor actor)
        {
            throw new System.NotImplementedException();
        }

        public List<IActor> GetActors()
        {
            throw new System.NotImplementedException();
        }

        public IActor GetFactionHead()
        {
            throw new System.NotImplementedException();
        }

        public List<IStructure> GetStructures()
        {
            throw new System.NotImplementedException();
        }

        public void SetFactionHead(IActor actor)
        {
            throw new System.NotImplementedException();
        }

        public abstract void TakeTurn();
    }
}