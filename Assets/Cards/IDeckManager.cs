using Assets.Map;
using System.Collections.Generic;

namespace Assets.Cards
{
    public interface IDeckManager
    {
        public List<ICard> Hand { get; }

        void CancelCard();

        void CellClicked(Cell cell);

        void CellHover(Cell cell);

        void ClearPreview();

        void ConfirmCard();

        void DrawCard();

        void DrawToHandSize();

        void DiscardHand();

        int GetMaxHandSize();

        void OnPlayerCardActive(ICard card);

        void PreviewCard(ICard card, ICoord coord);

        bool TryGetActiveCard(out ICard card);
        void DiscardCard(ICard card);
    }
}