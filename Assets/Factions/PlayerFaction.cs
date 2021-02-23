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

        public IDeckManager DeckManager;

        public PlayerFaction(string name, IServiceLocator serviceLocator) : base(name, serviceLocator)
        {
            _uiManager = serviceLocator.Find<IUIManager>();
            CellEventManager.OnCellClicked += CellClicked;

            DeckManager = new DeckManager(this);
        }

        public void CellClicked(Cell cell)
        {
            if (UIHelper.MouseOverUi())
            {
                return;
            }

            DeckManager.CellClicked(cell);
        }

        public void ResetUI()
        {
            _uiManager.DisableHighlights();
            _uiManager.MessageManager.HideAll();
        }

        public override void TakeTurn()
        {
            DeckManager.DrawToHandSize();

            var connected = StructureManager.GetStructuresLinkedTo(StructureManager.GetCore());
            
            HighlightConnected(connected);
            GetYieldForConnected(connected);
        }

        private void GetYieldForConnected(System.Collections.Generic.List<IStructure> connected)
        {
            foreach (var structure in connected)
            {
                foreach (var resource in structure.GetYield(structure))
                {
                    ModifyResource(resource.Key, resource.Value);
                }
            }
        }

        private void HighlightConnected(System.Collections.Generic.List<IStructure> connected)
        {
            foreach (var structure in connected)
            {
                StructureEventManager.HideHighlight(structure);
            }

            foreach (var structure in StructureManager.GetStructures().Except(connected).Where(s => s.RequiresLink))
            {
                StructureEventManager.ShowHiglight(structure);
            }
        }

        public override void EndTurn()
        {
            DeckManager.DiscardHand();
            base.EndTurn();
        }
    }
}