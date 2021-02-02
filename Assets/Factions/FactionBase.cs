using Assets.Cards;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.Structures;
using System.Collections.Generic;

namespace Assets.Factions
{
    public abstract class FactionBase : IFaction
    {
        private readonly Dictionary<ResourceType, int> _resources;

        protected FactionBase(string name, IServiceLocator serviceLocator)
        {
            _resources = new Dictionary<ResourceType, int>();
            Deck = new Deck();
            Name = name;
            Hand = new List<ICard>();

            CardLoader = new CardLoader(this);
            StructureManager = new StructureManager(serviceLocator.Find<IStructureFactory>(),
                                                    serviceLocator.Find<IFactionManager>(),
                                                    serviceLocator.Find<IMapManager>());
        }

        public event FactionDelegates.OnResourceChanged OnResourcesUpdated;

        public event FactionDelegates.OnTurnEnded TurnEnded;

        public event FactionDelegates.OnTurnStarted TurnStarted;

        public ICardLoader CardLoader { get; }
        public IDeck Deck { get; }
        public List<ICard> Hand { get; }
        public string Name { get; }

        public IStructureManager StructureManager { get; }

        public bool CanAfford(Dictionary<ResourceType, int> cost)
        {
            var resources = GetResources();
            foreach (var resource in cost)
            {
                if (!resources.ContainsKey(resource.Key))
                {
                    return false;
                }
                if (resources[resource.Key] < resource.Value)
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

        public void Draw()
        {
            var cardsToDraw = GetMaxHandSize() - Hand.Count;

            if (cardsToDraw > 0)
            {
                for (int i = 0; i < cardsToDraw; i++)
                {
                    DrawCard(Deck.Draw());
                }
            }
        }

        public void DrawCard(ICard card)
        {
            Hand.Add(card);
            CardEventManager.CardReceived(card, this);
        }

        public void EndTurn()
        {
            TurnEnded?.Invoke(this);
        }

        public int GetMaxHandSize()
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

        public abstract void TakeTurn();

        internal void AddResources(Dictionary<ResourceType, int> resouces)
        {
            foreach (var resouce in resouces)
            {
                ModifyResource(resouce.Key, resouce.Value);
            }
        }

        internal void RemoveResources(Dictionary<ResourceType, int> resouces)
        {
            foreach (var resouce in resouces)
            {
                ModifyResource(resouce.Key, -resouce.Value);
            }
        }
    }
}