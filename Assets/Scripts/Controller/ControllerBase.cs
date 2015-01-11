using UnityEngine;
using System.Collections;

namespace Controller {

// ----------------------------------------------------------------------------
public class ControllerBase : MonoBehaviour
{
    // Input (Temporary code to move around)
    public enum EDirection
    {
        None = 0,
        Up = 0x01,
        Down = 0x02,
        Left = 0x04,
        Right = 0x08,
    }
    private int m_InputDirection;
    private Vector2 m_Direction;

    // Properties
    private float m_Speed = 5.0f;
    private float m_Height = 0.0f;

    // Terrain
    public Terrain.TerrainSurface m_DefaultTerrainSurface;
    private SortedList m_TerrainSurfaces = new SortedList();
    private Terrain.TerrainJump m_TerrainJump;
    private Collision2D m_TerrainJumpCollision;
    private Terrain.TerrainNavigationInput m_TerrainNavInput = new Terrain.TerrainNavigationInput();
    private Terrain.TerrainNavigationOutput m_TerrainNavOutput = new Terrain.TerrainNavigationOutput();

    // ------------------------------------------------------------------------   
    private void Start()
    {
        Initialize();
    }

    // ------------------------------------------------------------------------   
    public void Initialize()
    {
        m_TerrainSurfaces.Add(-1, m_DefaultTerrainSurface.gameObject.GetComponent<Terrain.TerrainSurface>());
    }

    // ------------------------------------------------------------------------   
    void Update()
    {
        m_InputDirection = 0;
        if (Input.GetKey(KeyCode.UpArrow)) m_InputDirection |= (int)EDirection.Up;
        if (Input.GetKey(KeyCode.DownArrow)) m_InputDirection |= (int)EDirection.Down;
        if (Input.GetKey(KeyCode.LeftArrow)) m_InputDirection |= (int)EDirection.Left;
        if (Input.GetKey(KeyCode.RightArrow)) m_InputDirection |= (int)EDirection.Right;

        m_Direction = Vector2.zero;
        if ((m_InputDirection & (int)EDirection.Up) == 0) { m_Direction.y -= 1.0f; }
        if ((m_InputDirection & (int)EDirection.Down) == 0) { m_Direction.y += 1.0f; }
        if ((m_InputDirection & (int)EDirection.Left) == 0) { m_Direction.x += 1.0f; }
        if ((m_InputDirection & (int)EDirection.Right) == 0) { m_Direction.x -= 1.0f; }
    }

    // ------------------------------------------------------------------------   
    private void FixedUpdate()
    {
        rigidbody2D.velocity = Vector2.zero;

        m_TerrainNavInput.m_Origin = rigidbody2D.position;
        m_TerrainNavInput.m_Velocity = m_Direction.normalized * m_Speed;
        m_TerrainNavInput.m_Height = m_Height;

        // Terrain Surface
        if (m_TerrainSurfaces.Count > 0)
        {
            // The most recently added walkable surface is the one we consider.
            var terrainSurface = m_TerrainSurfaces.GetByIndex(m_TerrainSurfaces.Count - 1) as Terrain.TerrainSurface;
            if (terrainSurface != null)
            {
                terrainSurface.Navigation(m_TerrainNavInput, m_TerrainNavOutput);
            }
        }
        else
        {
            Debug.LogError("ControllerBase.FixedUpdate : m_TerrainSurfaces is empty. Make sure you have defined the default surface.");
        }

        // Terrain Jump
        if (m_TerrainJump != null)
        {
            m_TerrainNavInput.m_Velocity = m_TerrainNavOutput.m_Velocity;
            m_TerrainNavInput.m_Height = m_TerrainNavOutput.m_Height;
            m_TerrainNavInput.m_Collision = m_TerrainJumpCollision;

            m_TerrainJump.Navigation(m_TerrainNavInput, m_TerrainNavOutput);            
            m_TerrainJump = null;
            m_TerrainJumpCollision = null;
            
            rigidbody2D.position = m_TerrainNavOutput.m_Origin;
        }

        // Height
        m_Height = m_TerrainNavOutput.m_Height - Physics2D.gravity.magnitude * Time.fixedDeltaTime;
        if (m_Height < float.Epsilon)
        {
            m_Height = 0.0f;
        }

        rigidbody2D.velocity = m_TerrainNavOutput.m_Velocity;
    }

    // ------------------------------------------------------------------------   
    void OnTriggerEnter2D(Collider2D other)
    {
        var terrainSurface = other.gameObject.GetComponent<Terrain.TerrainSurface>();
        if (terrainSurface != null)
        {
            m_TerrainSurfaces.Add(Time.frameCount, terrainSurface);
        }
    }

    // ------------------------------------------------------------------------   
    void OnTriggerExit2D(Collider2D other)
    {
        var terrainSurface = other.gameObject.GetComponent<Terrain.TerrainSurface>();
        if (terrainSurface != null)
        {
            int index = m_TerrainSurfaces.IndexOfValue(terrainSurface);
            if (index >= 0)
            {
                m_TerrainSurfaces.RemoveAt(index);
            }
        }        
    }

    // ------------------------------------------------------------------------   
    void OnCollisionEnter2D(Collision2D collision)
    {
        var terrainJump = collision.gameObject.GetComponent<Terrain.TerrainJump>();
        if (terrainJump != null)
        {
            m_TerrainJump = terrainJump;
            m_TerrainJumpCollision = collision;
        }
    }
}

} // namespace Controller