using Assets.Map;
using Assets.Resources;
using Assets.ServiceLocator;
using Assets.Structures.Behaviors;
using System.Collections.Generic;

namespace Assets.Structures
{
    public class StructureFactory : GameServiceBase, IStructureFactory
    {
        private readonly Dictionary<string, IStructureBehaviour> _behaviorLookup = new Dictionary<string, IStructureBehaviour>();

        public IStructureBehaviour GetBehaviour<T>() where T : IStructureBehaviour
        {
            return _behaviorLookup[nameof(T)];
        }

        public override void Initialize()
        {
            var map = Locate<MapManager>();
            var resource = Locate<ResourceManager>();

            AddBehavior(new FarmBehavior(map, resource));
        }

        public IStructure MakeStructure<T>(string name, ICoord coord) where T : IStructureBehaviour
        {
            return new Structure(name, GetBehaviour<T>(), coord);
        }

        private void AddBehavior(IStructureBehaviour behavior)
        {
            _behaviorLookup.Add(nameof(behavior), behavior);
        }
    }
}