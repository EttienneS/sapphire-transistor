using Assets.Factions;
using Assets.ServiceLocator;
using System;
using UnityEngine;

namespace Assets.UI
{
    public class DrawView : MonoBehaviour
    {
        public DeckView DeckViewPrefab;

        private Lazy<PlayerFaction> _player = new Lazy<PlayerFaction>(() => Locator.Instance.Find<IFactionManager>().GetPlayerFaction());

        public void Show()
        {
            gameObject.SetActive(true);
            ClearChildren();

            foreach (var deck in _player.Value.Decks)
            {
                var deckPrefab = Instantiate(DeckViewPrefab, transform);
                deckPrefab.Load(deck.Key, deck.Value);
            }
        }

        public void Toggle()
        {
            if (gameObject.activeInHierarchy)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        private void ClearChildren()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void Hide()
        {
            ClearChildren();
            gameObject.SetActive(false);
        }
    }
}