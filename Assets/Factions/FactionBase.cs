using Assets.Actors;
using Assets.Map;
using Assets.Resources;
using Assets.Structures;
using Assets.Structures.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Factions
{
    public abstract class FactionBase : IFaction
    {
        private readonly Dictionary<IStructure, GameObject> _gameobjectLookup;
        private SpawnManager _spawnManager;

        public void EndTurn()
        {
            TurnEnded?.Invoke(this);
        }

        private IStructureFactory _structureFactory;

        private readonly Dictionary<ResourceType, int> _resources;

        public event FactionDelegates.OnResourceChanged OnResourcesUpdated;
        public event FactionDelegates.OnTurnEnded TurnEnded;

        public FactionBase(string name, IStructureFactory structureFactory, SpawnManager spawnManager)
        {
            _gameobjectLookup = new Dictionary<IStructure, GameObject>();
            _resources = new Dictionary<ResourceType, int>();

            Name = name;

            _structureFactory = structureFactory;
            _spawnManager = spawnManager;
        }

        public string Name { get; }

        public void AddActor(IActor actor)
        {
            throw new System.NotImplementedException();
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

        public void AddStructure(IStructureFacade selectedFacade, ICoord coord)
        {
            var structure = _structureFactory.MakeStructure(selectedFacade, coord);
            _gameobjectLookup.Add(structure, null);

            _spawnManager.SpawnStructure(selectedFacade, coord.ToAdjustedVector3(), (obj) => _gameobjectLookup[structure] = obj);
        }

        public List<IActor> GetActors()
        {
            throw new System.NotImplementedException();
        }

        public List<IStructureFacade> GetBuildableStructures()
        {
            var facades = new List<IStructureFacade>
            {
                new StructureFacade("Farm", "Barn", "", _structureFactory.GetBehaviour<FarmBehavior>())
            };
            return facades;
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
            return _gameobjectLookup.Keys.ToList();
        }

        public void SetFactionHead(IActor actor)
        {
            throw new System.NotImplementedException();
        }

        public abstract void TakeTurn();

        public void DoTurnStartActions()
        {
            foreach (var structure in GetStructures())
            {
                structure.Behaviour.TurnStart(structure);

                AddResources(structure.Behaviour.GetYield(structure));
            }
        }

        private void AddResources((ResourceType resouceType, int amount)[] resouces)
        {
            foreach (var resouce in resouces)
            {
                ModifyResource(resouce.resouceType, resouce.amount);
            }
        }

        public void DoTurnEndActions()
        {
            foreach (var structure in GetStructures())
            {
                structure.Behaviour.TurnEnd(structure);
            }
        }

    }
}