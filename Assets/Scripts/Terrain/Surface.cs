using UnityEngine;
using System.Collections;

namespace Terrain {

// ----------------------------------------------------------------------------
[RequireComponent(typeof(PolygonCollider2D))]
public class Surface : MonoBehaviour
{
    // The surface used to define the terrain's boundarie. For now, we used a polygon collider.
    protected PolygonCollider2D m_Surface;

    // The surface edges metadata.
    public SurfaceEdge[] m_SurfaceEdges;

    // The direction of this terrain. This is used to convert the movement input into the screen space displacement.
    public Vector2 m_HorizontalDirection = new Vector2(1.0f, 0.0f);
    public Vector2 m_VerticalDirection = new Vector2(0.0f, -1.0f);

    // ------------------------------------------------------------------------
    private void Start()
    {
        m_Surface = collider2D as PolygonCollider2D;        
    }
}

} // namespace Terrain
