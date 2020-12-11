using Assets.Structures;

namespace Assets.Factions
{
    public class AIFaction : FactionBase
    {
        public AIFaction(string name, IStructureFactory structureFactory) : base(name, structureFactory)
        {
        }

        public override void TakeTurn()
        {
        }
    }
}