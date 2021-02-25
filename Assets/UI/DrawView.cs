using Assets.Factions;
using Assets.ServiceLocator;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.UI
{
    public class DrawView : MonoBehaviour
    {
        public DeckView DeckViewPrefab;

        public void Start()
        {
            gameObject.SetActive(true);

            var playerFaction = Locator.Instance.Find<IFactionManager>().GetPlayerFaction();

            foreach (var deck in playerFaction.Decks)
            {
                var deckPrefab = Instantiate(DeckViewPrefab, transform);
                deckPrefab.Load(deck.Key, deck.Value);
            }
        }
    }
}