using UnityEngine;
using System;
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

    // The angle between both directions.
    private float m_HorizontalAngle;

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
        m_HorizontalDirection = m_HorizontalDirection.normalized;
        m_VerticalDirection = m_VerticalDirection.normalized;
        m_HorizontalAngle = (float)Math.Sin(Math.Acos(Vector2.Dot(m_HorizontalDirection, Vector2.right)));
    }

    // ------------------------------------------------------------------------
    public void Navigation(TerrainNavigationInput input, TerrainNavigationOutput output)
    {
        output.m_Velocity = m_HorizontalDirection * input.m_Velocity.x + m_VerticalDirection * input.m_Velocity.y;
        output.m_Height = (input.m_Height < float.Epsilon) ? input.m_Height : input.m_Height + m_HorizontalAngle * Vector2.Dot(output.m_Velocity, m_HorizontalDirection) * Time.fixedDeltaTime;
    }

#if UNITY_EDITOR
    // ------------------------------------------------------------------------
    public void OnSceneGUI()
    {
        Vector2 centerPosition = Vector2.zero;

        // Display the surface TerrainEdges.        
        if (m_Surface != null)
        {
            UnityEditor.Handles.color = Color.yellow;          
            
            var points = m_Surface.points;
            Vector2 pointA, pointB;
            for (int i = 0; i < points.Length; ++i)
            {
                pointA = transform.TransformPoint(points[i]);
                pointB = (i < points.Length - 1) ? transform.TransformPoint(points[i + 1]) : transform.TransformPoint(points[0]);

                centerPosition += pointA;

                UnityEditor.Handles.DrawLine(pointA, pointB);
                //UnityEditor.Handles.Label((Vector3)(pointA + pointB) * 0.5f, i.ToString(), UnityEditor.EditorStyles.miniLabel);
            }

            centerPosition = points.Length > 0 ? centerPosition / points.Length : Vector2.zero;
        }

        // Display the directions.
        var position = (Vector3)centerPosition;
        UnityEditor.Handles.color = Color.white;
        UnityEditor.Handles.DrawLine(position, position + (Vector3)m_HorizontalDirection.normalized);
        UnityEditor.Handles.color = Color.black;
        UnityEditor.Handles.DrawLine(position, position + (Vector3)m_VerticalDirection.normalized);
    }
#endif
}

} // namespace Terrain
