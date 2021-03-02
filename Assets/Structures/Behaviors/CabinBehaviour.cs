using Assets.Factions;
using Assets.Map;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Structures.Behaviors
{
    public class CabinBehaviour : StructureBehaviorBase
    {
        private readonly IMapManager _map;
        private readonly Dictionary<ICoord, IStructure> _trees;

        public CabinBehaviour(IMapManager map)
        {
            _map = map;
            _trees = new Dictionary<ICoord, IStructure>();

            StructureEventManager.OnStructurePlanned += StructurePlanned;
            StructureEventManager.OnStructureDestroyed += StructureDestroyed;
        }

        private void StructureDestroyed(IStructure structure)
        {
            var origin = structure.GetOrigin();
            if (_trees.ContainsKey(origin))
            {
                _trees.Remove(origin);
            }
        }

        private void StructurePlanned(IStructure structure)
        {
            if (structure.Type == StructureDefinition.StructureType.Tree)
            {
                _trees.Add(structure.GetOrigin(), structure);
            }
        }

        public override Dictionary<ResourceType, int> GetYield(IStructure structure)
        {
            var effectRadius = _map.GetCircle(structure.GetOrigin(), 5);
            var treesInArea = effectRadius.Select(c => c.Coord).Intersect(_trees.Keys.ToList());
            return new Dictionary<ResourceType, int>
            {
                { ResourceType.Wood, treesInArea.Count() }
            };
        }

        public override void TurnEnd(IStructure structure)
        {
        }

        public override void TurnStart(IStructure structure)
        {
        }
    }
}