using Assets.Factions;
using Assets.Helpers;
using Assets.Map;
using Assets.ServiceLocator;

namespace Assets.Structures
{
    public class StructurePlacementValidator : GameServiceBase, IStructurePlacementValidator
    {
        private IFactionManager _factionManager;

        private readonly InvalidPlacementResult _noRoadResult = new InvalidPlacementResult("Cell does not have a neighbouring road!");
        private readonly InvalidPlacementResult _notEmptyResult = new InvalidPlacementResult("Cell is not empty!");
        private readonly ValidPlacementResult _validResult = new ValidPlacementResult();

        public IStructurePlacementResult CanPlaceDefault(Cell cell)
        {
            if (CellEmpty(cell))
            {
                if (HasNeighbouringRoad(cell))
                {
                    return _validResult;
                }
                else
                {
                    return _noRoadResult;
                }
            }
            return _notEmptyResult;
        }

        public IStructurePlacementResult CanPlaceFarm(Cell cell)
        {
            if (CanPlaceDefault(cell).CanPlace)
            {
                if (cell.Terrain.Name == "Grass")
                {
                    return _validResult;
                }
            }
            return GetInvalidTerrainResult(cell.Terrain.Name, "Grass");
        }

        private InvalidPlacementResult GetInvalidTerrainResult(string current, string required)
        {
            return new InvalidPlacementResult($"Incorrect terrain: {current} != '{required}'");
        }

        public IStructurePlacementResult CanPlaceRoad(Cell cell)
        {
            if (CellEmpty(cell))
            {
                return _validResult;
            }

            return new InvalidPlacementResult("Cell is not empty!");
        }

        public override void Initialize()
        {
            _factionManager = Locate<IFactionManager>();
        }

        private bool CellEmpty(Cell cell)
        {
            if (_factionManager.TryGetStructureInCell(cell, out IStructure structure))
            {
                return false;
            }
            return true;
        }

        private bool HasNeighbouringRoad(Cell cell)
        {
            foreach (var neighbour in cell.GetCardinalNeighbours())
            {
                if (_factionManager.TryGetStructureInCell(neighbour, out IStructure structure))
                {
                    if (structure.Name == "Road")
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}