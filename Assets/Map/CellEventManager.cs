﻿using UnityEngine;

namespace Assets.Map
{
    public static class CellEventManager
    {
        public delegate void CellClickedDelegate(Cell cell);

        public static CellClickedDelegate OnCellClicked;

        public static void CellClicked(Cell cell)
        {
            Debug.Log($"Clicked {cell.Coord.X}-{cell.Coord.Z}");
            OnCellClicked.Invoke(cell);
        }
    }
}