﻿using Assets.Map;
using Assets.Resources;

namespace Assets.Structures.Behaviors
{
    public class NoBehavior : StructureBehaviorBase
    {
        public NoBehavior(IMapManager map) : base(map)
        {
        }

        public override (ResourceType, int)[] GetBaseYield(IStructure structure)
        {
            return new (ResourceType, int)[0];
        }

        public override void TurnEnd(IStructure structure)
        {
        }

        public override void TurnStart(IStructure structure)
        {
        }
    }
}