using Assets.Structures;

namespace Assets.Factions
{
    public class PlayerFaction : FactionBase
    {
        public PlayerFaction(string name, IStructureFactory structureFactory) : base(name, structureFactory)
        {
        }

        public override void TakeTurn()
        {
        }

        
    }
}