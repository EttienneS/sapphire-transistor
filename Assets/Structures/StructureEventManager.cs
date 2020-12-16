using UnityEngine;

namespace Assets.Structures
{
    public class StructureEventManager
    {
        public static event StructureDelegates.StructureBuildCompletedDelegate OnStructureBuildCompleted;

        public static event StructureDelegates.StructureBuildProgressDelegate OnStructureBuildProgress;

        public static event StructureDelegates.StructureDestroyedDelegate OnStructureDestroyed;

        public static event StructureDelegates.StructurePlannedDelegate OnStructurePlanned;

        public static void StructureDestroyed(IStructure structure)
        {
            Debug.Log($"Structure destroyed {structure}");
            OnStructureDestroyed?.Invoke(structure);
        }

        public static void StructurePlanned(IStructure structure)
        {
            Debug.Log($"Structure planned {structure}");
            OnStructurePlanned?.Invoke(structure);
        }

        public static void StructureBuildProgress(IStructure structure, int current, int max)
        {
            Debug.Log($"Structure progress {structure} {current}/{max}");
            OnStructureBuildProgress?.Invoke(structure, current, max);
        }

        public static void StructureBuildCompleted(IStructure structure)
        {
            Debug.Log($"Structure completed {structure}");
            OnStructureBuildCompleted?.Invoke(structure);
        }
    }
}