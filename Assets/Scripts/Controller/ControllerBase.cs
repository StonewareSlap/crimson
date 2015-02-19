using UnityEngine;
using System.Collections;

namespace Controller {

// ----------------------------------------------------------------------------
public class ControllerBase : MonoBehaviour
{
    private Vector2 m_Direction;
    private Vector2 m_DirectionFactor = new Vector2(1.0f, 0.75f);
    private bool m_bJumpRequest = false;
    
    // Properties
    public float m_MaxVelocity = 4.5f;
    public float m_GravityFactor = 3.0f;
   
    private float m_VerticalVelocity = 0.0f;
    public float m_Height = 0.0f;

    // Collision
    public Rigidbody2D m_RigidBody;
    public CircleCollider2D m_NavigationCollider;

    // Terrain
    public Terrain.TerrainSurface m_DefaultTerrainSurface;
    private SortedList m_TerrainSurfaces = new SortedList();
    private Terrain.TerrainJump m_TerrainJump;
    private Collision2D m_TerrainJumpCollision;

    // Terrain Navigation
    private Terrain.TerrainNavigationInput m_TerrainNavInput = new Terrain.TerrainNavigationInput();
    private Terrain.TerrainNavigationOutput m_TerrainNavOutput = new Terrain.TerrainNavigationOutput();

    // ------------------------------------------------------------------------   
    public float CurrentVelocity
    {
        get
        {
            return m_RigidBody.velocity.magnitude;
        }
    }

    public bool MovingLeft
    {
        get
        {
            return m_RigidBody.velocity.x < float.Epsilon;
        }
    }

    // ------------------------------------------------------------------------   
    private void Start()
    {
        Initialize();
    }

    // ------------------------------------------------------------------------   
    public void Initialize()
    {
        // Add the default TerrainSurface. This surface is used when the controller is not standing on another one.
        m_DefaultTerrainSurface.Initialize();
        m_TerrainSurfaces.Add(-1, m_DefaultTerrainSurface.gameObject.GetComponent<Terrain.TerrainSurface>());

        m_TerrainNavInput.m_OwnerGO = gameObject;
    }

    // ------------------------------------------------------------------------   
    void Update()
    {
        // Direction request
        m_Direction.x = Input.GetAxis("Horizontal") * m_DirectionFactor.x;
        m_Direction.y = Input.GetAxis("Vertical") * m_DirectionFactor.y;

        // Jump request
        m_bJumpRequest = Input.GetButton("Jump");
        if (m_bJumpRequest && m_Height < float.Epsilon)
        {
            m_VerticalVelocity = 12.5f;
        }

        // Height update
        m_Height += m_VerticalVelocity * Time.deltaTime;
        if (m_Height < float.Epsilon)
        {
            m_Height = m_VerticalVelocity = 0.0f;
        }
    }

    // ------------------------------------------------------------------------   
    private void FixedUpdate()
    {
        UpdateInterpolationMode();

        // Initialize the navigation structs.
        m_TerrainNavInput.m_Origin = m_TerrainNavOutput.m_Origin = m_RigidBody.position;
        m_TerrainNavInput.m_Velocity = m_TerrainNavOutput.m_Velocity = m_Direction * m_MaxVelocity;
        m_TerrainNavInput.m_Height = m_TerrainNavOutput.m_Height = m_Height;
        m_TerrainNavInput.m_Radius = m_NavigationCollider.radius;

        // Terrain Surface
        if (m_TerrainSurfaces.Count > 0)
        {
            // We consider the most recently added TerrainSurface.
            var terrainSurface = m_TerrainSurfaces.GetByIndex(m_TerrainSurfaces.Count - 1) as Terrain.TerrainSurface;
            if (terrainSurface != null)
            {
                terrainSurface.Navigation(m_TerrainNavInput, m_TerrainNavOutput);
            }
        }
        else
        {
            Debug.LogError("ControllerBase.FixedUpdate : m_TerrainSurfaces is empty. Make sure you have assigned m_DefaultTerrainSurface.");
        }

        // Terrain Jump
        if (m_TerrainJump != null)
        {
            m_TerrainNavInput.m_Velocity = m_TerrainNavOutput.m_Velocity;
            m_TerrainNavInput.m_Height = m_TerrainNavOutput.m_Height;
            m_TerrainNavInput.m_Collision = m_TerrainJumpCollision;

            m_TerrainJump.Navigation(m_TerrainNavInput, m_TerrainNavOutput);

            // Release the references since the TerrainJump was handled.          
            m_TerrainJump = null;
            m_TerrainJumpCollision = null;
            m_TerrainNavInput.m_Collision = null;
        }

        // Gravity
        m_VerticalVelocity -= Physics2D.gravity.magnitude * m_GravityFactor * Time.fixedDeltaTime;

        // Final Values
        m_RigidBody.position = m_TerrainNavOutput.m_Origin;
        m_RigidBody.velocity = m_TerrainNavOutput.m_Velocity;
        m_Height = m_TerrainNavOutput.m_Height;
    }

    // ------------------------------------------------------------------------   
    void UpdateInterpolationMode()
    {
        if (m_RigidBody)
        {
            // When we are jumping, we disable the rigid body interpolation since we will teleport it.
            // We might do this for other cases in the future. In fact, we have to do this every time we "teleport" the rigid body.
            m_RigidBody.interpolation = m_TerrainJump ? RigidbodyInterpolation2D.None : RigidbodyInterpolation2D.Interpolate;
        }
    }

    // ------------------------------------------------------------------------   
    void CheckForTerrainSurface(Collider2D other, bool bAdd)
    {
        var terrainSurface = other.gameObject.GetComponent<Terrain.TerrainSurface>();
        if (terrainSurface != null)
        {
            if (bAdd)
            {
                m_TerrainSurfaces.Add(Time.frameCount, terrainSurface);          
            }
            else
            {
                var index = m_TerrainSurfaces.IndexOfValue(terrainSurface);
                if (index >= 0)
                {
                    m_TerrainSurfaces.RemoveAt(index);
                }
            }
        }
    }

    // ------------------------------------------------------------------------   
    void CheckForTerrainJump(Collision2D collision)
    {
        var terrainJump = collision.gameObject.GetComponent<Terrain.TerrainJump>();
        if (terrainJump != null)
        {
            m_TerrainJump = terrainJump;
            m_TerrainJumpCollision = collision;
        }
    }

    // ------------------------------------------------------------------------   
    void OnTriggerEnter2D(Collider2D other)
    {
        CheckForTerrainSurface(other, true);
    }

    // ------------------------------------------------------------------------   
    void OnTriggerExit2D(Collider2D other)
    {
        CheckForTerrainSurface(other, false);
    }

    // ------------------------------------------------------------------------   
    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckForTerrainJump(collision);
    }

    // ------------------------------------------------------------------------   
    void OnCollisionStay2D(Collision2D collision)
    {
        CheckForTerrainJump(collision);
    }
}

} // namespace Controller