using Assets.Map;
using Assets.Resources;
using Assets.Structures;
using Assets.Structures.Behaviors;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Factions
{
    public class StructureManager : IStructureManager
    {
        private readonly Dictionary<IStructure, GameObject> _structureObjectLookup;
        private readonly ISpawnManager _spawnManager;
        private readonly IStructureFactory _structureFactory;

        public StructureManager(ISpawnManager spawnManager, IStructureFactory structureFactory)
        {
            _spawnManager = spawnManager;
            _structureFactory = structureFactory;
            _structureObjectLookup = new Dictionary<IStructure, GameObject>();

            StructureEventManager.OnStructureDestroyed += RemoveStructure;
        }

        public ICoord GetFactionCoreLocation()
        {
            return _structureObjectLookup.Keys.First().Coord;
        }

        public void AddStructure(IStructureFacade selectedFacade, ICoord coord)
        {
            var structure = _structureFactory.MakeStructure(selectedFacade, coord);
            _structureObjectLookup.Add(structure, null);
            _spawnManager.SpawnAddressable(selectedFacade.Address, coord.ToAdjustedVector3(), (obj) => _structureObjectLookup[structure] = obj);
        }

        public List<IStructureFacade> GetBuildableStructures()
        {
            var facades = new List<IStructureFacade>
            {
                new StructureFacade("Farm", "Barn", "", _structureFactory.GetBehaviour<FarmBehavior>(), (ResourceType.Gold, 3)),
                new StructureFacade("House", "House", "", _structureFactory.GetBehaviour<NoBehavior>(), (ResourceType.Gold, 2)),
                new StructureFacade("Road", "Road", "", _structureFactory.GetBehaviour<NoBehavior>(), (ResourceType.Gold, 1))
            };
            return facades;
        }

        public List<IStructure> GetStructures()
        {
            return _structureObjectLookup.Keys.ToList();
        }

        public void RemoveStructure(IStructure structure)
        {
            if (_structureObjectLookup.ContainsKey(structure))
            {
                _spawnManager.AddItemToDestroy(_structureObjectLookup[structure]);
                _structureObjectLookup.Remove(structure);
            }
        }

        public void DoTurnEndActions()
        {
            foreach (var structure in GetStructures())
            {
                structure.TurnEnd(structure);
            }
        }

        public void DoTurnStartActions()
        {
            foreach (var structure in GetStructures())
            {
                structure.TurnStart(structure);
            }
        }

        public List<(ResourceType, int)> GetCombinedYield()
        {
            var yield = new List<(ResourceType, int)>();
            foreach (var structure in GetStructures())
            {
                foreach (var res in structure.GetYield(structure))
                {
                    yield.Add((res.Item1, res.Item2));
                }
            }

            return yield;
        }
    }

}