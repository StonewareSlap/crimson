using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Controller;

namespace Sprite {

// ------------------------------------------------------------------------   
public class SpriteTransform : MonoBehaviour
{
    // The controller responsible for this sprite movement in the world.
    public Controller.ControllerBase m_Controller;

    // Initial sorting order defined in the data. 
    // We use this as an offset applied to the computed sorting order to preserve the data intention.
    private Dictionary<SpriteRenderer, int> m_InitialSortingOrder = new Dictionary<SpriteRenderer,int>();

    // Values used to compute the actual sprite sorting order.
    private float m_MaxY = 100.0f;
    private float m_MinY = -100.0f;        
    private float m_SortingOrderMin = short.MinValue + 200;
    private float m_SortingOrderMax = short.MaxValue - 200;
    private float m_Frequency = 0.1f;

    // Values used for the sprite orientation.
    public float m_OrientationTime = 0.1f;
    private Vector2 m_OrientationVelocity = Vector3.zero;
    private Vector2 m_Orientation = Vector2.zero;
    private Vector2 m_LeftOrientation = Vector2.zero;
    private Vector2 m_RightOrientation = new Vector2(180.0f, 0.0f);

    // Values used for the shadow scale.
    // @Todo: Scale with height...
    public GameObject m_ShadowSprite;

    // ------------------------------------------------------------------------     
    private void Start()
    {
        Initialize();
        StartCoroutine(AutoSortingCoroutine());
    }
    
    // ------------------------------------------------------------------------     
    private void Initialize()
    {
        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            m_InitialSortingOrder.Add(spriteRenderer, spriteRenderer.sortingOrder);
        }
    }

    // ------------------------------------------------------------------------     
    public void Update()
    {
        if (m_Controller != null)
        {
            // Jump position
            transform.localPosition = Vector3.up * m_Controller.m_Height;

            // Orientation
            Vector2 targetOrientation = (m_Controller.m_Orientation == ControllerBase.EOrientation.Left) ? m_LeftOrientation : m_RightOrientation;
            m_Orientation = Vector2.SmoothDamp(m_Orientation, targetOrientation, ref m_OrientationVelocity, m_OrientationTime);
            transform.localRotation = Quaternion.Euler(0.0f, m_Orientation.x, 0.0f);            
        }
    }

    // ------------------------------------------------------------------------     
    private IEnumerator AutoSortingCoroutine()
    {
        while (true)
        {
            AutoSort();
            yield return new WaitForSeconds(m_Frequency);
        }
    }

    // ------------------------------------------------------------------------     
    private void AutoSort()
    {
        int sortingOffset = GetSortingOffset(transform.position.y);
        foreach (SpriteRenderer spriteRenderer in m_InitialSortingOrder.Keys)
        {
            spriteRenderer.sortingOrder = m_InitialSortingOrder[spriteRenderer] + sortingOffset;
        }
    }

    // ------------------------------------------------------------------------     
    private int GetSortingOffset(float currentY)
    {
        float clampedY = Mathf.Clamp(currentY, m_MinY, m_MaxY);
        float t = (clampedY - m_MinY) / (m_MaxY - m_MinY);
        float sortingOffset = (t * (m_SortingOrderMax - m_SortingOrderMin)) + m_SortingOrderMin;        
        return Mathf.RoundToInt(-sortingOffset);
    }
}
    
} // namespace Sprite
