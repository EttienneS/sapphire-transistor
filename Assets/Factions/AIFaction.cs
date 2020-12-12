using Assets.Structures;
using UnityEngine;

namespace Assets.Factions
{
    public class AIFaction : FactionBase
    {
        public AIFaction(string name, IStructureFactory structureFactory, SpawnManager spawnManager) : base(name, structureFactory, spawnManager)
        {
        }

        public override void TakeTurn()
        {
            Debug.Log($"Turn over: {Name}");

            EndTurn();
        }
    }
}