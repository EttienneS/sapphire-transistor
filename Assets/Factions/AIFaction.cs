using Assets.Structures;

namespace Assets.Factions
{
    public class AIFaction : FactionBase
    {
        public AIFaction(string name, IStructureFactory structureFactory, SpawnManager spawnManager) : base(name, structureFactory, spawnManager)
        {
        }

        public override void TakeTurn()
        {
        }
    }
}