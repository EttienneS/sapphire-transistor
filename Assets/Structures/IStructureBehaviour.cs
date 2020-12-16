using Assets.Resources;

namespace Assets.Structures
{
    public interface IStructureBehaviour
    {
        (ResourceType, int)[] GetBaseYield(IStructure structure);

        void TurnStart(IStructure structure);

        void TurnEnd(IStructure structure);
    }
}