using Assets.Map;

namespace Assets.Structures
{
    public class StructureDefinition
    {
        public StructureDefinition(string name, string asset, int poolSize, StructureType type, string behaviour, bool requiresLink, int width, int height, CanPlaceDelegate canPlace)
        {
            Name = name;
            Asset = asset;
            PoolSize = poolSize;
            Type = type;
            Behaviour = behaviour;
            RequiresLink = requiresLink;
            Width = width;
            Height = height;
            CanPlace = canPlace;
        }

        public delegate IPlacementResult CanPlaceDelegate(ICoord coord);

        public enum StructureType
        {
            Tree, Rock, Core, Road, House, Barn, Field, Empty, Cabin
        }

        public string Name { get; }
        public string Asset { get; }
        public int PoolSize { get; }
        public StructureType Type { get; }
        public string Behaviour { get; }
        public bool RequiresLink { get; }
        public int Width { get; }
        public int Height { get; }
        public CanPlaceDelegate CanPlace { get; }

        
    }


}