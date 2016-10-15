using UnityEngine;
using System.Collections;
using System;

public class GameGrid : MonoBehaviour
{
    [Flags]
    enum GridFlags
    {
        Occupied,
        NotAvailable
    }
    class GridData
    {
        public GridFlags[] gridFlags;
    }

    public int Width = 10;
    public int Height = 10;
    public float Spacing = .5f;

    [SerializeField]
    private GridData _gridData = new GridData();

    GameGrid()
    {
        _gridData.gridFlags = new GridFlags[Width * Height];
    }

    void OnValidate()
    {
        Width = Mathf.Max(Width, 1);
        Height = Mathf.Max(Height, 1);
        GridData data = new GridData();
        data.gridFlags = new GridFlags[Width * Height];
        int min = Mathf.Min(Width * Height, this._gridData.gridFlags.Length);
        for (int i = 0; i < min; ++i)
        {
            data.gridFlags[i] = this._gridData.gridFlags[i];
        }
        this._gridData = data;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        float lineDrawer = 0.0f;
        float widthEnd = Spacing * Width;
        float heightEnd = Spacing * Height;
        for (int h = 0; h <= Height; ++h)
        {
            Gizmos.DrawLine(new Vector3(0, 0, lineDrawer) + transform.position, new Vector3(widthEnd, 0, lineDrawer) + transform.position);
            lineDrawer += Spacing;
        }
        lineDrawer = 0.0f;
        for (int w = 0; w <= Width; ++w)
        {
            Gizmos.DrawLine(new Vector3(lineDrawer, 0, 0) + transform.position, new Vector3(lineDrawer, 0, heightEnd) + transform.position);
            lineDrawer += Spacing;
        }
    }

    // -1 No hit
    public int GetGridCoordFromWorldPos( Vector2 xzPos )
    {
        Vector2 gridDim = new Vector2(Width, Height) * Spacing;
        Vector2 basePoint = transform.position.ToVec2XZ();
        Vector2 min = basePoint;
        Vector2 max = basePoint + gridDim;
        if (xzPos.x > min.x && xzPos.x < max.x && xzPos.y > min.y && xzPos.y < max.y)
        {
            int h = (int)Mathf.Floor(xzPos.y / Spacing);
            int w = (int)Mathf.Floor(xzPos.x / Spacing);
            return h * Width + w;
        }

        return -1;
    }

    public Vector2 GetWorldPos(int gridLocation)
    {
        int width = gridLocation % Height;
        int height = (gridLocation - width) / Height;
        Vector2 offset = transform.position;
        return offset + new Vector2(width, height) * Spacing;
    }

    public void ToggleIndexFromGrid(int idx)
    {
        _gridData.gridFlags[idx] ^= GridFlags.NotAvailable;
    }

    public bool IsIndexAvailable(int idx)
    {
        return ( _gridData.gridFlags[idx] & GridFlags.NotAvailable ) != 0;
    }
}
