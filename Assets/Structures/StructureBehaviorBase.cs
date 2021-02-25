using Assets.Factions;
using Assets.Map;
using System.Collections.Generic;

namespace Assets.Structures
{
    public abstract class StructureBehaviorBase : IStructureBehaviour
    {
        private readonly IMapManager _map;

        public StructureBehaviorBase(IMapManager map)
        {
            _map = map;
        }

        public abstract Dictionary<ResourceType, int> GetBaseYield(IStructure structure);

        public abstract void TurnEnd(IStructure structure);

        public abstract void TurnStart(IStructure structure);
    }
}