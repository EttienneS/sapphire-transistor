using Assets.Cards;
using Assets.Factions;
using Assets.ServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.UI
{
    public class HandView : MonoBehaviour
    {
        public GameObject CardContainer;
        public CardView CardViewPrefab;
        private readonly Dictionary<ICard, CardView> _handLookup = new Dictionary<ICard, CardView>();
        private readonly Lazy<PlayerFaction> _playerFaction = new Lazy<PlayerFaction>(() => Locator.Instance.Find<IFactionManager>().GetPlayerFaction());

        public void CancelCard()
        {
            _playerFaction.Value.HandManager.CancelCard();
        }

        public void ConfirmCard()
        {
            _playerFaction.Value.HandManager.ConfirmCard();
        }

        public void Start()
        {
            CardEventManager.OnCardReceived += (ICard card, IFaction faction) =>
            {
                if (faction is PlayerFaction)
                {
                    OnCardReceived(card);
                }
            };

            CardEventManager.OnCardDiscarded += (ICard card) =>
            {
                if (_handLookup.ContainsKey(card))
                {
                    OnCardDiscarded(card);
                }
            };
        }

        public void Update()
        {
            foreach (var card in _handLookup.Keys.ToList())
            {
                if (_handLookup[card] == null)
                {
                    var cardView = Instantiate(CardViewPrefab, CardContainer.transform);
                    cardView.Load(card);
                    _handLookup[card] = cardView;
                }
            }
        }

        private void OnCardDiscarded(ICard card)
        {
            Locator.Instance.Find<ISpawnManager>().AddItemToDestroy(_handLookup[card].gameObject);
            _handLookup.Remove(card);
        }

        private void OnCardReceived(ICard card)
        {
            _handLookup.Add(card, null);
        }
    }
}