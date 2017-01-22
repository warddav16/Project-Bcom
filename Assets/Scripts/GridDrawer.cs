using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDrawer : MonoBehaviour
{
    public GameGrid Grid;

    GameManager _manager;
    MeshRenderer[] _gridRenderables;
    Mesh _mesh;

    public Material CanReach;
    public Material CannotReach;
    public Material Enemy;
    public Material IAmHere;

    void Awake()
    {
        CreateMesh();
    }

    void Start()
    {
        _manager = GameManager.Instance();
        _gridRenderables = new MeshRenderer[Grid.Width * Grid.Height];

        for( int i = 0, max = Grid.Width * Grid.Height; i < max; ++i)
        {
            GameObject g = new GameObject();
            MeshFilter mf = g.AddComponent<MeshFilter>();
            mf.mesh = _mesh;
            MeshRenderer mr = g.AddComponent<MeshRenderer>();
            mr.material = CannotReach;
#if UNITY_EDITOR
            g.name = "GridRenderable: " + i.ToString();
            g.transform.parent = this.transform;
#endif
            g.transform.forward = Vector3.up;
            g.transform.position = Grid.GetWorldPos(i).UpscaleXZ();
            g.transform.localScale = new Vector3(Grid.Spacing, Grid.Spacing, Grid.Spacing);
            _gridRenderables[i] = mr;
        }
    }

    void CreateMesh()
    {
        _mesh = new Mesh();
        _mesh.name = "GridMesh";
        _mesh.vertices = new Vector3[] 
        {
             new Vector3(-.5f, -.5f, 0.01f),
             new Vector3(.5f, -.5f, 0.01f),
             new Vector3(.5f, .5f, 0.01f),
             new Vector3(-.5f, .5f, 0.01f)
         };
        _mesh.uv = new Vector2[] 
        {
             new Vector2 (0, 0),
             new Vector2 (0, 1),
             new Vector2(1, 1),
             new Vector2 (1, 0)
        };
        _mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        _mesh.RecalculateNormals();
    }

    void Update()
    {
        DrawGrid();
    }

    void DrawGrid()
    {
        GridUnit activeUnit = _manager.GetActiveGridUnit();
        for( int i = 0, max = Grid.Width * Grid.Height; i < max; ++i)
        {
            MeshRenderer mr = _gridRenderables[i];
            if( activeUnit )
            {
                GameObject gridObject = Grid.GetGridObject(i);
                if (gridObject )
                {
                    // Grid is occupied, either its an enemy or ally
                    GridUnit dudeHere = gridObject.GetComponent<GridUnit>();
                    if (activeUnit == dudeHere)
                    {
                        mr.material = IAmHere;
                    }
                    else
                    {
                        mr.material = dudeHere.PlayerAffinity == activeUnit.PlayerAffinity ? CannotReach : Enemy;
                    }
                }
                else
                {
                    if (activeUnit.CanMoveTo(i))
                    {
                        mr.material = CanReach;
                    }
                    else
                    {
                        mr.material = CannotReach;
                    }
                }
            }
            else
            {
                mr.material = CannotReach;
            }
        }
    }
}
