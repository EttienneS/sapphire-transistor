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

        private readonly IStructurePlacementValidator _structurePlacementValidator; 

        public StructureManager(ISpawnManager spawnManager, IStructureFactory structureFactory, IStructurePlacementValidator structurePlacementValidator)
        {
            _spawnManager = spawnManager;
            _structureFactory = structureFactory;
            _structureObjectLookup = new Dictionary<IStructure, GameObject>();
            _structurePlacementValidator = structurePlacementValidator;
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
                new StructureFacade("Road", "Road", "", _structureFactory.GetBehaviour<NoBehavior>(), _structurePlacementValidator.CanPlaceRoad,  (ResourceType.Gold, 1)),
                new StructureFacade("Farm", "Barn", "", _structureFactory.GetBehaviour<FarmBehavior>(), _structurePlacementValidator.CanPlaceFarm, (ResourceType.Gold, 3)),
                new StructureFacade("House", "House", "", _structureFactory.GetBehaviour<NoBehavior>(), _structurePlacementValidator.CanPlaceDefault, (ResourceType.Gold, 2)),
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
                _spawnManager.RecyleItem(structure.Name, _structureObjectLookup[structure]);
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