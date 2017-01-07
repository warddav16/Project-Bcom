using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GameGrid))]
public class GridEditor : Editor
{
    void OnSceneGUI()
    {
        // Validate grid
        GameGrid grid = target as GameGrid;
        if (!grid)
            return;

        // Raycast against grid to see where we are
        int layerMask = (1 << LayerMask.GetMask("Grid"));
        layerMask = ~layerMask;
        RaycastHit hit;
        Vector3 innerGridOffset = new Vector3(grid.Spacing / 2, 0, grid.Spacing / 2);
        var mousePosInScene = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        int hitIdx = -1;
        if (Physics.Raycast(mousePosInScene, out hit, 1000.0f, layerMask))
        {
            hitIdx = grid.GetGridCoordFromWorldPos(hit.point.ToVec2XZ());
            Debug.Log(Event.current.type);
            if(Event.current.type == EventType.MouseDown && Event.current.button == 0 )
            {
                grid.ToggleIndexFromGrid(hitIdx);
            }
        }

        // Draw grid
        for( int h = 0; h < grid.Height; ++h )
        {
            for (int w = 0; w < grid.Width; ++w )
            {
                Vector3 drawPoint = grid.transform.position + new Vector3(w * grid.Spacing, 0, h * grid.Spacing) + innerGridOffset;
                Color cubeColor = Color.green;

                int idx = h * grid.Width + w;
                if( grid.IsIndexAvailable(idx) )
                {
                    cubeColor = Color.red;
                }
                cubeColor.a = .5f;
                Handles.color = cubeColor;
                DrawQuad(drawPoint, grid.Spacing, cubeColor);
            }
        }
    }

    void DrawQuad( Vector3 pos, float scale, Color col)
    {
        float halfScale = scale / 2.0f;
        Vector3[] verts = {
            new Vector3( pos.x - halfScale, pos.y, pos.z - halfScale ),
            new Vector3( pos.x - halfScale, pos.y, pos.z + halfScale ),
            new Vector3( pos.x + halfScale, pos.y, pos.z + halfScale ),
            new Vector3( pos.x + halfScale, pos.y, pos.z - halfScale )
        };

        Handles.DrawSolidRectangleWithOutline( verts, col, col);
    }
}
