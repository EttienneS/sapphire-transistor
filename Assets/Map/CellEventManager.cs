using UnityEngine;

namespace Assets.Map
{
    public static class CellEventManager
    {
        public delegate void CellClickedDelegate(Cell cell);

        public delegate void MouseOverCellDelegate(Cell cell);

        public static CellClickedDelegate OnCellClicked;
        public static MouseOverCellDelegate OnMouseOver;

        public static void CellClicked(Cell cell)
        {
            Debug.Log($"Clicked {cell.Coord.X}-{cell.Coord.Z}");
            OnCellClicked?.Invoke(cell);
        }

        public static void MouseOverCell(Cell cell)
        {
            // Debug.Log($"Mouse over {cell.Coord.X}-{cell.Coord.Z}");
            OnMouseOver?.Invoke(cell);
        }
    }
}