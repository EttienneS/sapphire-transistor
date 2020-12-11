using Assets.Factions;
using Assets.Map;
using System.Collections.Generic;

namespace Assets.Structures
{
    public interface IStructureFactory
    {
        IStructureBehaviour GetBehaviour<T>() where T : IStructureBehaviour;

        IStructure MakeStructure<T>(string name, ICoord coord) where T : IStructureBehaviour;

    }
}