using UnityEngine;
using System;
using System.Collections;

namespace Terrain {

// ----------------------------------------------------------------------------
[Serializable]
public class SurfaceEdge
{
    public enum EType
    {
        Block, // Stop the character at this edge.
        Pass, // Character can pass through.
        Jump, // Character jump at this edge.
    };

    public EType m_Type;

    // ------------------------------------------------------------------------
    public Color ToColor()
    {
        switch (m_Type)
        {
            case EType.Block: return Color.red;
            case EType.Pass: return Color.green;
            case EType.Jump: return Color.grey;
        }
        return Color.black;
    }
}

} // namespace Edge
