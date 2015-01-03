using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Terrain {

// ----------------------------------------------------------------------------
[RequireComponent(typeof(PolygonCollider2D))]
public class Surface : MonoBehaviour
{
    // The surface used to define the terrain's boundarie. For now, we used a polygon collider.
    protected PolygonCollider2D m_Surface;

    // The surface edges metadata.
    public List<SurfaceEdge> m_SurfaceEdges = new List<SurfaceEdge>();

    // The direction of this terrain. This is used to convert the movement input into the screen space displacement.
    public Vector2 m_HorizontalDirection = new Vector2(1.0f, 0.0f);
    public Vector2 m_VerticalDirection = new Vector2(0.0f, -1.0f);

    // ------------------------------------------------------------------------
    public int EdgeCount { get { return m_Surface.points.Length; } }

    // ------------------------------------------------------------------------
    private void Start()
    {
        Initialize();
    }

    // ------------------------------------------------------------------------   
    public void Initialize()
    {
        InitializeCommon();
    }

    // ------------------------------------------------------------------------
    private void InitializeCommon()
    {
        m_Surface = collider2D as PolygonCollider2D;
        m_Surface.isTrigger = true;
    }

    // ------------------------------------------------------------------------
    public void OnSceneGUI()
    {
        var position = transform.position;

        // Display the directions.
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawLine(position, position + (Vector3)m_HorizontalDirection.normalized * 3.0f);
        UnityEditor.Handles.color = Color.blue;
        UnityEditor.Handles.DrawLine(position, position + (Vector3)m_VerticalDirection.normalized * 3.0f);

        // Display the surface edges.        
        if (m_Surface != null && EdgeCount == m_SurfaceEdges.Count)
        {
            var points = m_Surface.points;
            Vector2 pointA, pointB;
            for (int i = 0; i < points.Length; ++i)
            {
                pointA = transform.TransformPoint(points[i]);
                pointB = (i < points.Length - 1) ? transform.TransformPoint(points[i + 1]) : transform.TransformPoint(points[0]);

                UnityEditor.Handles.color = m_SurfaceEdges[i].ToColor();
                UnityEditor.Handles.DrawLine(pointA, pointB);
                UnityEditor.Handles.Label((Vector3)(pointA + pointB) * 0.5f, i.ToString());
            }
        }
    }
}

} // namespace Terrain
