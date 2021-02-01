using Assets.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI
{
    public class CardView : MonoBehaviour
    {
        public TMP_Text Title;
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
        }

        public void Clicked()
        {
            CardEventManager.SetPlayerCardActive(_card);
        }
    }
}