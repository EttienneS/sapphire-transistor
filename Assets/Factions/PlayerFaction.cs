using Assets.Map;
using Assets.ServiceLocator;
using Assets.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Factions
{
    public class PlayerFaction : FactionBase
    {
        private IUIManager _uiManager;

        public PlayerFaction(string name, IServiceLocator serviceLocator) : base(name, serviceLocator)
        {
            _uiManager = serviceLocator.Find<IUIManager>();
            CellEventManager.OnCellClicked += CellClicked;
        }

        public override void TakeTurn()
        {
        }

        public void CellClicked(Cell cell)
        {
            if (MouseOverUi())
            {
                return;
            }

            var elements = new List<(string, RadialMenuDelegates.MenuItemClicked)>();

            foreach (var structure in StructureManager.GetBuildableStructures())
            {
                elements.Add(($"{structure.Name}", () =>
                {
                    StructureManager.AddStructure(structure, cell.Coord);
                }
                ));
            }
            _uiManager.RadialMenuManager.ShowRadialMenu(true, elements.ToArray());
        }

        private bool MouseOverUi()
        {
            if (EventSystem.current == null)
            {
                // event system not on yet
                return false;
            }

            return EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject != null;
        }
    }
}