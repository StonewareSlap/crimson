using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Camera {

// ----------------------------------------------------------------------------
public class CameraGameplay : MonoBehaviour
{
    // The list of GOs we want to look at.
    private List<GameObject> m_LookAtGOs = new List<GameObject>();

    // The camera offset from the computed target point.
    public Vector3 m_TargetPositionOffset = new Vector2(0.0f, 0.0f);

    // The SmoothDamp time.
    public float m_SmoothDampTime = 0.5f;

    // The SmoothDamp velocity.
    private Vector3 m_SmoothDampVelocity = Vector3.zero;

    // ----------------------------------------------------------------------------
    private void Start()
    {
        Initialize();
    }

    // ----------------------------------------------------------------------------
    public void Initialize()
    {
        // First, we want to cache the objects we want to look at using the camera.
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            m_LookAtGOs.Add(player);
        }
    }

    // ----------------------------------------------------------------------------
    void Update()
    {
        // For now only do a simple smooth damped the GOs.
        if (m_LookAtGOs.Count > 0)
        {
            Vector3 targetPosition = Vector3.zero;
            foreach (var go in m_LookAtGOs)
            {
                targetPosition += go.transform.TransformPoint(m_TargetPositionOffset);
            }

            targetPosition /= m_LookAtGOs.Count;
            targetPosition.z = -10.0f;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref m_SmoothDampVelocity, m_SmoothDampTime);
        }        
    }
}

} // namespace Camera
