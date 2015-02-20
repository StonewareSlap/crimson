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

    // Animation properties
    private string m_WalkProperty = "walk";

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

        // @Todo: Scale the animation speed based on the velocity.
        if (m_Controller.Velocity > float.Epsilon)
        {
            m_Animator.speed = 1.5f;
            m_Animator.SetBool(m_WalkProperty, true);
        }
        else
        {
            m_Animator.speed = 1.0f;
            m_Animator.SetBool(m_WalkProperty, false);
        }       
    }
}

} // namespace Sprite