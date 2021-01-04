using Assets.Map;
using UnityEngine;

namespace Assets.UI
{
    public interface IUIManager
    {
        RadialMenuManager RadialMenuManager { get; set; }

        void HighlightCell(Cell cell, Color color);

        void DisableHighlight();
    }
}