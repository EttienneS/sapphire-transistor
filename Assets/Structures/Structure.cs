using Assets.Factions;
using Assets.Map;
using System.Collections.Generic;

namespace Assets.Structures
{
    public class Structure : IStructure
    {
        public Structure(StructureDefinition.StructureType type, IStructureBehaviour behaviour, Coord coord, bool requiresLink = true)
        {
            Type = type;
            Behaviour = behaviour;

            RequiresLink = requiresLink;

         
            Coord = coord;

            StructureEventManager.StructurePlanned(this);
        }

        public IStructureBehaviour Behaviour { get; }

        public StructureDefinition.StructureType Type { get; }

        public bool RequiresLink { get; }

        public Coord Coord { get; }

        public string GetStatus()
        {
            return $"Location: {Coord}";
        }

        public Dictionary<ResourceType, int> GetYield(IStructure structure)
        {
            return Behaviour.GetYield(this);
        }

        public override string ToString()
        {
            return $"{Type}: {Coord}";
        }

        public void TurnEnd(IStructure structure)
        {
            Behaviour.TurnEnd(this);
        }

        public void TurnStart(IStructure structure)
        {
            Behaviour.TurnStart(this);
        }
    }
}