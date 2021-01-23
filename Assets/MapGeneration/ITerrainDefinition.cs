using Assets.Map;

namespace Assets.MapGeneration
{
    public interface ITerrainDefinition
    {
        ITerrain GetTerrainTypeForHeight(float cellHeight);
    }
}