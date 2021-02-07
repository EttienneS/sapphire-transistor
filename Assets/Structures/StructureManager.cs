using Assets.Factions;
using Assets.Map;
using Assets.Map.Pathing;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Structures
{
    public class StructureManager : IStructureManager
    {
        private readonly List<IStructure> _structures;
        private IStructureFactory _structureFactory;
        private IMapManager _mapManager;

        public StructureManager(IStructureFactory structureFactory, IFactionManager factionManager, IMapManager mapManager)
        {
            _structures = new List<IStructure>();
            _structureFactory = structureFactory;
            _mapManager = mapManager;
            PlacementValidator = new PlacementValidator(factionManager, _mapManager);
        }


        public IPlacementValidator PlacementValidator { get; }

        public void AddStructure(StructureType type, ICoord coord)
        {
            if (type != StructureType.Empty)
            {
                _structures.Add(_structureFactory.GetStructure(type, coord));
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



        public List<IStructure> GetStructures()
        {
            return _structures;
        }

        public List<IStructure> GetStructuresLinkedTo(IStructure structure)
        {
            return _structures.Where(s => s.Connected).ToList();
        }

        public void RemoveStructure(IStructure structure)
        {
            if (_structures.Contains(structure))
            {
                _structures.Remove(structure);
                StructureEventManager.StructureDestroyed(structure);
            }
        }
    }
}