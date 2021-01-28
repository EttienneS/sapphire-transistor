﻿using Assets.Cards;
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

            StructureManager = new StructureManager(serviceLocator.Find<IStructureFactory>());
        }

        public event FactionDelegates.OnResourceChanged OnResourcesUpdated;

        public event FactionDelegates.OnTurnEnded TurnEnded;

        public event FactionDelegates.OnTurnStarted TurnStarted;

        public IDeck Deck { get; }
        public List<ICard> Hand { get; }
        public string Name { get; }

        public IStructureManager StructureManager { get; }

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

        public void PlayCard(ICard card, ICoord anchor)
        {
            CardEventManager.CardPlayed(card, this, anchor);
            Hand.Remove(card);
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