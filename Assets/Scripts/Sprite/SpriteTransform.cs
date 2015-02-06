using UnityEngine;
using System.Collections;

namespace Sprite {

    // ------------------------------------------------------------------------   
    public class SpriteTransform : MonoBehaviour
    {
        // The controller responsible for this sprite movement in the world.
        public Controller.ControllerBase m_Controller;

        private Vector3 m_LocalPosition = new Vector3(0.0f, 0.0f, 0.0f);

        // ------------------------------------------------------------------------   
        private void Update()
        {
            if (m_Controller != null)
            {
                m_LocalPosition.Set(0.0f, m_Controller.m_Height, transform.parent.position.y);
                transform.localPosition = m_LocalPosition;
            }
        }
    }

} // namespace Sprite
