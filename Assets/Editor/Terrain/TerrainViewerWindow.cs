using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Terrain
{

// ----------------------------------------------------------------------------
public class TerrainViewerWindow : EditorWindow
{
    private static string m_TerrainEdgeTag = "TerrainEdge";
    private static string m_TerrainSurfaceTag = "TerrainSurface";
    private static string m_TerrainJumpTag = "TerrainJump";

    // ----------------------------------------------------------------------------
    [MenuItem("Crimson/Terrain Viewer")]
    public static void Init()
    {
        TerrainViewerWindow window = GetWindow<TerrainViewerWindow>();
        window.title = "Terrain Viewer";
    }

    // ----------------------------------------------------------------------------
    private static void OnScene(SceneView sceneview)
    {
        var terrainEdges = GameObject.FindGameObjectsWithTag(m_TerrainEdgeTag);
        foreach (var go in terrainEdges)
        {
            var edge = go.GetComponent<TerrainEdge>();
            if (edge != null) edge.OnSceneGUI();
        }

        var terrainSurfaces = GameObject.FindGameObjectsWithTag(m_TerrainSurfaceTag);
        foreach (var go in terrainSurfaces)
        {
            var surface = go.GetComponent<TerrainSurface>();
            if (surface != null) surface.OnSceneGUI();
        }
        
        var terrainJumps = GameObject.FindGameObjectsWithTag(m_TerrainJumpTag);
        foreach (var go in terrainJumps)
        {
            var jump = go.GetComponent<TerrainJump>();
            if (jump != null) jump.OnSceneGUI(); 
        }
    }

    // ----------------------------------------------------------------------------
    public void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnScene;
    }

    // ----------------------------------------------------------------------------
    public void Update()
    {
        if (SceneView.onSceneGUIDelegate == null)
        {
            SceneView.onSceneGUIDelegate += OnScene;
        }
    }
}

} // namespace Terrain