using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Terrain {

// ----------------------------------------------------------------------------
[CustomEditor(typeof(Surface))]
public class SurfaceEditor : Editor
{
    private SerializedObject m_object;
    private SerializedProperty m_horizontalDirection;
    private SerializedProperty m_verticalDirection;
    private SerializedProperty m_edgeMetadatas;

    // ------------------------------------------------------------------------
    public void OnEnable()
    {
        m_object = new SerializedObject(target);

        m_horizontalDirection = m_object.FindProperty("m_HorizontalDirection");
        m_verticalDirection = m_object.FindProperty("m_VerticalDirection");
        m_edgeMetadatas = m_object.FindProperty("m_edgeMetadata.Array");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI()
    {
        m_object.Update();

        GUILayout.Label("Direction", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(m_horizontalDirection);
        EditorGUILayout.PropertyField(m_verticalDirection);

        //var edgeMetadatas = GetEdgeMetadataArray();
        //if (edgeMetadatas != null)
        //{
        //    for (int i = 0; i < edgeMetadatas.Length; ++i)
        //    {
        //        var result = EditorGUILayout.ObjectField(edgeMetadatas[i], typeof(WalkableSurface.EdgeMetadata), true);
        //    }
        //}

        m_object.ApplyModifiedProperties();
    }

    // ------------------------------------------------------------------------
    private void OnSceneGUI()
    {
        var walkableSurface = target as Surface;

        // Display the surface's direction.
        Handles.color = Color.green;
        Handles.DrawLine(walkableSurface.transform.position, walkableSurface.transform.position + (Vector3)m_horizontalDirection.vector2Value.normalized * 3.0f);
        Handles.color = Color.blue;
        Handles.DrawLine(walkableSurface.transform.position, walkableSurface.transform.position + (Vector3)m_verticalDirection.vector2Value.normalized * 3.0f);

        // Display the surface's edges.
        //walkableSurface.OnSceneGUI();
    }

    // ------------------------------------------------------------------------
    //private WalkableSurface.EdgeMetadata[] GetEdgeMetadataArray()
    //{
    //    var size = m_object.FindProperty("m_edgeMetadata.Array.size").intValue;
    //    var edgeMetadatas = new WalkableSurface.EdgeMetadata[size];

    //    for (int i = 0; i < size; ++i)
    //    {
    //        edgeMetadatas[i] = (m_object.FindProperty(string.Format("m_edgeMetadata.Array.data[{0}]", i)).objectReferenceValue) as WalkableSurface.EdgeMetadata;
    //    }

    //    return edgeMetadatas;

    //    return null;
    //}
}

} // namespace Terrain