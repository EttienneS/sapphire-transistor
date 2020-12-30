using Assets.Factions;
using Assets.Map.Pathing;
using Assets.ServiceLocator;
using Assets.Structures;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Map
{
    public class RoadManager : MonoBehaviour
    {
        private Pathfinder _pathfinder;

        private List<LineRenderer> _lines;

        private IFactionManager _factionManager;

        public void Load(Pathfinder pathfinder)
        {
            _pathfinder = pathfinder;
            _lines = new List<LineRenderer>();

            _factionManager = Locator.Instance.Find<IFactionManager>();

            StructureEventManager.OnStructureBuildCompleted += StructureEventManager_OnStructureBuildCompleted;
            StructureEventManager.OnStructureDestroyed += StructureEventManager_OnStructureDestroyed;
        }

        private void StructureEventManager_OnStructureDestroyed(IStructure structure)
        {
        }

        private void StructureEventManager_OnStructureBuildCompleted(IStructure structure)
        {
        }
    }
}