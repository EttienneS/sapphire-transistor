﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Map.Pathing
{
    public class Pathfinder : MonoBehaviour
    {
        private Task _currentTask;
        private Queue<PathRequest> _pathQueue = new Queue<PathRequest>();
        private CellPriorityQueue _searchFrontier;
        private int _searchFrontierPhase;

        public void AddPathRequest(PathRequest pathRequest)
        {
            _pathQueue.Enqueue(pathRequest);
        }

        public void Update()
        {
            ResolvePaths();
        }

        internal void ResolveAll()
        {
            while (_pathQueue.Count > 0)
            {
                ResolvePaths();
            }
        }

        private void ResolvePathRequest(PathRequest request)
        {
            try
            {
                if (SearchForPath(request))
                {
                    var fromCell = request.From;
                    var toCell = request.To;
                    if (fromCell != null && toCell != null)
                    {
                        var path = new List<Cell> { toCell };
                        var current = toCell;
                        while (current != fromCell)
                        {
                            current = current.PathFrom;
                            path.Add(current);
                        }
                        path.Reverse();
                        request.PopulatePath(path);
                        request.PathFound();
                    }
                }
                else
                {
                    request.PathInvalid();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                _currentTask = null;
            }
        }

        private void ResolvePaths()
        {
            if (_currentTask == null || _currentTask.IsCompleted)
            {
                if (_pathQueue.Count != 0)
                {
                    _currentTask = new Task(() => ResolvePathRequest(_pathQueue.Dequeue()));
                    _currentTask.Start();
                }
            }
        }

        private bool SearchForPath(PathRequest request)
        {
            try
            {
                var fromCell = request.From;
                var toCell = request.To;

                _searchFrontierPhase += 2;

                if (_searchFrontier == null)
                {
                    _searchFrontier = new CellPriorityQueue();
                }
                else
                {
                    _searchFrontier.Clear();
                }

                fromCell.SearchPhase = _searchFrontierPhase;
                fromCell.SearchDistance = 0;
                _searchFrontier.Enqueue(fromCell);

                while (_searchFrontier.Count > 0)
                {
                    var current = _searchFrontier.Dequeue();
                    current.SearchPhase++;

                    if (current == toCell)
                    {
                        return true;
                    }

                    for (var d = Direction.N; d <= Direction.NW; d++)
                    {
                        var neighbor = current.GetNeighbor(d);

                        if (neighbor == null || neighbor.SearchPhase > _searchFrontierPhase)
                        {
                            continue;
                        }

                        var neighborTravelCost = request.CalculateTravelCost.Invoke(current, neighbor);
                        if (neighborTravelCost < 0)
                        {
                            continue;
                        }

                        var distance = current.SearchDistance + neighborTravelCost;
                        if (neighbor.SearchPhase < _searchFrontierPhase)
                        {
                            neighbor.SearchPhase = _searchFrontierPhase;
                            neighbor.SearchDistance = distance;
                            neighbor.PathFrom = current;
                            neighbor.SearchHeuristic = neighbor.DistanceTo(toCell);
                            _searchFrontier.Enqueue(neighbor);
                        }
                        else if (distance < neighbor.SearchDistance)
                        {
                            var oldPriority = neighbor.SearchPriority;
                            neighbor.SearchDistance = distance;
                            neighbor.PathFrom = current;
                            _searchFrontier.Change(neighbor, oldPriority);
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Pathing error: " + ex.ToString());
                throw;
            }
        }
    }
}