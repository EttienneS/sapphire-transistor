using UnityEngine;

namespace Assets.Structures
{
    public static class StructureEventManager
    {
        public static event StructureDelegates.StructureDestroyedDelegate OnStructureDestroyed;

        public static event StructureDelegates.StructurePlannedDelegate OnStructurePlanned;

        public static void StructureDestroyed(IStructure structure)
        {
            OnStructureDestroyed?.Invoke(structure);
        }

        public static void StructurePlanned(IStructure structure)
        {
            OnStructurePlanned?.Invoke(structure);
        }
    }
}