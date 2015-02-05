using UnityEngine;
using System.Collections;
using Controller;

namespace Terrain {

    // ----------------------------------------------------------------------------
    public class TerrainNavigationInput
    {
        public GameObject m_OwnerGO = null;
        public Vector2 m_Origin = Vector2.zero;
        public Vector2 m_Velocity = Vector2.zero;
        public Collision2D m_Collision = null;
        public float m_Height = 0.0f;
        public float m_Radius = 0.0f;
    }

} // namespace Terrain.Navigation
