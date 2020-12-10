using UnityEditor;
using UnityEngine;

namespace Assets
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var myTarget = (MapGenerator)target;

            GUILayout.Space(10);
            if (GUILayout.Button("Regenerate World"))
            {
                myTarget.RegenerateMap();
            }
        }
    }
}