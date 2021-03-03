using Assets.Map;

namespace Assets.Structures
{
    public class StructureDefinition
    {
        public StructureDefinition(string name, string asset, int poolSize, StructureType type, string behaviour, bool requiresLink, CanPlaceDelegate canPlace)
        {
            Name = name;
            Asset = asset;
            PoolSize = poolSize;
            Type = type;
            Behaviour = behaviour;
            RequiresLink = requiresLink;
            CanPlace = canPlace;
        }

        public delegate IPlacementResult CanPlaceDelegate(Coord coord);

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
        public CanPlaceDelegate CanPlace { get; }
    }
}