using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Terrain {

// ----------------------------------------------------------------------------
[RequireComponent(typeof(PolygonCollider2D))]
public class TerrainSurface : MonoBehaviour
{
    // The surface used to define the boundaries.
    protected PolygonCollider2D m_Surface;

    // The direction of this terrain. This is used to convert the requested displacement into the screen space displacement.
    public Vector2 m_HorizontalDirection = new Vector2(1.0f, 0.0f);
    public Vector2 m_VerticalDirection = new Vector2(0.0f, 1.0f);

    // ------------------------------------------------------------------------
    private void Start()
    {
        Initialize();
    }

    // ------------------------------------------------------------------------   
    public void Initialize()
    {
        m_Surface = collider2D as PolygonCollider2D;
        m_Surface.isTrigger = true;
    }

    // ------------------------------------------------------------------------
    // Calculate the resulting navigation parameters from the input.
    // @Note: This method is called from the fixed update (physics step).
    public void Navigation(TerrainNavigationInput input, TerrainNavigationOutput output)
    {            
        output.m_Velocity = m_HorizontalDirection.normalized * input.m_Velocity.x + m_VerticalDirection.normalized * input.m_Velocity.y;
        output.m_Height = input.m_Height;
    }

    // ------------------------------------------------------------------------
    public void OnSceneGUI()
    {
        Vector2 centerPosition = Vector2.zero;

        // Display the surface TerrainEdges.        
        if (m_Surface != null)
        {
            var points = m_Surface.points;
            Vector2 pointA, pointB;
            for (int i = 0; i < points.Length; ++i)
            {
                pointA = transform.TransformPoint(points[i]);
                pointB = (i < points.Length - 1) ? transform.TransformPoint(points[i + 1]) : transform.TransformPoint(points[0]);

                centerPosition += pointA;

                UnityEditor.Handles.color = Color.green;
                UnityEditor.Handles.DrawLine(pointA, pointB);
                UnityEditor.Handles.Label((Vector3)(pointA + pointB) * 0.5f, i.ToString());
            }

            centerPosition = points.Length > 0 ? centerPosition / points.Length : Vector2.zero;
        }

        // Display the directions.
        var position = (Vector3)centerPosition;
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawLine(position, position + (Vector3)m_HorizontalDirection.normalized);
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawLine(position, position + (Vector3)m_VerticalDirection.normalized);
    }
}

} // namespace Terrain
