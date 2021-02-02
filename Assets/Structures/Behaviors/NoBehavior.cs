using Assets.Factions;
using Assets.Map;
using System.Collections.Generic;

namespace Assets.Structures.Behaviors
{
    public class NoBehavior : StructureBehaviorBase
    {
        public NoBehavior(IMapManager map) : base(map)
        {
        }

        public override Dictionary<ResourceType, int> GetBaseYield(IStructure structure)
        {
            return new Dictionary<ResourceType, int>();
        }

        public override void TurnEnd(IStructure structure)
        {
        }

        public override void TurnStart(IStructure structure)
        {
        }
    }
}