using Assets.Map;

namespace Assets.Structures
{
    public interface IPlacementValidator
    {
        IPlacementResult CellEmpty(ICoord coord);
        IPlacementResult EmptyAndTerrainMatches(ICoord coord, StructureDefinition.StructureType type, TerrainType required);
        IPlacementResult CellEmptyOrSame(ICoord coord, StructureDefinition.StructureType structureToPlace);
    }

    public interface IPlacementResult
    {
        bool CanPlace { get; }

        string Message { get; }
    }

    public class InvalidPlacementResult : IPlacementResult
    {
        public InvalidPlacementResult(string message)
        {
            CanPlace = false;
            Message = message;
        }

        public bool CanPlace { get; }

        public string Message { get; }
    }

    public class ValidPlacementResult : IPlacementResult
    {
        public ValidPlacementResult()
        {
            CanPlace = true;
            Message = "Valid";
        }

        public bool CanPlace { get; }

        public string Message { get; }
    }
}