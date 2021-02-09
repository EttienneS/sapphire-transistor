using Assets.Cards;
using Assets.Helpers;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.Structures;
using Assets.UI;
using System.Linq;

namespace Assets.Factions
{
    public class PlayerFaction : FactionBase
    {
        private readonly IUIManager _uiManager;

        private ICard _activeCard;

        private (ICard card, ICoord coord)? _activePreview;

        public PlayerFaction(string name, IServiceLocator serviceLocator) : base(name, serviceLocator)
        {
            _uiManager = serviceLocator.Find<IUIManager>();
            CellEventManager.OnCellClicked += CellClicked;
            CardEventManager.OnSetPlayerCardActive += OnPlayerCardActive;
        }

        public void CancelCard()
        {
            ClearPreview();
        }

        public void CellClicked(Cell cell)
        {
            if (UIHelper.MouseOverUi())
            {
                return;
            }

            if (TryGetActiveCard(out ICard activeCard))
            {
                if (_activePreview.HasValue && _activePreview.Value.coord == cell.Coord && _activePreview.Value.card == activeCard)
                {
                    ConfirmCard();
                }
                else
                {
                    PreviewCard(activeCard, cell.Coord);
                }
            }
        }

        public void ConfirmCard()
        {
            var card = _activePreview.Value.card;
            var coord = _activePreview.Value.coord;
            var cost = card.GetCost();

            if (card.CanPlay(coord) && CanAfford(cost))
            {
                card.Play(coord);
                RemoveResources(cost);

                Hand.Remove(_activeCard);
                _activeCard = null;
            }
            ClearPreview();
        }

        public void PreviewCard(ICard card, ICoord coord)
        {
            ClearPreview();
            _activePreview = (card, coord);
            _activePreview.Value.card.Preview(_activePreview.Value.coord);
        }

        public void ResetUI()
        {
            _uiManager.DisableHighlights();
            _uiManager.MessageManager.HideAll();
        }

        public override void TakeTurn()
        {
            var connected = StructureManager.GetStructuresLinkedTo(StructureManager.GetCore());

            foreach (var structure in connected)
            {
                StructureEventManager.HideHighlight(structure);
            }

            foreach (var structure in StructureManager.GetStructures().Except(connected))
            {
                StructureEventManager.ShowHiglight(structure);
            }

            // foreach (var structure in StructureManager.GetStructuresLinkedTo(core))

            //AddResources(StructureManager.GetCombinedYield());
            //var yield = new Dictionary<ResourceType, int>();
            //foreach (var structure in GetStructures())
            //{
            //    foreach (var res in structure.GetYield(structure))
            //    {
            //        if (!yield.ContainsKey(res.Key))
            //        {
            //            yield.Add(res.Key, 0);
            //        }
            //        yield[res.Key] += res.Value;
            //    }
            //}

            //return yield;
        }

        public bool TryGetActiveCard(out ICard card)
        {
            card = _activeCard;
            if (_activeCard == null)
            {
                return false;
            }
            return true;
        }

        private void ClearPreview()
        {
            if (_activePreview.HasValue)
            {
                _activePreview.Value.card.ClearPreview();
                _activePreview = null;
            }
        }

        private void OnPlayerCardActive(ICard card)
        {
            if (Hand.Contains(card))
            {
                ClearPreview();
                _activeCard = card;
            }
            else
            {
                throw new System.Exception($"Card not in hand!: {card}");
            }
        }
    }
}