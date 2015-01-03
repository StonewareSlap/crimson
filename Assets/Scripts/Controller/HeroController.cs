﻿using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour {

    public float m_speed = 2.0f;

    public enum EDirection
    {
        None = 0,
        Up = 0x01,
        Down = 0x02,
        Left = 0x04,
        Right = 0x08,
    }
    private int m_inputDirection;
    public  Vector2 m_direction;
    private Vector2 m_nextPosition;

    private SortedList m_walkableSurfaces = new SortedList();

	void Start() 
    {	        
	}
	
	void Update() 
    {
        m_inputDirection = 0;
        if (Input.GetKey(KeyCode.UpArrow)) m_inputDirection |= (int)EDirection.Up;
        if (Input.GetKey(KeyCode.DownArrow)) m_inputDirection |= (int)EDirection.Down;
        if (Input.GetKey(KeyCode.LeftArrow)) m_inputDirection |= (int)EDirection.Left;
        if (Input.GetKey(KeyCode.RightArrow)) m_inputDirection |= (int)EDirection.Right;

        m_direction = Vector2.zero;
        if ((m_inputDirection & (int)EDirection.Up) == 0) { m_direction.y -= 1.0f; }
        if ((m_inputDirection & (int)EDirection.Down) == 0) { m_direction.y += 1.0f; }
        if ((m_inputDirection & (int)EDirection.Left) == 0) { m_direction.x += 1.0f; }
        if ((m_inputDirection & (int)EDirection.Right) == 0) { m_direction.x -= 1.0f; }      
	}

    void FixedUpdate()
    {
        rigidbody2D.velocity = Vector2.zero;

        if (m_walkableSurfaces.Count > 0)
        {
            // The most recently added walkable surface is the one we consider.
            GameObject walkableSurfaceObject = m_walkableSurfaces.GetByIndex(m_walkableSurfaces.Count - 1) as GameObject;
            var walkableSurface = walkableSurfaceObject != null ? walkableSurfaceObject.GetComponent<Terrain.Surface>() : null;
            if (walkableSurface)
            {
                //Vector2 velocity = walkableSurface.ComputeVelocity(this);
                //rigidbody2D.velocity = velocity;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        m_walkableSurfaces.Add(Time.frameCount, other.gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        int index = m_walkableSurfaces.IndexOfValue(other.gameObject);
        if (index >= 0)
        {
            m_walkableSurfaces.RemoveAt(index);
        }
    }
}