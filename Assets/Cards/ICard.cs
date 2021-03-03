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

        bool CanPlay(Coord coord);

        void ClearPreview();

        ICardAction[,] GetActions();

        Dictionary<ResourceType, int> GetCost();

        void Play(Coord coord);

        void Preview(Coord coord);

        void RotateCW();
        void RotateCCW();

    }
}