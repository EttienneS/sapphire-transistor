using Assets.Factions;
using Assets.Helpers;
using Assets.Map;
using System;

namespace Assets.Structures
{
    public class PlacementValidator : IPlacementValidator
    {
        private IFactionManager _factionManager;
        private IMapManager _mapManager;

        private readonly InvalidPlacementResult _noRoadResult = new InvalidPlacementResult("Cell does not have a neighbouring road!");
        private readonly InvalidPlacementResult _notEmptyResult = new InvalidPlacementResult("Cell is not empty!");
        private readonly ValidPlacementResult _validResult = new ValidPlacementResult();

        public PlacementValidator(IFactionManager factionManager, IMapManager mapManager)
        {
            _factionManager = factionManager;
            _mapManager = mapManager;
        }

        public IPlacementResult CanPlace(ICoord coord, StructureType? structureType)
        {
            if (_mapManager.TryGetCellAtCoord(coord, out Cell cell))
            {
                if (structureType == null)
                {
                    return _validResult;
                }
                var type = structureType.Value;
                switch (type)
                {
                    case StructureType.House:
                    case StructureType.Rock:
                    case StructureType.Tree:
                    case StructureType.Road:
                    case StructureType.Barn:
                        return CellEmptyOrSame(cell, type);

                    case StructureType.Field:
                        return EmptyAndTerrainMatches(cell, type, TerrainType.Grass);

                    case StructureType.Empty:
                        return CellEmpty(cell);

                    default:
                        throw new NotImplementedException();
                }
            }
            throw new IndexOutOfRangeException();
        }

        private IPlacementResult CellEmpty(Cell cell)
        {
            if (_factionManager.TryGetStructureInCell(cell, out IStructure structure))
            {
                return _notEmptyResult;
            }
            return _validResult;
        }

        private IPlacementResult EmptyAndTerrainMatches(Cell cell, StructureType type, TerrainType required)
        {
            var empty = CellEmptyOrSame(cell, type);
            if (empty.CanPlace)
            {
                if (cell.Terrain.Type == TerrainType.Grass)
                {
                    return _validResult;
                }
                else
                {
                    return new InvalidPlacementResult($"Incorrect terrain: {cell.Terrain.Type} != '{required}'");
                }
            }
            return empty;
        }

        private IPlacementResult CellEmptyOrSame(Cell cell, StructureType structureToPlace)
        {
            if (_factionManager.TryGetStructureInCell(cell, out IStructure structure))
            {
                if (structure.Type == structureToPlace)
                {
                    return _validResult;
                }
                return _notEmptyResult;
            }
            return _validResult;
        }

        private bool HasNeigbourContainingStructure(Cell cell, StructureType type)
        {
            foreach (var neighbour in cell.GetCardinalNeighbours())
            {
                if (_factionManager.TryGetStructureInCell(neighbour, out IStructure structure)
                    && structure.Type == type)
                {
                    return true;
                }
            }

            return false;
        }
    }
}