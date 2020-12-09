using Assets.Resources;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Map
{
    public interface ITerrain
    {
        Color Color { get; }

        string Name { get; }

        Dictionary<IResource, int> ResourceValue { get; }
    }
}