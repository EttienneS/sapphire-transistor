using Assets.Map;

namespace Assets.Cards.Actions
{
    public interface ICardAction
    {
        bool CanPlay(ICoord coord);

        void ClearPreview();

        void Play(ICoord coord);

        void Preview(ICoord coord);
    }
}