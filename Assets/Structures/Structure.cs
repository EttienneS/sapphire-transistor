using Assets.Factions;
using Assets.Map;
using System.Collections.Generic;

namespace Assets.Structures
{
    public class Structure : IStructure
    {
        public Structure(StructureType type, int width, int height, IStructureBehaviour behaviour, ICoord coord)
        {
            Type = type;
            Behaviour = behaviour;

            Width = width;
            Height = height;

            var coords = new List<ICoord>();
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Height; z++)
                {
                    coords.Add(new Coord(coord.X + x, coord.Y, coord.Z + z));
                }
            }
            OccupiedCoords = coords.ToArray();

            StructureEventManager.StructurePlanned(this);
        }

        public IStructureBehaviour Behaviour { get; }
        public StructureType Type { get; }


        public ICoord[] OccupiedCoords { get; }

        public int Width { get; }

        public int Height { get; }

        public Dictionary<ResourceType, int> GetYield(IStructure structure)
        {
            return Behaviour.GetBaseYield(this);
        }

        public void TurnEnd(IStructure structure)
        {
            Behaviour.TurnEnd(this);
        }

        public void TurnStart(IStructure structure)
        {
            Behaviour.TurnStart(this);
        }

        public override string ToString()
        {
            return $"{Type}: {GetOrigin()}";
        }

        public ICoord GetOrigin()
        {
            return OccupiedCoords[0];
        }

        public string GetStatus()
        {
            return $"Location: {GetOrigin()}";
        }
    }
}