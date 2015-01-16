using UnityEngine;
using UnityEditor;
using System.Collections;

// ----------------------------------------------------------------------------
namespace Terrain
{
    // ----------------------------------------------------------------------------
    [CustomEditor(typeof(TerrainEdge))]
    public class TerrainEdgeEditor : Editor
    {
        private SerializedObject m_Object;

        // ------------------------------------------------------------------------
        public void OnEnable()
        {
            var TerrainEdge = target as TerrainEdge;
            TerrainEdge.Initialize();

            m_Object = new SerializedObject(target);
        }

        // ------------------------------------------------------------------------
        public override void OnInspectorGUI()
        {
            m_Object.Update();

            GUILayout.Label("Terrain.TerrainEdge\n----------", EditorStyles.boldLabel);

            m_Object.ApplyModifiedProperties();
        }
    }

} // namespace Terrain