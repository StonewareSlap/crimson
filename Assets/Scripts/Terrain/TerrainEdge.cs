using UnityEngine;
using System;
using System.Collections;

namespace Terrain {

// ----------------------------------------------------------------------------
[RequireComponent(typeof(EdgeCollider2D))]
public class TerrainEdge : MonoBehaviour
{
    // Edge collider associated with this class. It is used as the base collider for all types.
    public EdgeCollider2D m_EdgeCollider;

    // ------------------------------------------------------------------------
    private void Start()
    {
        Initialize();
    }

    // ------------------------------------------------------------------------   
    public void Initialize()
    {
        m_EdgeCollider = collider2D as EdgeCollider2D;
    }

    // ------------------------------------------------------------------------
#if UNITY_EDITOR
    public void OnSceneGUI()
    {
        // Display the edge.
        if (m_EdgeCollider != null)
        {
            UnityEditor.Handles.color = Color.red;

            var points = m_EdgeCollider.points;
            Vector2 pointA, pointB;
            for (int i = 0; i < points.Length - 1; ++i)
            {
                pointA = transform.TransformPoint(points[i]);
                pointB = transform.TransformPoint(points[i + 1]);

                UnityEditor.Handles.DrawLine(pointA, pointB);
                //UnityEditor.Handles.Label((Vector3)(pointA + pointB) * 0.5f, i.ToString(), UnityEditor.EditorStyles.miniLabel);

            }
        }
    }
#endif
}

} // namespace Edge
