using Assets.Map;

namespace Assets.Cards.Actions
{
    public interface ICardAction
    {
        bool CanPlay(Coord coord);

        void ClearPreview();

        void Play(Coord coord);

        void Preview(Coord coord);
    }
}