using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static Tools;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    public int[,] gridArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void SetValue(int x, int y, int value)
    {
        if (x > 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
        }
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }
    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public void IncreaseValue(int x, int y)
    {
        if (x > 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] += 1;
        }
    }

    public void IncreaseValue(Vector3 worldPosition)
    {
        // int x, y;
        // GetXY(worldPosition, out x, out y);
        // IncreaseValue(x, y);
        AddValue(worldPosition);
    }

    public void DecreaseValue(Vector3 worldPosition)
    {
        // int x, y;
        // GetXY(worldPosition, out x, out y);
        // IncreaseValue(x, y);
        LowerValue(worldPosition, 10, 1, 3);
    }



    public int GetValue(int x, int y)
    {
        if (x > 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return 0;
        }
    }

    public int GetValue(Vector2 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    public void print()
    {
        Debug.Log("width: " + gridArray.GetLength(0));
        Debug.Log("height: " + gridArray.GetLength(1));
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                if (gridArray[x, y] != 0)
                {
                    Debug.Log(gridArray[x, y].ToString() + "x: " + x + "y: " + y);
                }
            }
        }
    }

    public int GetWidth()
    {
        return gridArray.GetLength(0);
    }

    public int GetHeight()
    {
        return gridArray.GetLength(1);
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public void AddValue(int x, int y, int value)
    {
        SetValue(x, y, GetValue(x, y) + value);
    }
    public void AddValue(Vector3 worldPosition, int value, int fullValueRange, int totalRange)
    {
        int lowerValueAmount = Mathf.RoundToInt(value / (totalRange - fullValueRange));
        GetXY(worldPosition, out int originX, out int originY);
        for (int x = 0; x < totalRange; x++)
        {
            for (int y = 0; y < totalRange - x; y++)
            {
                int radius = x + y;
                int addValue = value;
                if (radius > fullValueRange)
                {
                    addValue -= lowerValueAmount * (radius - fullValueRange);
                }
                AddValue(originX + x, originY + y, addValue);
                if (x != 0)
                {
                    AddValue(originX - x, originY + y, addValue);
                }
                if (y != 0)
                {
                    AddValue(originX + x, originY - y, addValue);
                    if (x != 0)
                    {
                        AddValue(originX - x, originY - y, addValue);
                    }
                }

            }
        }
    }

    public void AddValue(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int originX, out int originY);
        AddValue(originX, originY, 1);
        AddValue(originX + 1, originY, 1);
        AddValue(originX - 1, originY, 1);
        AddValue(originX, originY + 1, 1);
        AddValue(originX, originY - 1, 1);
    }


    public void LowerValue(int x, int y, int value)
    {
        SetValue(x, y, GetValue(x, y) - value);
    }
    public void LowerValue(Vector3 worldPosition, int value, int fullValueRange, int totalRange)
    {
        int lowerValueAmount = Mathf.RoundToInt(value / (totalRange - fullValueRange));
        GetXY(worldPosition, out int originX, out int originY);
        for (int x = 0; x < totalRange; x++)
        {
            for (int y = 0; y < totalRange - x; y++)
            {
                int radius = x + y;
                int lowerValue = value;
                if (radius > fullValueRange)
                {
                    lowerValue -= lowerValueAmount * (radius - fullValueRange);
                }
                LowerValue(originX + x, originY + y, lowerValue);
                if (x != 0)
                {
                    LowerValue(originX - x, originY + y, lowerValue);
                }
                if (y != 0)
                {
                    LowerValue(originX + x, originY - y, lowerValue);
                    if (x != 0)
                    {
                        LowerValue(originX - x, originY - y, lowerValue);
                    }
                }

            }
        }
    }
}
