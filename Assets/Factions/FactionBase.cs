using Assets.Actors;
using Assets.Map;
using Assets.Resources;
using Assets.Structures;
using Assets.Structures.Behaviors;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Factions
{
    public abstract class FactionBase : IFaction
    {
        private readonly Dictionary<ResourceType, int> _resources;
        private readonly SpawnManager _spawnManager;
        private readonly IStructureFactory _structureFactory;
        private readonly Dictionary<IStructure, GameObject> _structureObjectLookup;

        public FactionBase(string name, IStructureFactory structureFactory, SpawnManager spawnManager)
        {
            _structureObjectLookup = new Dictionary<IStructure, GameObject>();
            _resources = new Dictionary<ResourceType, int>();

            Name = name;

            _structureFactory = structureFactory;
            _spawnManager = spawnManager;
        }

        public event FactionDelegates.OnResourceChanged OnResourcesUpdated;

        public event FactionDelegates.OnTurnEnded TurnEnded;

        public event FactionDelegates.OnTurnStarted TurnStarted;

        public string Name { get; }

        public void AddActor(IActor actor)
        {
            throw new System.NotImplementedException();
        }

        public void AddStructure(IStructureFacade selectedFacade, ICoord coord)
        {
            var structure = _structureFactory.MakeStructure(selectedFacade, coord);
            _structureObjectLookup.Add(structure, null);

            _spawnManager.SpawnStructure(selectedFacade, coord.ToAdjustedVector3(), (obj) => _structureObjectLookup[structure] = obj);
        }

        public void DoTurnEndActions()
        {
            foreach (var structure in GetStructures())
            {
                structure.Behaviour.TurnEnd(structure);
            }
        }

        public void DoTurnStartActions()
        {
            foreach (var structure in GetStructures())
            {
                structure.Behaviour.TurnStart(structure);

                AddResources(structure.Behaviour.GetYield(structure));
            }

            TurnStarted?.Invoke(this);
        }

        public void EndTurn()
        {
            TurnEnded?.Invoke(this);
        }

        public List<IActor> GetActors()
        {
            throw new System.NotImplementedException();
        }

        public List<IStructureFacade> GetBuildableStructures()
        {
            var facades = new List<IStructureFacade>
            {
                new StructureFacade("Farm", "Barn", "", _structureFactory.GetBehaviour<FarmBehavior>()),
                new StructureFacade("Road", "Road", "", _structureFactory.GetBehaviour<NoBehavior>()),
                new StructureFacade("House", "Placeholder", "", _structureFactory.GetBehaviour<NoBehavior>()),
            };
            return facades;
        }

        public ICoord GetFactionCoreLocation()
        {
            return _structureObjectLookup.Keys.First().Coord;
        }

        public IActor GetFactionHead()
        {
            throw new System.NotImplementedException();
        }

        public Dictionary<ResourceType, int> GetResources()
        {
            return _resources;
        }

        public List<IStructure> GetStructures()
        {
            return _structureObjectLookup.Keys.ToList();
        }

        public void ModifyResource(ResourceType resource, int amount)
        {
            if (!_resources.ContainsKey(resource))
            {
                _resources.Add(resource, 0);
            }
            _resources[resource] += amount;

            OnResourcesUpdated?.Invoke(resource, _resources[resource]);
        }

        public void SetFactionHead(IActor actor)
        {
            throw new System.NotImplementedException();
        }

        public abstract void TakeTurn();

        private void AddResources((ResourceType resouceType, int amount)[] resouces)
        {
            foreach (var resouce in resouces)
            {
                ModifyResource(resouce.resouceType, resouce.amount);
            }
        }
    }
}