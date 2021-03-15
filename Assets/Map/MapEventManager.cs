namespace Assets.Map
{
    public static class MapEventManager
    {
        public static event MapDelegates.OnChunkRenderStatusChangedDelegate OnChunkRenderDeactivated;

        public static event MapDelegates.OnChunkRenderStatusChangedDelegate OnChunkRenderActivated;

        public static event MapDelegates.OnChunkRenderStatusChangedDelegate OnChunkRenderCreated;

        public static event MapDelegates.OnCellTerrainChangedDelegate OnCellTerrainChangedDelegate;

        public static void CellTerrainChanged(Cell cell, ITerrain terrain)
        {
            OnCellTerrainChangedDelegate?.Invoke(cell, terrain);
        }

        public static void ChunkRenderCreated(ChunkRenderer renderer)
        {
            OnChunkRenderCreated?.Invoke(renderer);
        }

        public static void ChunkRendererActivated(ChunkRenderer renderer)
        {
            OnChunkRenderActivated?.Invoke(renderer);
        }

        public static void ChunkRendererDeactivated(ChunkRenderer renderer)
        {
            OnChunkRenderDeactivated?.Invoke(renderer);
        }
    }
}