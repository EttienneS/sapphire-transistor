using Assets.ServiceLocator;
using Assets.Structures;
using System.Collections.Generic;
using System.Linq;

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

            if (faction is PlayerFaction)
            {
                _playerFaction = faction;
            }

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
        }

        public void MoveToNextTurn()
        {
            if (_activeFaction != null)
            {
                _factionQueue.Enqueue(_activeFaction);
            }
            _activeFaction = _factionQueue.Dequeue();

            OnTurnStarted?.Invoke(_activeFaction);

            _activeFaction.DoTurnStartActions();
            _activeFaction.TakeTurn();
        }

        public List<IFaction> GetFactions()
        {
            return _factionQueue.ToList();
        }

        public IFaction GetOwnerOfStructure(IStructure structure)
        {
            foreach (var faction in GetFactions())
            {
                if (faction.StructureManager.GetStructures().Contains(structure))
                {
                    return faction;
                }
            }

            throw new KeyNotFoundException($"{structure} owner not found!");
        }
    }
}