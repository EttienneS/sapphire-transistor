using Assets.Cards;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Factions
{
    public class FactionManager : GameServiceBase, IFactionManager
    {
        private IFaction _activeFaction;
        private Queue<IFaction> _factionQueue;
        private IFaction _natureFaction;
        private PlayerFaction _playerFaction;
        private Lazy<ICardManager> _cardManager;

        public event FactionDelegates.OnTurnEnded OnTurnEnded;

        public event FactionDelegates.OnTurnStarted OnTurnStarted;

        public void AddFaction(IFaction faction)
        {
            _factionQueue.Enqueue(faction);

            if (faction is PlayerFaction player)
            {
                _playerFaction = player;
            }

            if (faction is NatureFaction)
            {
                _natureFaction = faction;
            }

            faction.TurnEnded += Faction_TurnEnded;
        }

        public IFaction GetActiveFaction()
        {
            return _activeFaction;
        }

        public List<IFaction> GetAllFactions()
        {
            var factions = _factionQueue.ToList();
            if (_activeFaction != null)
            {
                factions.Add(_activeFaction);
            }
            return factions;
        }

        public IFaction GetNatureFaction()
        {
            return _natureFaction;
        }

        public IFaction GetOwnerOfStructure(IStructure structure)
        {
            foreach (var faction in GetAllFactions())
            {
                if (faction.StructureManager.GetStructures().Contains(structure))
                {
                    return faction;
                }
            }

            throw new KeyNotFoundException($"{structure} owner not found!");
        }

        public PlayerFaction GetPlayerFaction()
        {
            return _playerFaction;
        }

        public override void Initialize()
        {
            _factionQueue = new Queue<IFaction>();
            _cardManager = new Lazy<ICardManager>(() => Locate<ICardManager>());
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

        public bool TryGetStructureAtCoord(ICoord coord, out IStructure structure)
        {
            foreach (var faction in GetAllFactions())
            {
                structure = faction.StructureManager.GetStructures().Find(s => s.OccupiedCoords.Any(c => c.Equals(coord)));
                if (structure != null)
                {
                    return true;
                }
            }

            structure = null;
            return false;
        }

        private void Faction_TurnEnded(IFaction faction)
        {
            faction.DoTurnEndActions();
            OnTurnEnded?.Invoke(faction);
            MoveToNextTurn();
        }
    }
}