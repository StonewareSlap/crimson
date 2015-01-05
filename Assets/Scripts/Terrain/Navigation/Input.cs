using UnityEngine;
using System.Collections;

namespace Terrain.Navigation {

    // ----------------------------------------------------------------------------
    public class Input
    {
        public Vector2 m_Origin = Vector2.zero; // Should it be the anchor so it can be moved by a jump?
        public Vector2 m_Velocity = Vector2.zero;
        public Vector2 m_Intersection = Vector2.zero;
        public float m_Height = 0.0f;
    }

} // namespace Terrain.Navigation
