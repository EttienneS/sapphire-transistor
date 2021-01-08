using Assets.Map;
using UnityEngine;

namespace Assets.UI
{
    public interface IUIManager
    {
        RadialMenuManager RadialMenuManager { get; set; }

        void HighlightCell(ICoord coord, Color color);

        void DisableHighlight();
    }
}