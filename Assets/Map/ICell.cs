using System.Collections.Generic;

namespace Assets.Map
{
    public interface ICell
    {
        ITerrain GetTerrain();

        List<ICell> GetNonNullNeighbors();
    }
}