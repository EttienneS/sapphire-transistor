using Assets.Factions;
using System.Collections.Generic;

namespace Assets.Structures
{
    public interface IStructureBehaviour
    {
        Dictionary<ResourceType, int> GetBaseYield(IStructure structure);

        void TurnStart(IStructure structure);

        void TurnEnd(IStructure structure);
    }
}