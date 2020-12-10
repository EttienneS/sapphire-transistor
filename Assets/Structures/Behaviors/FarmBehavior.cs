using Assets.Map;
using Assets.Resources;

namespace Assets.Structures.Behaviors
{
    public class FarmBehavior : StructureBehaviorBase
    {
        private readonly (IResource, int)[] _baseYield;

        public FarmBehavior(MapManager map, IResourceManager resourceManager) : base(map, resourceManager)
        {
            _baseYield = new[]
            {
                (resourceManager.GetResouceByName("Food"), 1)
            };
        }

        public override (IResource, int)[] GetYield(IStructure structure)
        {
            return _baseYield;
        }

        public override void TurnEnd(IStructure structure)
        {
        }

        public override void TurnStart(IStructure structure)
        {
        }
    }
}