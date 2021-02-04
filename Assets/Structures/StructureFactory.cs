using Assets.Map;
using Assets.ServiceLocator;
using Assets.Structures.Behaviors;
using System.Collections.Generic;

namespace Assets.Structures
{
    public class StructureFactory : GameServiceBase, IStructureFactory
    {
        private readonly Dictionary<string, IStructureBehaviour> _behaviorLookup = new Dictionary<string, IStructureBehaviour>();
        private readonly Dictionary<StructureType, MakeStructureDelegate> _structureBuilderLookup = new Dictionary<StructureType, MakeStructureDelegate>();

        private delegate IStructure MakeStructureDelegate(ICoord coord, StructureType type);

        public IStructureBehaviour GetBehaviour<T>() where T : IStructureBehaviour
        {
            return _behaviorLookup[typeof(T).Name];
        }

        public IStructure GetStructure(StructureType type, ICoord coord)
        {
            return _structureBuilderLookup[type].Invoke(coord, type);
        }

        public override void Initialize()
        {
            var map = Locate<IMapManager>();

            AddBehavior(new HouseBehavior(map));
            AddBehavior(new FieldBehaviour(map));
            AddBehavior(new SettlementCore(map));
            AddBehavior(new NoBehavior(map));

            _structureBuilderLookup.Add(StructureType.Tree, (coord, type) => new Structure(type, 1, 1, GetBehaviour<NoBehavior>(), coord));
            _structureBuilderLookup.Add(StructureType.Rock, (coord, type) => new Structure(type, 1, 1, GetBehaviour<NoBehavior>(), coord));
            _structureBuilderLookup.Add(StructureType.Core, (coord, type) => new Structure(type, 2, 2, GetBehaviour<SettlementCore>(), coord));
            _structureBuilderLookup.Add(StructureType.Road, (coord, type) => new Structure(type, 1, 1, GetBehaviour<NoBehavior>(), coord));
            _structureBuilderLookup.Add(StructureType.House, (coord, type) => new Structure(type, 1, 1, GetBehaviour<HouseBehavior>(), coord));
            _structureBuilderLookup.Add(StructureType.Barn, (coord, type) => new Structure(type, 2, 2, GetBehaviour<NoBehavior>(), coord));
            _structureBuilderLookup.Add(StructureType.Field, (coord, type) => new Structure(type, 1, 1, GetBehaviour<FieldBehaviour>(), coord));
        }

        private void AddBehavior(IStructureBehaviour behavior)
        {
            _behaviorLookup.Add(behavior.GetType().Name, behavior);
        }
    }
}