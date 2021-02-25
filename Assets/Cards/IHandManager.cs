using Assets.Map;
using System.Collections.Generic;

namespace Assets.Cards
{
    public interface IHandManager
    {
        public List<ICard> Hand { get; }

        void CancelCard();

        void CellClicked(Cell cell);

        void CellHover(Cell cell);

        void ClearPreview();

        void ConfirmCard();

        void DiscardHand();

        int GetMaxHandSize();

        void OnPlayerCardActive(ICard card);

        void PreviewCard(ICard card, ICoord coord);

        bool TryGetActiveCard(out ICard card);

        void DiscardCard(ICard card);

        void DrawCard(ICard card);

        int GetOpenHandSize();
    }
}