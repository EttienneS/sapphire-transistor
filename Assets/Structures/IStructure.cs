using Assets.Map;
using Assets.Resources;

namespace Assets.Structures
{
    public interface IStructure
    {
        IStructureBehaviour Behaviour { get; }
        ICoord Coord { get; }
        string Name { get; }
        string Address { get; }

        (ResourceType, int)[] GetYield(IStructure structure);

        void TurnEnd(IStructure structure);

        void TurnStart(IStructure structure);
    }
}