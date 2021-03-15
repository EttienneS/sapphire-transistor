using Assets.Map;

namespace Assets.MapGeneration
{
    public interface ITerrainDefinition
    {
        ITerrain GetTerrainTypeForHeight(float cellHeight);
        ITerrain GetTerrainTypeByType(TerrainType type);
        float GetHeightForType(TerrainType type);
    }
}