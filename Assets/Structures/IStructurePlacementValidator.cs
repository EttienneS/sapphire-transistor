using Assets.Map;

namespace Assets.Structures
{
    public interface IStructurePlacementValidator
    {
        IStructurePlacementResult CanPlaceRoad(Cell cell);
        IStructurePlacementResult CanPlaceFarm(Cell cell);
        IStructurePlacementResult CanPlaceDefault(Cell cell);
    }

    public interface IStructurePlacementResult
    {
        bool CanPlace { get; }

        string Message { get; }
    }

    public class InvalidPlacementResult : IStructurePlacementResult
    {
        public InvalidPlacementResult(string message)
        {
            CanPlace = false;
            Message = message;
        }

        public bool CanPlace { get; }

        public string Message { get; }
    }

    public class ValidPlacementResult : IStructurePlacementResult
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