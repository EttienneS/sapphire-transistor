using Assets.Cards;
using TMPro;
using UnityEngine;

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