using Assets.Map;
using Assets.Resources;
using Assets.Structures;
using System.Collections.Generic;

namespace Assets.Factions
{
    public class StructureManager : IStructureManager
    {
        private readonly List<IStructure> _structures;
        private IStructureFactory _structureFactory;

        public StructureManager(IStructureFactory structureFactory)
        {
            _structures = new List<IStructure>();
            _structureFactory = structureFactory;
            StructureEventManager.OnStructureDestroyed += RemoveStructure;
        }

        public void AddStructure(StructureType type, ICoord coord)
        {
            _structures.Add(_structureFactory.GetStructure(type, coord));
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
    }
}