using Assets.Map;
using Assets.ServiceLocator;
using Assets.Structures.Behaviors;
using System.Collections.Generic;

namespace Assets.Structures
{
    public class StructureFactory : GameServiceBase, IStructureFactory
    {
        private readonly Dictionary<string, IStructureBehaviour> _behaviorLookup = new Dictionary<string, IStructureBehaviour>();

        public IStructureBehaviour GetBehaviour(string name)
        {
            return _behaviorLookup[name];
        }

        public IStructure GetStructure(StructureDefinition definition, ICoord coord)
        {
            return new Structure(definition.Type, definition.Width, definition.Height, GetBehaviour(definition.Behaviour), coord, definition.RequiresLink);
        }

        public override void Initialize()
        {
            var map = Locate<IMapManager>();

            AddBehavior(new HouseBehavior());
            AddBehavior(new FarmBehaviour());
            AddBehavior(new SettlementCore());
            AddBehavior(new NoBehavior());
            AddBehavior(new CabinBehaviour(map));
        }

        private void AddBehavior(IStructureBehaviour behavior)
        {
            _behaviorLookup.Add(behavior.GetType().Name, behavior);
        }
    }
}