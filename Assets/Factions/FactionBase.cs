using Assets.Actors;
using Assets.Structures;
using Assets.Structures.Behaviors;
using System.Collections.Generic;

namespace Assets.Factions
{
    public abstract class FactionBase : IFaction
    {
        private IStructureFactory _structureFactory;
        public FactionBase(string name, IStructureFactory structureFactory)
        {
            Name = name;

            _structureFactory = structureFactory;
        }

        public string Name { get; }
        public void AddActor(IActor actor)
        {
            throw new System.NotImplementedException();
        }

        public List<IActor> GetActors()
        {
            throw new System.NotImplementedException();
        }

        public List<IStructureFacade> GetBuildableStructures()
        {
            var facades = new List<IStructureFacade>();
            facades.Add(new StructureFacade("Farm", "", _structureFactory.GetBehaviour<FarmBehavior>()));
            return facades;
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