using Assets.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI
{
    public class CardView : MonoBehaviour
    {
        public TMP_Text Title;
        public TMP_Text Cost;
        public TMP_Text Content;

        private ICard _card;

        public void Load(ICard card)
        {
            _card = card;

            Title.text = _card.Name;

            CardEventManager.OnSetPlayerCardActive += CardActive;
        }

        public void OnDestroy()
        {
            CardEventManager.OnSetPlayerCardActive -= CardActive;
        }

        private void CardActive(ICard card)
        {
            var img = GetComponentInChildren<Image>();
            if (card == _card)
            {
                img.color = Color.red;
            }
            else
            {
                img.color = Color.white;
            }
        }

        public void Update()
        {
            // content changes if card is rotated
            Content.text = _card.ToString();

            Cost.text = "";
            foreach (var costItem in _card.GetCost())
            {
                Cost.text += $"{costItem.Value}{costItem.Key.ToString()[0]} ";
            }
            Cost.text = Cost.text.Trim();
        }

        public void Clicked()
        {
            CardEventManager.SetPlayerCardActive(_card);
        }
    }
}