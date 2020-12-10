using System.Collections.Generic;

namespace Assets.Map
{
    public interface ICell
    {
        List<ICell> NonNullNeighbors { get; }
        ITerrain Terrain { get; }
    }
}