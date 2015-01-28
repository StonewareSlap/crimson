using UnityEngine;
using System.Collections;

namespace Terrain {

// ----------------------------------------------------------------------------
[RequireComponent(typeof(EdgeCollider2D))]
public class TerrainJump : MonoBehaviour
{
    // The edge collider used to define the jump highest section. This TerrainEdge can always be crossed on contact.
    public EdgeCollider2D m_TopEdge;

    // The edge collider used to define the jump lowest section. This TerrainEdge can be crossed when the object reaches a data defined height.
    public EdgeCollider2D m_BottomEdge;

    // The direction of the jump.
    public Vector2 m_Direction;

    // The direction from the top to bottom edge. It is used as the raycast direction.
    private Vector2 m_TopToBottomDirection;

    // The associated TerrainPlatform (optional)
    private TerrainPlatform m_TerrainPlatform;

    // TerrainEdge collision layer.
    private static int m_EdgeLayerMask = LayerMask.GetMask("TerrainEdge");

    // ----------------------------------------------------------------------------
    private void Start()
    {
        Initialize();
    }

    // ------------------------------------------------------------------------   
    public void Initialize()
    {
        m_Direction = m_Direction.normalized;
        m_TopToBottomDirection = Vector2.up * -1.0f;
        
        // Cache the associated TerrainPlatform.
        m_TerrainPlatform = transform.parent ? transform.parent.GetComponent<TerrainPlatform>() : null;
        if (m_TerrainPlatform && !m_TerrainPlatform.m_TerrainJumps.Contains(this)) m_TerrainPlatform = null;
    }

    // ----------------------------------------------------------------------------
    public void Navigation(TerrainNavigationInput input, TerrainNavigationOutput output)
    {
        // Configure the data depending on which edge we touched.
        var bCollidedWithBottomEdge = false;
        var raycastEdge = m_BottomEdge;
        var raycastDirection = m_TopToBottomDirection;
        var direction = m_Direction;
        
        if (input.m_Collision.collider == m_BottomEdge)
        {
            bCollidedWithBottomEdge = true;
            raycastEdge = m_TopEdge;
            raycastDirection *= -1.0f;
            direction *= -1.0f;
        }

        // Update the TerrainPlatform's collision (optional).
        if (m_TerrainPlatform && !m_TerrainPlatform.UpdateCollision(input, bCollidedWithBottomEdge, false))
        {
            return;
        }

        // Check if the input velocity is in the required direction.
        if (Vector2.Dot(input.m_Velocity, direction) > 0.0f)
        {
            // Find where the origin should be moved.
            var contactPoint = input.m_Collision.contacts[0].point;
            var hits = Physics2D.RaycastAll(contactPoint, raycastDirection, 100.0f);
            foreach (var hit in hits)
            {
                if (hit.collider == raycastEdge)
                {
                    var heightDelta = Vector2.Dot(hit.point - contactPoint, raycastDirection);

                    // Make sure we have reached the required height for this case.
                    if (bCollidedWithBottomEdge && input.m_Height < heightDelta)
                    {
                        break;
                    }

                    // Prevent the jump if the destination is not collision free (skip current jump collision).
                    var destination = hit.point + direction * input.m_Radius;
                    var destinationHits = Physics2D.OverlapCircleAll(destination, input.m_Radius, m_EdgeLayerMask);
                    foreach (var destinationHit in destinationHits)
                    {
                        if (destinationHit == m_TopEdge || destinationHit == m_BottomEdge)
                        {
                            continue;
                        }

                        Prevent(input, output);
                        break;
                    }

                    // Success.                   
                    output.m_Origin = hit.point + direction * input.m_Radius;
                    output.m_Velocity = input.m_Velocity;
                    output.m_Height = (raycastEdge == m_TopEdge) ? input.m_Height - heightDelta : input.m_Height + heightDelta;

                    // Update the TerrainPlatform's collision (optional transition).
                    if (m_TerrainPlatform) m_TerrainPlatform.UpdateCollision(input, bCollidedWithBottomEdge, true);

                    return;
                }
            }
        }

        Prevent(input, output);
        return;
    }

    // ------------------------------------------------------------------------
    private void Prevent(TerrainNavigationInput input, TerrainNavigationOutput output)
    {
        output.m_Origin = input.m_Origin;
        output.m_Velocity = input.m_Velocity;
        output.m_Height = input.m_Height;
    }

#if UNITY_EDITOR
    // ------------------------------------------------------------------------
    public void OnSceneGUI()
    {
        // Display the edges.
        OnSceneGUIDrawEdge(m_TopEdge, true);
        OnSceneGUIDrawEdge(m_BottomEdge, false);
    }

    // ------------------------------------------------------------------------
    private void OnSceneGUIDrawEdge(EdgeCollider2D edge, bool bDrawDirections)
    {
        if (edge != null)
        {
            UnityEditor.Handles.color = Color.cyan;

            var points = edge.points;
            Vector2 pointA, pointB;
            for (int i = 0; i < points.Length - 1; ++i)
            {
                pointA = transform.TransformPoint(points[i]);
                pointB = transform.TransformPoint(points[i + 1]);

                UnityEditor.Handles.DrawLine(pointA, pointB);
                //UnityEditor.Handles.Label((Vector3)(pointA + pointB) * 0.5f, i.ToString(), UnityEditor.EditorStyles.miniLabel);

                if (bDrawDirections && i == 0)
                {
                    Vector2 center = (pointA + pointB) * 0.5f;
                    UnityEditor.Handles.color = Color.white;
                    UnityEditor.Handles.DrawLine(center, center + m_Direction.normalized);
                    UnityEditor.Handles.color = Color.black;
                    UnityEditor.Handles.DrawLine(center, center + m_TopToBottomDirection.normalized);
                }
            }
        }
    }
#endif
}

} // namespace Terrain