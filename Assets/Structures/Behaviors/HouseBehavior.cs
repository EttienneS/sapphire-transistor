﻿using Assets.Factions;
using Assets.Map;
using System.Collections.Generic;

namespace Assets.Structures.Behaviors
{
    public class HouseBehavior : StructureBehaviorBase
    {
        private readonly Dictionary<ResourceType, int> _baseYield;

        public HouseBehavior() 
        {
            _baseYield = new Dictionary<ResourceType, int>
            {
                { ResourceType.Gold, 1 }
            };
        }

        public override Dictionary<ResourceType, int> GetYield(IStructure structure)
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