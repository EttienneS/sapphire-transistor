using Assets.Factions;
using Assets.Map;
using System.Collections.Generic;

namespace Assets.Structures
{
    public abstract class StructureBehaviorBase : IStructureBehaviour
    {
        public abstract Dictionary<ResourceType, int> GetYield(IStructure structure);

        public abstract void TurnEnd(IStructure structure);

        public abstract void TurnStart(IStructure structure);
    }
}