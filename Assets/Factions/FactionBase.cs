using Assets.Cards;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.Structures;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Factions
{
    public static class FactionExtensions
    {
        public static List<ICoord> GetOpenAnchorPoints(this IFaction faction)
        {
            return faction.StructureManager.GetStructures()
                                           .Where(s => s.Type == StructureType.Anchor)
                                           .Select(s => s.GetOrigin())
                                           .ToList();
        }
    }

    public abstract class FactionBase : IFaction
    {
        private readonly Dictionary<ResourceType, int> _resources;

        internal List<ICard> _cards;

        protected FactionBase(string name, IServiceLocator serviceLocator)
        {
            _cards = new List<ICard>();
            _resources = new Dictionary<ResourceType, int>();

            Name = name;

            StructureManager = new StructureManager(serviceLocator.Find<IStructureFactory>());
        }

        public event FactionDelegates.OnResourceChanged OnResourcesUpdated;

        public event FactionDelegates.OnTurnEnded TurnEnded;

        public event FactionDelegates.OnTurnStarted TurnStarted;

        public string Name { get; }

        public IStructureManager StructureManager { get; }

        public void AddCard(ICard card)
        {
            _cards.Add(card);
            CardEventManager.CardReceived(card, this);
        }

        public bool CanAfford((ResourceType resource, int amount)[] cost)
        {
            var resources = GetResources();
            foreach (var resource in cost)
            {
                if (!resources.ContainsKey(resource.resource))
                {
                    return false;
                }
                if (resources[resource.resource] < resource.amount)
                {
                    return false;
                }
            }

            return true;
        }

        public void DoTurnEndActions()
        {
            StructureManager.DoTurnEndActions();
        }

        public void DoTurnStartActions()
        {
            StructureManager.DoTurnStartActions();
            AddResources(StructureManager.GetCombinedYield());
            TurnStarted?.Invoke(this);
        }

        public void EndTurn()
        {
            TurnEnded?.Invoke(this);
        }

        public int GetHandSize()
        {
            return 5;
        }

        public Dictionary<ResourceType, int> GetResources()
        {
            return _resources;
        }

        public void ModifyResource(ResourceType resource, int amount)
        {
            if (!_resources.ContainsKey(resource))
            {
                _resources.Add(resource, 0);
            }
            _resources[resource] += amount;

            OnResourcesUpdated?.Invoke(resource, _resources[resource]);
        }

        public void PlayCard(ICard card, ICoord anchor)
        {
            CardEventManager.CardPlayed(card, this, anchor);
            _cards.Remove(card);
        }

        public abstract void TakeTurn();

        private void AddResources(List<(ResourceType resouceType, int amount)> resouces)
        {
            foreach (var resouce in resouces)
            {
                ModifyResource(resouce.resouceType, resouce.amount);
            }
        }
    }
}