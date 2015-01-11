using UnityEngine;
using System.Collections;

namespace Terrain {

// ----------------------------------------------------------------------------
public class TerrainJump : MonoBehaviour
{
    // The TerrainEdge used to define the jump highest section. This TerrainEdge can always be crossed
    // on contact.
    public EdgeCollider2D m_TopEdge;

    // The TerrainEdge used to define the jump lowest section. This TerrainEdge can be crossed when
    // the object reaches a data defined height.
    public EdgeCollider2D m_BottomEdge;

    // The direction from the top to bottom edge. It is used as the raycast direction.
    public Vector2 m_TopToBottomDirection;

    // ----------------------------------------------------------------------------
    public void Navigation(TerrainNavigationInput input, TerrainNavigationOutput output)
    {
        // Configure the data depending on which edge we touched.
        var raycastEdge = m_BottomEdge;
        var raycastDirection = m_TopToBottomDirection.normalized;

        if (input.m_Collision.collider == m_BottomEdge)
        {
            raycastEdge = m_TopEdge;
            raycastDirection *= -1.0f;
        }

        // Find where the origin should be moved.
        var contactPoint = input.m_Collision.contacts[0].point;
        var hits = Physics2D.RaycastAll(contactPoint, raycastDirection, 100.0f);
        foreach (var hit in hits)
        {
            if (hit.collider == raycastEdge)
            {
                var heightDelta = Vector2.Dot(hit.point - contactPoint, raycastDirection);

                if (raycastEdge == m_TopEdge)
                {
                    // Make sure we have reached the required height for this case
                    if (input.m_Height < heightDelta)
                    {
                        output.m_Origin = input.m_Origin;
                        output.m_Velocity = input.m_Velocity;
                        output.m_Height = input.m_Height;
                        return;
                    }
                }

                output.m_Origin = hit.point;
                output.m_Velocity = input.m_Velocity;
                output.m_Height = (raycastEdge == m_BottomEdge) ? input.m_Height + heightDelta : input.m_Height - heightDelta;
                return;
            }
        }        
    }
}

} // namespace Terrain