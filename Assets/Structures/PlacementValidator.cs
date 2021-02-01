using Assets.Factions;
using Assets.Helpers;
using Assets.Map;
using Assets.ServiceLocator;
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
                    case StructureType.Tree:
                    case StructureType.Road:
                        return CellEmptyOrSame(cell, type);

                    default:
                        throw new NotImplementedException();
                }
            }
            throw new IndexOutOfRangeException();
        }

        //public IPlacementResult CanPlaceDefault(Cell cell, StructureType structureToPlace)
        //{
        //    var oneCellHasroad = false;
        //    if (CellEmptyOrSame(cell, structureToPlace))
        //    {
        //        if (HasNeigbourContainingStructure(cell, StructureType.Road))
        //        {
        //            oneCellHasroad = true;
        //        }
        //    }
        //    else
        //    {
        //        return _notEmptyResult;
        //    }

        //    if (oneCellHasroad)
        //    {
        //        return _validResult;
        //    }
        //    return _noRoadResult;
        //}

        //public IPlacementResult CanPlaceFarm(Cell origin)
        //{
        //    var defaultPlacement = CanPlaceDefault(origin, StructureType.Farm);
        //    if (defaultPlacement.CanPlace)
        //    {
        //        if (origin.Terrain.Type == TerrainType.Grass)
        //        {
        //            return GetInvalidTerrainResult(origin.Terrain.Type, TerrainType.Grass);
        //        }
        //        return _validResult;
        //    }
        //    return defaultPlacement;
        //}

        private InvalidPlacementResult GetInvalidTerrainResult(TerrainType current, TerrainType required)
        {
            return new InvalidPlacementResult($"Incorrect terrain: {current} != '{required}'");
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