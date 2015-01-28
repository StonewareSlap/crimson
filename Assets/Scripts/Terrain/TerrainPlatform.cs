using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ----------------------------------------------------------------------------
namespace Terrain {

// ----------------------------------------------------------------------------
public class TerrainPlatform : MonoBehaviour
{
    // The TerrainJump creating the platform's bondary.
    public List<TerrainJump> m_TerrainJumps = new List<TerrainJump>();

    // The GameObject currently standing on the platform.
    public List<GameObject> m_ContainedGOs = new List<GameObject>();
}

} // namespace Terrain