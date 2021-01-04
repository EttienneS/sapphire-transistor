﻿using Assets.Resources;

namespace Assets.Structures
{
    public interface IStructureFacade
    {
        string Address { get; }
        (ResourceType, int)[] Cost { get; }
        string Description { get; }
        string Name { get; }
        IStructureBehaviour BehaviorPrototype { get; }
    }
}