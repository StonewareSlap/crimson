using UnityEngine;
using System.Collections;

namespace Terrain {

    // ----------------------------------------------------------------------------
    public class TerrainNavigationInput
    {
        public Vector2 m_Origin = Vector2.zero; // Should it be the anchor so it can be moved by a jump?
        public Vector2 m_Velocity = Vector2.zero;
        public Collision2D m_Collision = null;
        public float m_Height = 0.0f;
    }

} // namespace Terrain.Navigation
