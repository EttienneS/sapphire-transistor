using Assets.Factions;
using Assets.Helpers;
using Assets.Map;
using Assets.ServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Structures
{

    public class StructureManager : IStructureManager
    {
        private readonly List<IStructure> _structures;
        private IFactionManager _factionManager;
        private IMapManager _mapManager;
        private IStructureFactory _structureFactory;
        private IStructureDefinitionManager _structureDefinitionManager;

        public StructureManager(IStructureDefinitionManager structureDefinitionManager, IStructureFactory structureFactory, IFactionManager factionManager, IMapManager mapManager)
        {
            _structures = new List<IStructure>();
            _structureFactory = structureFactory;
            _factionManager = factionManager;
            _mapManager = mapManager;
            _structureDefinitionManager = structureDefinitionManager;
        }

        public List<StructureDefinition> StructureDefinitions { get; }

        public void AddStructure(StructureDefinition.StructureType type, ICoord coord)
        {
            if (type != StructureDefinition.StructureType.Empty)
            {
                if (_factionManager.TryGetStructureAtCoord(coord, out IStructure structure))
                {
                    _factionManager.GetOwnerOfStructure(structure)
                                   .StructureManager.RemoveStructure(structure);
                }
                _structures.Add(_structureFactory.GetStructure(_structureDefinitionManager.GetDefinitionForType(type), coord));
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

        public IStructure GetCore()
        {
            return _structures.First(s => s.Type == StructureDefinition.StructureType.Core);
        }

        public List<IStructure> GetStructures()
        {
            return _structures;
        }

        public List<IStructure> GetStructuresLinkedTo(IStructure structure)
        {
            var network = new List<IStructure>();

            using (Instrumenter.Start())
            {
                var frontier = new Queue<ICoord>();
                frontier.Enqueue(structure.GetOrigin());

                var closed = new List<ICoord>();
                while (frontier.Count > 0)
                {
                    var current = frontier.Dequeue();
                    if (closed.Contains(current))
                    {
                        continue;
                    }
                    closed.Add(current);

                    if (TryGetStructureAtCoord(current, out IStructure currentStructure))
                    {
                        foreach (var coord in currentStructure.OccupiedCoords)
                        {
                            if (TryGetStructureAtCoord(coord, out IStructure linkedStructure))
                            {
                                if (!network.Contains(linkedStructure))
                                {
                                    network.Add(linkedStructure);
                                }

                                if (linkedStructure.Type == StructureDefinition.StructureType.Road || linkedStructure.Type == StructureDefinition.StructureType.Core)
                                {
                                    if (_mapManager.TryGetCellAtCoord(linkedStructure.GetOrigin(), out Cell cell))
                                    {
                                        foreach (var linkedCell in cell.GetCardinalNeighbours())
                                        {
                                            if (!closed.Contains(linkedCell.Coord))
                                            {
                                                frontier.Enqueue(linkedCell.Coord);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return network;
        }

        public void RemoveStructure(IStructure structure)
        {
            if (_structures.Contains(structure))
            {
                _structures.Remove(structure);
                StructureEventManager.StructureDestroyed(structure);
            }
        }

        public bool TryGetStructureAtCoord(ICoord coord, out IStructure structure)
        {
            structure = _structures.Find(s => s.OccupiedCoords.Contains(coord));

            return structure != null;
        }
    }
}