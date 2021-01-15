using Assets.Map;
using Assets.Resources;
using Assets.Structures;
using Assets.Structures.Behaviors;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Factions
{
    public class StructureManager : IStructureManager
    {
        private readonly List<IStructure> _structures;
        private readonly ISpawnManager _spawnManager;
        private readonly IStructureFactory _structureFactory;

        private readonly IStructurePlacementValidator _structurePlacementValidator;

        public StructureManager(ISpawnManager spawnManager, IStructureFactory structureFactory, IStructurePlacementValidator structurePlacementValidator)
        {
            _spawnManager = spawnManager;
            _structureFactory = structureFactory;
            _structures = new List<IStructure>();
            _structurePlacementValidator = structurePlacementValidator;
            StructureEventManager.OnStructureDestroyed += RemoveStructure;
        }

        public void AddStructure(IStructureFacade facade, ICoord coord)
        {
            _structures.Add(_structureFactory.MakeStructure(facade, coord));
        }

        public List<IStructureFacade> GetBuildableStructures()
        {
            var facades = new List<IStructureFacade>
            {
                new StructureFacade("Road", 1,1, "Road", "", _structureFactory.GetBehaviour<NoBehavior>(), _structurePlacementValidator.CanPlaceRoad,  (ResourceType.Gold, 1)),
                new StructureFacade("Farm", 2,2, "Barn", "", _structureFactory.GetBehaviour<FarmBehavior>(), _structurePlacementValidator.CanPlaceFarm, (ResourceType.Gold, 3)),
                new StructureFacade("House", 1,1, "House", "", _structureFactory.GetBehaviour<NoBehavior>(), _structurePlacementValidator.CanPlaceDefault, (ResourceType.Gold, 2)),
            };
            return facades;
        }

        public List<IStructure> GetStructures()
        {
            return _structures;
        }

        public void RemoveStructure(IStructure structure)
        {
            if (_structures.Contains(structure))
            {
                _structures.Remove(structure);
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