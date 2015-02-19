using UnityEngine;
using System.Collections;

namespace Sprite {

// ------------------------------------------------------------------------   
public class SpriteAnimation : MonoBehaviour
{
    // The controller responsible for this sprite movement in the world.
    public Controller.ControllerBase m_Controller;

    // The animator associated with this sprite.
    public Animator m_Animator;

    // Is the sprite facing left?
    private bool m_bFacingLeft = true;

    // ------------------------------------------------------------------------     
    public void Update()
    {
        if (m_Controller == null)
        {
            return;
        }

        if (m_Animator == null)
        {
            return;
        }

        //@Todo: Scale the animation speed based on the velocity.
        if (m_Controller.CurrentVelocity > float.Epsilon)
        {
            m_Animator.speed = 1.5f;
            m_Animator.SetBool("walk", true);
        }
        else
        {
            m_Animator.speed = 1.0f;
            m_Animator.SetBool("walk", false);
        }

        //@Todo: Check the horizontal velocity and do a smooth rotation. It will look good.
        if (Mathf.Abs(m_Controller.CurrentVelocity) > float.Epsilon)
        {
            bool bMovingLeft = m_Controller.MovingLeft;
            if (bMovingLeft && !m_bFacingLeft)
            {
                transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
                m_bFacingLeft = true;
            }
            else if (!bMovingLeft && m_bFacingLeft)
            {
                transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
                m_bFacingLeft = false;
            }
        }        
    }
}

} // namespace Sprite