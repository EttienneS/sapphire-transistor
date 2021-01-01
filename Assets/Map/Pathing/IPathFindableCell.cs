namespace Assets.Map.Pathing
{
    public interface IPathFindableCell
    {
        ICoord Coord { get; }
        IPathFindableCell NextWithSamePriority { get; set; }
        IPathFindableCell PathFrom { get; set; }
        float SearchDistance { get; set; }
        int SearchHeuristic { get; set; }
        int SearchPhase { get; set; }

        int SearchPriority { get; }

        int DistanceTo(IPathFindableCell other);

        IPathFindableCell GetNeighbor(Direction direction);

        float GetTravelCost();
    }
}