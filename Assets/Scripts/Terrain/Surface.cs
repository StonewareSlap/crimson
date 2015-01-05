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

    // The direction of this terrain. This is used to convert the requested displacement into the screen space displacement.
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
    // Initialize members used by the CustomEditor and Game.
    private void InitializeCommon()
    {
        m_Surface = collider2D as PolygonCollider2D;
        m_Surface.isTrigger = true;
    }

    // ------------------------------------------------------------------------
    // Calculate the resulting navigation parameters from the input.
    // @Note: This method is called from the fixed update (physics step).
    public void Navigation(Navigation.Input input, Navigation.Output output)
    {
        output.m_Height = input.m_Height;

        if (input.m_Velocity.sqrMagnitude < float.Epsilon)
        {
            output.m_Velocity = Vector2.zero;
            return;
        }
            
        // The first step is to transform the requestedVelocity into the surface referential.
        var transformedVelocity = m_HorizontalDirection.normalized * input.m_Velocity.x + m_VerticalDirection.normalized * input.m_Velocity.y;
        var transformedVelocityStep = transformedVelocity * Time.fixedDeltaTime;

        // We now check if the transformed velocity for this step will keep the origin inside the surface.
        if (m_Surface.OverlapPoint(input.m_Origin + transformedVelocityStep))
        {
            output.m_Velocity = transformedVelocity;
            return;
        }

        // Since the transformed velocity for this step will move the origin out of the surface we need to check the edges.
        var raycastOrigin = input.m_Origin + transformedVelocityStep;
        var raycastDirection = transformedVelocity.normalized * -1.0f;
        var raycastDistance = (raycastOrigin - input.m_Origin).magnitude + 1.0f;        
        var raycastHits = Physics2D.RaycastAll(raycastOrigin, raycastDirection, raycastDistance);
        foreach (var hit in raycastHits)
        {
            if (hit.collider != m_Surface)
            {
                continue;
            }
 
            // Find the edge we would be crossing if the transformed displacement was applied.
            SurfaceEdge surfaceEdge = FindClosestEdge(hit.point);
            if (surfaceEdge == null)
            {
                // Error, we should always find an edge.
                Debug.LogError("Terrain.Surface.CalculateDisplacement " + name + ": Edge not found, preventing motion." + hit.point.ToString());
                output.m_Velocity = Vector2.zero;
                return;
            }

            // Set the new input values and ask the edge to calculate the velocity.
            input.m_Velocity = transformedVelocity;
            input.m_Intersection = hit.point;
            surfaceEdge.Navigation(input, output);
            return;
        }

        // Error, the raycast should've hit the surface collider.
        Debug.LogError("Terrain.Surface.CalculateDisplacement " + name + ": Raycast failed to hit the collider, preventing motion.");
        output.m_Velocity = Vector2.zero;
    }

    // ------------------------------------------------------------------------
    SurfaceEdge FindClosestEdge(Vector2 point)
    {
        if (m_SurfaceEdges.Count > 0)
        {
            return m_SurfaceEdges[0];
        }

        return null;
    }

    // ------------------------------------------------------------------------
    public void OnSceneGUI()
    {
        var position = transform.position;

        // Display the directions.
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawLine(position, position + (Vector3)m_HorizontalDirection.normalized);
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawLine(position, position + (Vector3)m_VerticalDirection.normalized);

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
