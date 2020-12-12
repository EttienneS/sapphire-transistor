using Assets.Structures;

namespace Assets.Factions
{
    public class PlayerFaction : FactionBase
    {
        public PlayerFaction(string name, IStructureFactory structureFactory, SpawnManager spawnManager) : base(name, structureFactory, spawnManager)
        {
        }

        public override void TakeTurn()
        {
        }
    }
}