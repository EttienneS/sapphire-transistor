using Assets.Map;
using Assets.Resources;
using System.Collections.Generic;

namespace Assets.Structures
{
    public interface IStructure
    {
        IStructureBehaviour Behaviour { get; }

        ICoord[] OccupiedCoords { get; }

        string Name { get; }
        string AssetAddress { get; }
        string Description { get; }

        int Width { get; }
        int Height { get; }

        (ResourceType, int)[] GetYield(IStructure structure);

        void TurnEnd(IStructure structure);

        void TurnStart(IStructure structure);
        ICoord GetOrigin();
        string GetStatus();
    }
}