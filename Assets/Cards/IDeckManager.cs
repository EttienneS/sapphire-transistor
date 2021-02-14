using Assets.Map;
using System.Collections.Generic;

namespace Assets.Cards
{
    public interface IDeckManager
    {
        public ICardLoader CardLoader { get; }
        public IDeck Deck { get; }
        public List<ICard> Hand { get; }

        void CancelCard();

        void CellClicked(Cell cell);

        void ClearPreview();

        void ConfirmCard();

        void DrawCard(ICard card);

        void DrawToHandSize();

        void DiscardHand();

        int GetMaxHandSize();

        void OnPlayerCardActive(ICard card);

        void PreviewCard(ICard card, ICoord coord);

        bool TryGetActiveCard(out ICard card);
        void DiscardCard(ICard card);
    }
}