using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
    // 2d array
    public Unit[,] grid;

    private Vector3Int tilemapSize;
    private Tilemap tilemap;

    void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        tilemap.CompressBounds();
        tilemapSize = tilemap.size;
        grid = new Unit[tilemapSize.x, tilemapSize.y];
        Debug.Log($"Grid is ready with size [{tilemapSize.x}, {tilemapSize.y}].");
    }

    // set unit on the grid
    public bool Set(int x, int y, Unit unit)
    {
        if (isOutOfBounds(x, y))
        {
            Debug.Log($"Cannot set unit on location {x}, {y} because the coordinates are out of bounds (max {tilemapSize.x}, {tilemapSize.y}).");
            return false;
        }
        else
        {
            grid[x, y] = unit;
            return true;
        }
    }
    public bool isOutOfBounds(int x, int y)
    {
        return (x >= tilemapSize.x || y >= tilemapSize.y || x < 0 || y < 0);
    }

    public bool Unset(int x, int y)
    {
        if (isOutOfBounds(x, y))
        {
            Debug.Log($"Cannot unset unit on location {x}, {y} because the coordinates are out of bounds (max {tilemapSize.x}, {tilemapSize.y}).");
            return false;
        }
        else
        {
            grid[x, y] = null;
            return true;
        }
    }

    public Unit Get(int x, int y)
    {
        if (isOutOfBounds(x, y))
        {
            Debug.Log($"Cannot get unit at location {x}, {y} because coordinates are out of bounds (max {tilemapSize.x}, {tilemapSize.y}).");
            return null;
        }
        else
        {
            return grid[x, y];
        }
    }
}
