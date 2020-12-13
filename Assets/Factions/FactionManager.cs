using Assets.ServiceLocator;
using Assets.Structures;
using System.Collections.Generic;

namespace Assets.Factions
{
    public class FactionManager : GameServiceBase, IFactionManager
    {
        private Queue<IFaction> _factionQueue;

        private IFaction _activeFaction;

        private IFaction _playerFaction;

        public event FactionDelegates.OnTurnEnded OnTurnEnded;
        public event FactionDelegates.OnTurnStarted OnTurnStarted;

        public void AddFaction(IFaction faction)
        {
            _factionQueue.Enqueue(faction);

            faction.TurnEnded += Faction_TurnEnded;
        }

        private void Faction_TurnEnded(IFaction faction)
        {
            faction.DoTurnEndActions();
            OnTurnEnded?.Invoke(faction);
            MoveToNextTurn();
        }

        public IFaction GetPlayerFaction()
        {
            return _playerFaction;
        }

        public IFaction GetActiveFaction()
        {
            return _activeFaction;
        }

        public override void Initialize()
        {
            _factionQueue = new Queue<IFaction>();

            var structureFactory = Locate<StructureFactory>();
            var spawnManager = Locate<SpawnManager>();

            _playerFaction = new PlayerFaction("Player", structureFactory, spawnManager);
            AddFaction(_playerFaction);
            AddFaction(new AIFaction("Enemy", structureFactory, spawnManager));

            _activeFaction = _factionQueue.Dequeue();
        }

        public void MoveToNextTurn()
        {
            _factionQueue.Enqueue(_activeFaction);
            _activeFaction = _factionQueue.Dequeue();

            OnTurnStarted?.Invoke(_activeFaction);

            _activeFaction.DoTurnStartActions();
            _activeFaction.TakeTurn();
        }
    }
}