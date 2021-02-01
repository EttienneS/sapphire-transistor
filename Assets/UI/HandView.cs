﻿using Assets.Cards;
using Assets.Factions;
using Assets.Map;
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
            _playerFaction.Value.CancelCard();
        }

        public void ConfirmCard()
        {
            _playerFaction.Value.ConfirmCard();
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

            CardEventManager.OnCardPlayed += (ICard card, ICoord _) =>
            {
                if (_handLookup.ContainsKey(card))
                {
                    OnCardPlayed(card);
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

        private void OnCardPlayed(ICard card)
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