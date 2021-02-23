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
        public Image Background;
        public UnityEngine.UI.Outline Outline;

        private ICard _card;

        public void Load(ICard card)
        {
            _card = card;

            Title.text = _card.Name;
            Background.color = _card.Color.GetActualColor();
            CardEventManager.OnSetPlayerCardActive += CardActive;
        }

        public void OnDestroy()
        {
            CardEventManager.OnSetPlayerCardActive -= CardActive;
        }

        private void CardActive(ICard card)
        {
            if (card == _card)
            {
                Outline.effectColor = Color.red;
            }
            else
            {
                Outline.effectColor = new Color(0,0,0,0);
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