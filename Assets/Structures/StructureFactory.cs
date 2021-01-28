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

        private delegate IStructure MakeStructureDelegate(ICoord coord);

        public IStructureBehaviour GetBehaviour<T>() where T : IStructureBehaviour
        {
            return _behaviorLookup[typeof(T).Name];
        }

        public IStructure GetStructure(StructureType type, ICoord coord)
        {
            return _structureBuilderLookup[type].Invoke(coord);
        }

        public override void Initialize()
        {
            var map = Locate<IMapManager>();

            AddBehavior(new HouseBehavior(map));
            AddBehavior(new FarmBehavior(map));
            AddBehavior(new SettlementCore(map));
            AddBehavior(new NoBehavior(map));

            _structureBuilderLookup.Add(StructureType.Tree, (coord) => new Structure(StructureType.Tree, 1, 1, "Tree", "Pine tree", GetBehaviour<NoBehavior>(), coord));
            _structureBuilderLookup.Add(StructureType.Rock, (coord) => new Structure(StructureType.Rock, 1, 1, "Rock", "A rock", GetBehaviour<NoBehavior>(), coord));
            _structureBuilderLookup.Add(StructureType.Core, (coord) => new Structure(StructureType.Core, 2, 2, "BellTower", "The heart of this settlement", GetBehaviour<SettlementCore>(), coord));
            _structureBuilderLookup.Add(StructureType.Road, (coord) => new Structure(StructureType.Road, 1, 1, "Road", "Road", GetBehaviour<NoBehavior>(), coord));
            _structureBuilderLookup.Add(StructureType.Anchor, (coord) => new Structure(StructureType.Anchor, 1, 1, "Anchor", "Place where other structures can be placed", GetBehaviour<NoBehavior>(), coord));
            _structureBuilderLookup.Add(StructureType.Base, (coord) => new Structure(StructureType.Base, 1, 1, "Base", "Base of a card matrix that gets placed on an anchor", GetBehaviour<NoBehavior>(), coord));
            _structureBuilderLookup.Add(StructureType.House, (coord) => new Structure(StructureType.House, 1, 1, "House", "A house", GetBehaviour<HouseBehavior>(), coord));
        }

        private void AddBehavior(IStructureBehaviour behavior)
        {
            _behaviorLookup.Add(behavior.GetType().Name, behavior);
        }
    }
}