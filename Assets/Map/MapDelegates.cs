namespace Assets.Map
{
    public static class MapDelegates
    {
        public delegate bool CheckCell(Cell cell);

        public delegate void OnChunkRenderStatusChangedDelegate(ChunkRenderer renderer);
    }
}