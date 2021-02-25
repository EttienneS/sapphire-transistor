using Assets.Map;
using UnityEngine;

namespace Assets.UI
{
    public interface IUIManager
    {
        RadialMenuManager RadialMenuManager { get; set; }
        MessageManager MessageManager { get; set; }

        void HighlightCells(ICoord[] coord, Color color);

        void DisableHighlights();

        void ShowDrawView();
        void HideDrawView();
    }
}