using Assets.Cards;
using Assets.Factions;
using Assets.Map;
using Assets.ServiceLocator;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.UI
{
    public class HandView : MonoBehaviour
    {
        public CardView CardViewPrefab;
        public GameObject CardContainer;

        private Dictionary<ICard, CardView> _handLookup = new Dictionary<ICard, CardView>();

        public void Start()
        {
            CardEventManager.OnCardReceived += (ICard card, IFaction faction) =>
            {
                if (faction is PlayerFaction)
                {
                    OnCardReceived(card);
                }
            };

            CardEventManager.OnCardPlayed += (ICard card, IFaction faction, ICoord _) =>
            {
                if (faction is PlayerFaction)
                {
                    OnCardPlayed(card);
                }
            };
        }

        public void RotateActiveCW()
        {
            CardEventManager.RotateCardsCCW();
        }

        public void RotateActiveCCW()
        {
            CardEventManager.RotateCardsCW();
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