using Assets.Map;

namespace Assets.Structures
{
    public class Structure : IStructure
    {
        public Structure(string name, IStructureBehaviour behaviour, ICoord coord)
        {
            Name = name;
            Behaviour = behaviour;
            Coord = coord;
        }

        public IStructureBehaviour Behaviour { get; }
        public ICoord Coord { get; }
        public string Name { get; }
    }
}