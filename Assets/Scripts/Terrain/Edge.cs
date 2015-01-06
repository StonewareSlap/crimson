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
        Jump, // Character needs to jump at this edge.
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

    // ------------------------------------------------------------------------
    public void Navigation(Navigation.Input input, Navigation.Output output)
    {
        switch (m_Type)
        {
            case EType.Block:
            {
                Block(input, output);
                break;
            }
            case EType.Pass:
            {
                Pass(input, output);
                break;
            }
            case EType.Jump:
            {
                Jump(input, output);
                break;
            }
            default:
            {
                output.m_Velocity = Vector2.zero;
                break;
            }
        }
    }

    private void Block(Navigation.Input input, Navigation.Output output)
    {
        // Wrong calculation...
        output.m_Velocity = input.m_Intersection - input.m_Origin;
        output.m_Height = input.m_Height;
    }

    private void Pass(Navigation.Input input, Navigation.Output output)
    {
        output.m_Velocity = input.m_Velocity;
        output.m_Height = input.m_Height;
    }

    private void Jump(Navigation.Input input, Navigation.Output output)
    {
        // @Example...
        if (input.m_Height > 5.0f)
        {
            Pass(input, output);
        }
        else
        {
            Block(input, output);
        }
    }
}

} // namespace Edge
