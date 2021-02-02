using Assets.Cards;
using System.Collections.Generic;

namespace Assets.Factions
{
    public interface IFaction
    {
        event FactionDelegates.OnResourceChanged OnResourcesUpdated;

        event FactionDelegates.OnTurnEnded TurnEnded;

        event FactionDelegates.OnTurnStarted TurnStarted;

        string Name { get; }

        IDeck Deck { get; }

        List<ICard> Hand { get; }

        IStructureManager StructureManager { get; }

        ICardLoader CardLoader { get; }

        bool CanAfford(Dictionary<ResourceType, int> cost);

        void DoTurnEndActions();

        void DoTurnStartActions();

        Dictionary<ResourceType, int> GetResources();

        void ModifyResource(ResourceType resource, int amount);

        void TakeTurn();

        int GetMaxHandSize();

        void DrawCard(ICard card);

        void Draw();
    }
}