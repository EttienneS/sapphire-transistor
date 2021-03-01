using Assets.Factions;
using Assets.Map;
using Assets.ServiceLocator;
using System;

namespace Assets.Structures
{
    public class PlacementValidator : GameServiceBase, IPlacementValidator
    {
        private Lazy<IFactionManager> _factionManager;
        private Lazy<IMapManager>  _mapManager;

        private readonly InvalidPlacementResult _noRoadResult = new InvalidPlacementResult("Cell does not have a neighbouring road!");
        private readonly InvalidPlacementResult _notEmptyResult = new InvalidPlacementResult("Cell is not empty!");
        private readonly ValidPlacementResult _validResult = new ValidPlacementResult();

        //public IPlacementResult CanPlace(ICoord coord, StructureDefinition.StructureType? structureType)
        //{
        //    if (_mapManager.TryGetCellAtCoord(coord, out Cell cell))
        //    {
        //        if (structureType == null)
        //        {
        //            return _validResult;
        //        }
        //        var type = structureType.Value;
        //        switch (type)
        //        {
        //            case StructureDefinition.StructureType.House:
        //            case StructureDefinition.StructureType.Rock:
        //            case StructureDefinition.StructureType.Tree:
        //            case StructureDefinition.StructureType.Road:
        //            case StructureDefinition.StructureType.Barn:
        //                return CellEmptyOrSame(cell, type);

        //            case StructureDefinition.StructureType.Cabin:
        //                return EmptyAndTerrainMatches(cell, type, TerrainType.Forrest);

        //            case StructureDefinition.StructureType.Field:
        //                return EmptyAndTerrainMatches(cell, type, TerrainType.Grass);

        //            case StructureDefinition.StructureType.Empty:
        //                return CellEmpty(cell);

        //            default:
        //                throw new NotImplementedException();
        //        }
        //    }
        //    throw new IndexOutOfRangeException();
        //}

        public IPlacementResult CellEmpty(ICoord coord)
        {
            if (_factionManager.Value.TryGetStructureAtCoord(coord, out _))
            {
                return _notEmptyResult;
            }
            return _validResult;
        }

        public IPlacementResult EmptyAndTerrainMatches(ICoord coord, StructureDefinition.StructureType type, TerrainType required)
        {
            var empty = CellEmptyOrSame(coord, type);

            if (empty.CanPlace && _mapManager.Value.TryGetCellAtCoord(coord, out Cell cell))
            {
                if (cell.Terrain.Type == required)
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

        public IPlacementResult CellEmptyOrSame(ICoord coord, StructureDefinition.StructureType structureToPlace)
        {
            if (_factionManager.Value.TryGetStructureAtCoord(coord, out IStructure structure))
            {
                if (structure.Type == structureToPlace)
                {
                    return _validResult;
                }
                return _notEmptyResult;
            }
            return _validResult;
        }

        public override void Initialize()
        {
            _factionManager = new Lazy<IFactionManager>(() => Locate<IFactionManager>());
            _mapManager = new Lazy<IMapManager>(() => Locate<IMapManager>());
        }

        //public bool HasNeigbourContainingStructure(Cell cell, StructureDefinition.StructureType type)
        //{
        //    foreach (var neighbour in cell.GetCardinalNeighbours())
        //    {
        //        if (_factionManager.TryGetStructureAtCoord(neighbour.Coord, out IStructure structure)
        //            && structure.Type == type)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}
    }
}