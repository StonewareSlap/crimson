using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Terrain {

// ----------------------------------------------------------------------------
[CustomEditor(typeof(TerrainSurface))]
public class TerrainSurfaceEditor : Editor
{
    private SerializedObject m_Object;
    private SerializedProperty m_HorizontalDirection;
    private SerializedProperty m_VerticalDirection;

    // ------------------------------------------------------------------------
    public void OnEnable()
    {
        var surface = target as TerrainSurface;
        surface.Initialize();

        m_Object = new SerializedObject(target);

        m_HorizontalDirection = m_Object.FindProperty("m_HorizontalDirection");
        m_VerticalDirection = m_Object.FindProperty("m_VerticalDirection");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI()
    {
        m_Object.Update();

        GUILayout.Label("Terrain.Surface\n----------", EditorStyles.boldLabel);

        GUILayout.Label("Direction", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(m_HorizontalDirection);
        EditorGUILayout.PropertyField(m_VerticalDirection);        

        m_Object.ApplyModifiedProperties();
    }

    // ------------------------------------------------------------------------
    public void OnSceneGUI()
    {
        // Display the surface informations.
        var surface = target as TerrainSurface;
        if (surface != null)
        {
            surface.OnSceneGUI();
        }
    }
}

} // namespace Terrain