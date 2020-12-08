using Assets.ServiceLocator;
using System.Collections.Generic;

namespace Assets.Factions
{
    public class FactionManager : GameServiceBase, IFactionManager
    {
        private Queue<IFaction> _factionQueue;

        private IFaction _activeFaction;

        public void AddFaction(IFaction faction)
        {
            _factionQueue.Enqueue(faction);
        }

        public IFaction GetActiveFaction()
        {
            return _activeFaction;
        }

        public override void Initialize()
        {
            _factionQueue = new Queue<IFaction>();

            AddFaction(new Faction("Player"));
            AddFaction(new Faction("World"));

            _activeFaction = _factionQueue.Dequeue();
        }

        public void MoveToNextTurn()
        {
            _factionQueue.Enqueue(_activeFaction);
            _activeFaction = _factionQueue.Dequeue();
        }
    }
}