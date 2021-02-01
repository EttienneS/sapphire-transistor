using Assets.Map;

namespace Assets.Cards
{
    public interface ICard
    {
        string Name { get; }

        ICardAction[,] GetActions();

        bool CanPlay(ICoord coord);

        void Play(ICoord coord);

        void Preview(ICoord coord);

        void ClearPreview();
    }
}