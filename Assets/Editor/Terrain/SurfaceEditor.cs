using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Terrain {

// ----------------------------------------------------------------------------
[CustomEditor(typeof(Surface))]
public class SurfaceEditor : Editor
{
    private SerializedObject m_Object;
    private SerializedProperty m_HorizontalDirection;
    private SerializedProperty m_VerticalDirection;
    private SerializedProperty m_SurfaceEdges;

    // ------------------------------------------------------------------------
    public void OnEnable()
    {
        var surface = target as Surface;
        surface.Initialize();

        m_Object = new SerializedObject(target);

        m_HorizontalDirection = m_Object.FindProperty("m_HorizontalDirection");
        m_VerticalDirection = m_Object.FindProperty("m_VerticalDirection");
        m_SurfaceEdges = m_Object.FindProperty("m_SurfaceEdges.Array");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI()
    {
        m_Object.Update();

        GUILayout.Label("TERRAIN\n----------", EditorStyles.boldLabel);

        GUILayout.Label("Direction", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(m_HorizontalDirection);
        EditorGUILayout.PropertyField(m_VerticalDirection);

        GUILayout.Label("Edges", EditorStyles.boldLabel);

        var surface = target as Surface;
        if (surface != null)
        {
            // Make sure the array match the surface edge count.
            var size = m_SurfaceEdges.arraySize;
            if (size != surface.EdgeCount)
            {                
                while (surface.EdgeCount > m_SurfaceEdges.arraySize)
                {
                    m_SurfaceEdges.InsertArrayElementAtIndex(m_SurfaceEdges.arraySize);
                }

                while (surface.EdgeCount < m_SurfaceEdges.arraySize)
                {
                    m_SurfaceEdges.DeleteArrayElementAtIndex(m_SurfaceEdges.arraySize - 1);
                }               
            }

            for (int i = 0; i < m_SurfaceEdges.arraySize; ++i)
            {
                SerializedProperty element = m_SurfaceEdges.GetArrayElementAtIndex(i);
                
                var type = element.FindPropertyRelative("m_Type");
                EditorGUILayout.PropertyField(type);
            }
        }

        m_Object.ApplyModifiedProperties();
    }

    // ------------------------------------------------------------------------
    private void OnSceneGUI()
    {
        // Display the surface informations.
        var surface = target as Surface;
        if (surface != null)
        {
            surface.OnSceneGUI();
        }
    }
}

} // namespace Terrain