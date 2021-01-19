using Assets.Map;
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
            return _behaviorLookup[typeof(T).Name];
        }

        public override void Initialize()
        {
            var map = Locate<IMapManager>();

            AddBehavior(new HouseBehavior(map));
            AddBehavior(new FarmBehavior(map));
            AddBehavior(new SettlementCore(map));
            AddBehavior(new NoBehavior(map));
        }

        public IStructure MakeStructure(IStructureFacade facade, ICoord coord)
        {
            var structure = new Structure(facade, coord);

            StructureEventManager.StructurePlanned(structure);

            return structure;
        }

        private void AddBehavior(IStructureBehaviour behavior)
        {
            _behaviorLookup.Add(behavior.GetType().Name, behavior);
        }
    }
}