using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ----------------------------------------------------------------------------
namespace Terrain {

// ----------------------------------------------------------------------------
public class TerrainPlatform : MonoBehaviour
{
    // The TerrainJump creating the platform's bondary.
    public List<TerrainJump> m_TerrainJumps = new List<TerrainJump>();

    // The GameObject currently standing on the platform.
    public List<GameObject> m_StandingOnGOs = new List<GameObject>();

    // The offset applied to the GO anchor when its standing on the TerrainPlatform.
    public float m_Offset;

    // ----------------------------------------------------------------------------
    public bool UpdateCollision(TerrainNavigationInput input, bool bCollidedWithBottomEdge, bool bTransition)
    {
        Collider2D ownerCollider = input.m_Collision.contacts[0].otherCollider;
        bool bOwnerStandingOn = m_StandingOnGOs.Contains(input.m_OwnerGO);
        bool bResult = (bCollidedWithBottomEdge && !bOwnerStandingOn) || (!bCollidedWithBottomEdge && bOwnerStandingOn);

        // Collisions
        foreach (var terrainJump in m_TerrainJumps)
        {
            if (terrainJump == null)
            {
                continue;
            }

            if ((bOwnerStandingOn && !bTransition) || (!bOwnerStandingOn && bTransition))
            {
                Physics2D.IgnoreCollision(terrainJump.m_TopEdge, ownerCollider, false);
                Physics2D.IgnoreCollision(terrainJump.m_BottomEdge, ownerCollider);
            }
            else
            {
                Physics2D.IgnoreCollision(terrainJump.m_TopEdge, ownerCollider);
                Physics2D.IgnoreCollision(terrainJump.m_BottomEdge, ownerCollider, false);
            }
        }

        // StandingOn transition
        if (bTransition)
        {
            if (bOwnerStandingOn)
            {
                m_StandingOnGOs.Remove(input.m_OwnerGO);
            }
            else
            {
                m_StandingOnGOs.Add(input.m_OwnerGO);
            }
        }

        return bResult;
    }
}

} // namespace Terrain