using Assets.Factions;
using System.Collections.Generic;

namespace Assets.Structures
{
    public interface IStructureBehaviour
    {
        Dictionary<ResourceType, int> GetYield(IStructure structure);

        void TurnStart(IStructure structure);

        void TurnEnd(IStructure structure);
    }
}