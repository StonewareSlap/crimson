using UnityEngine;
using System.Collections;

namespace Sprite {

    // ------------------------------------------------------------------------   
    public class SpriteTransform : MonoBehaviour
    {
        // The controller responsible for this sprite movement in the world.
        public Controller.ControllerBase m_Controller;

        private Vector3 m_DepthSortingVector = new Vector3(0.0f, 0.0f, 1.0f);

        // ------------------------------------------------------------------------   
        private void Update()
        {
            if (m_Controller != null)
            {
                transform.localPosition = Vector3.up * m_Controller.m_Height + m_DepthSortingVector * transform.parent.position.y;
            }
        }
    }

} // namespace Sprite
