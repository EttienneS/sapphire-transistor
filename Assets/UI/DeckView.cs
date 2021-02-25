using Assets.Cards;
using Assets.Factions;
using Assets.ServiceLocator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI
{
    public class DeckView : MonoBehaviour
    {
        private IDeck _deck;

        public TMP_Text DeckInfo;
        public Image Background;

        internal void Load(CardColor color, IDeck deck)
        {
            _deck = deck;
            Background.color = color.GetActualColor();
        }

        public void OnClick()
        {
            Locator.Instance.Find<IFactionManager>()
                            .GetPlayerFaction()
                            .HandManager
                            .DrawCard(_deck.Draw());
        }

        public void Update()
        {
            DeckInfo.text = $"{_deck.GetRemaining()} ({_deck.GetDiscardPile()})";

            if (_deck.GetRemaining() <= 0)
            {
                GetComponentInChildren<Button>().interactable = false;
            }
        }
    }
}