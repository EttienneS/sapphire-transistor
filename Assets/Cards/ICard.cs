using Assets.Cards.Actions;
using Assets.Factions;
using Assets.Map;
using System.Collections.Generic;

namespace Assets.Cards
{
    public interface ICard
    {
        string Name { get; }

        Dictionary<ResourceType, int> GetCost();

        ICardAction[,] GetActions();

        bool CanPlay(ICoord coord);

        void Play(ICoord coord);

        void Preview(ICoord coord);

        void ClearPreview();
    }
}