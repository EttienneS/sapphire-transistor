using Assets.Resources;

namespace Assets.Structures
{
    public interface IStructureBehaviour
    {
        (IResource, int)[] GetYield(IStructure structure);

        void TurnStart(IStructure structure);

        void TurnEnd(IStructure structure);
    }
}