using Assets.Map;
using Assets.Resources;

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
            Coord = coord;
        }

        public IStructureBehaviour Behaviour { get; }
        public int TotalTurnsToBuild { get; set; }
        public int ElapsedTurnsToBuild { get; set; }
        public ICoord Coord { get; }
        public string Name { get; }

        public bool Built { get; set; }

        public string Address { get; }

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
            return $"{Name}: {Coord}";
        }
    }
}