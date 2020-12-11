using Assets.ServiceLocator;
using Assets.Structures;
using System;
using System.Collections.Generic;

namespace Assets.Factions
{
    public class FactionManager : GameServiceBase, IFactionManager
    {
        private Queue<IFaction> _factionQueue;

        private IFaction _activeFaction;

        private IFaction _playerFaction;

        public void AddFaction(IFaction faction)
        {
            _factionQueue.Enqueue(faction);
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
            _playerFaction = new PlayerFaction("Player", structureFactory);

            AddFaction(_playerFaction);
            AddFaction(new AIFaction("Enemy", structureFactory));

            _activeFaction = _factionQueue.Dequeue();
        }

        public void MoveToNextTurn()
        {
            _factionQueue.Enqueue(_activeFaction);
            _activeFaction = _factionQueue.Dequeue();
        }
    }
}