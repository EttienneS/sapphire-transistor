using Terrain = Assets.Map.Terrain;

namespace Assets.MapGeneration
{
    public interface ITerrainDefinition
    {
        Terrain GetTerrainTypeForHeight(float cellHeight);
    }
}