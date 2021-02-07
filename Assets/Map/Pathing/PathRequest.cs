using System.Collections.Generic;
using UnityEngine;

namespace Assets.Map.Pathing
{
    public static class PathfinderDelegates
    {
        public delegate void PathCompletedDelegate(List<Cell> path);

        public delegate void PathInvalidDelegate();

        public delegate int CalculateTravelCostForCellDelegate(Cell fromCell, Cell toCell);
    }

    public class PathRequest
    {
        private List<Cell> _path;

        public event PathfinderDelegates.PathCompletedDelegate OnPathCompleted;

        public event PathfinderDelegates.PathInvalidDelegate OnPathInvalid;

        public PathfinderDelegates.CalculateTravelCostForCellDelegate CalculateTravelCost { get; }

        public PathRequest(Cell from,
                           Cell to,
                           PathfinderDelegates.CalculateTravelCostForCellDelegate canTraverseCell,
                           PathfinderDelegates.PathCompletedDelegate onPathCompleted,
                           PathfinderDelegates.PathInvalidDelegate onPathInvalid)
        {
            From = from;
            To = to;
            OnPathCompleted += onPathCompleted;
            OnPathInvalid += onPathInvalid;

            CalculateTravelCost = canTraverseCell;
        }

        public Cell From { get; set; }
        public Cell To { get; set; }

        public List<Cell> GetPath()
        {
            return _path;
        }

        public void PathInvalid()
        {
            Debug.LogWarning($"No path found from {From} to {To}");
            OnPathInvalid?.Invoke();
        }

        public void PopulatePath(List<Cell> path)
        {
            _path = path;
        }

        internal void PathFound()
        {
            Debug.Log($"Path found from {From} to {To}");
            OnPathCompleted?.Invoke(_path);
        }
    }
}