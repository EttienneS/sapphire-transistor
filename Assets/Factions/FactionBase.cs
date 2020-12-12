using Assets.Actors;
using Assets.Map;
using Assets.Structures;
using Assets.Structures.Behaviors;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Factions
{
    public abstract class FactionBase : IFaction
    {
        private readonly Dictionary<IStructure, GameObject> _gameobjectLookup;
        private SpawnManager _spawnManager;
        private IStructureFactory _structureFactory;

        public FactionBase(string name, IStructureFactory structureFactory, SpawnManager spawnManager)
        {
            _gameobjectLookup = new Dictionary<IStructure, GameObject>();
            Name = name;

            _structureFactory = structureFactory;
            _spawnManager = spawnManager;
        }

        public string Name { get; }

        public void AddActor(IActor actor)
        {
            throw new System.NotImplementedException();
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
            var facades = new List<IStructureFacade>();
            facades.Add(new StructureFacade("Farm", "Barn", "", _structureFactory.GetBehaviour<FarmBehavior>()));
            return facades;
        }

        public IActor GetFactionHead()
        {
            throw new System.NotImplementedException();
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
    }
}