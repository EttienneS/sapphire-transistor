using UnityEngine;

namespace Assets.Structures
{
    public static class StructureEventManager
    {
        public static event StructureDelegates.StructureDestroyedDelegate OnStructureDestroyed;

        public static event StructureDelegates.StructurePlannedDelegate OnStructurePlanned;

        public static event StructureDelegates.StructureHighlightDelegate OnShowHighlight;
        public static event StructureDelegates.StructureHighlightDelegate OnHideHighlight;

        public static void ShowHiglight(IStructure structure)
        {
            OnShowHighlight?.Invoke(structure);
        }

        public static void HideHighlight(IStructure structure)
        {
            OnHideHighlight?.Invoke(structure);
        }

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