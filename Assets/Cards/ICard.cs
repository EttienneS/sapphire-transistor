using Assets.Cards.Actions;
using Assets.Factions;
using Assets.Map;
using System.Collections.Generic;

namespace Assets.Cards
{
    public interface ICard
    {
        CardColor Color { get; }
        string Name { get; }

        bool CanPlay(ICoord coord);

        void ClearPreview();

        ICardAction[,] GetActions();

        Dictionary<ResourceType, int> GetCost();

        void Play(ICoord coord);

        void Preview(ICoord coord);
    }
}