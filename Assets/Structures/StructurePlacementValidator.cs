using Assets.Factions;
using Assets.Helpers;
using Assets.Map;
using Assets.ServiceLocator;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Structures
{
    public class StructurePlacementValidator : GameServiceBase, IStructurePlacementValidator
    {
        private IFactionManager _factionManager;
        private IMapManager _mapManager;

        private readonly InvalidPlacementResult _noRoadResult = new InvalidPlacementResult("Cell does not have a neighbouring road!");
        private readonly InvalidPlacementResult _notEmptyResult = new InvalidPlacementResult("Cell is not empty!");
        private readonly ValidPlacementResult _validResult = new ValidPlacementResult();

        public IStructurePlacementResult CanPlaceDefault(Cell origin, int width, int height)
        {
            var oneCellHasroad = false;
            foreach (var cell in GetCellsForPlacementCoords(origin, width, height))
            {
                if (CellEmpty(cell))
                {
                    if (HasNeighbouringRoad(cell))
                    {
                        oneCellHasroad = true;
                    }
                }
                else
                {
                    return _notEmptyResult;
                }
            }
            if (oneCellHasroad)
            {
                return _validResult;
            }
            return _noRoadResult;
        }

        private List<Cell> GetCellsForPlacementCoords(Cell origin, int width, int height)
        {
            var cells = new List<Cell>();

            foreach (var coord in StructureExtensions.GetPlacementCoords(origin.Coord, width, height))
            {
                cells.Add(_mapManager.GetCellAtCoord(coord));
            }

            return cells;
        }

        public IStructurePlacementResult CanPlaceFarm(Cell origin, int width, int height)
        {
            var defaultPlacement = CanPlaceDefault(origin, width, height);
            if (defaultPlacement.CanPlace)
            {
                if (!GetCellsForPlacementCoords(origin, width, height).All(c => c.Terrain.Name == "Grass"))
                {
                    return GetInvalidTerrainResult(origin.Terrain.Name, "Grass");
                }
                return _validResult;
            }
            return defaultPlacement;
        }

        private InvalidPlacementResult GetInvalidTerrainResult(string current, string required)
        {
            return new InvalidPlacementResult($"Incorrect terrain: {current} != '{required}'");
        }

        public IStructurePlacementResult CanPlaceRoad(Cell origin, int width, int height)
        {
            foreach (var cell in GetCellsForPlacementCoords(origin, width, height))
            {
                if (!CellEmpty(cell))
                {
                    return _notEmptyResult;
                }
            }

            return _validResult;
        }

        public override void Initialize()
        {
            _factionManager = Locate<IFactionManager>();
            _mapManager = Locate<IMapManager>();
        }

        private bool CellEmpty(Cell cell)
        {
            if (_factionManager.TryGetStructureInCell(cell, out _))
            {
                return false;
            }
            return true;
        }

        private bool HasNeighbouringRoad(Cell cell)
        {
            foreach (var neighbour in cell.GetCardinalNeighbours())
            {
                if (_factionManager.TryGetStructureInCell(neighbour, out IStructure structure)
                    && structure.Name == "Road")
                {
                    return true;
                }
            }

            return false;
        }
    }
}