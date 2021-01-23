using Assets.Map;
using Assets.Resources;
using System.Collections.Generic;

namespace Assets.Structures
{
    public class Structure : IStructure
    {
        public Structure(IStructureFacade facade, ICoord coord)
        {
            Name = facade.Name;
            Behaviour = facade.BehaviorPrototype;

            TotalTurnsToBuild = facade.GetTotalBuildTime();
            Address = facade.Address;

            Width = facade.Width;
            Height = facade.Height;
            Description = facade.Description;

            var coords = new List<ICoord>();
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Height; z++)
                {
                    coords.Add(new Coord(coord.X + x, coord.Y, coord.Z + z));
                }
            }
            OccupiedCoords = coords.ToArray();
        }

        public IStructureBehaviour Behaviour { get; }
        public int TotalTurnsToBuild { get; set; }
        public int ElapsedTurnsToBuild { get; set; }
        public string Name { get; }

        public bool Built { get; set; }

        public string Address { get; }

        public string Description { get; }

        public ICoord[] OccupiedCoords { get; }

        public int Width { get; }

        public int Height { get; }

        public (ResourceType, int)[] GetYield(IStructure structure)
        {
            if (Built)
            {
                return Behaviour.GetBaseYield(this);
            }
            else
            {
                return new (ResourceType, int)[0];
            }
        }

        public void TurnEnd(IStructure structure)
        {
            if (Built)
            {
                Behaviour.TurnEnd(this);
            }
        }

        public void TurnStart(IStructure structure)
        {
            if (Built)
            {
                Behaviour.TurnStart(this);
            }
            else
            {
                BuildStructure();
            }
        }

        private void BuildStructure()
        {
            ElapsedTurnsToBuild++;
            if (ElapsedTurnsToBuild >= TotalTurnsToBuild)
            {
                StructureEventManager.StructureBuildCompleted(this);
                Built = true;
            }
            else
            {
                StructureEventManager.StructureBuildProgress(this, ElapsedTurnsToBuild, TotalTurnsToBuild);
            }
        }

        public override string ToString()
        {
            return $"{Name}: {GetOrigin()}";
        }

        public ICoord GetOrigin()
        {
            return OccupiedCoords[0];
        }

        public string GetStatus()
        {
            return $"Location: {GetOrigin()}\n{Description}\nBuilt: {ElapsedTurnsToBuild}/{TotalTurnsToBuild}";
        }
    }
}