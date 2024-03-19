using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x, y] = DisplayText(gridArray[x, y].ToString(), TextAnchor.MiddleCenter, null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 5);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void SetValue(int x, int y, int value)
    {
        if (x > 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
            debugTextArray[x, y].color = Color.red;
        }
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        Debug.Log("X input position: " + worldPosition.x);
        Debug.Log("Y input position: " + worldPosition.y);
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        Debug.Log("X result (hoping for an interger between 0 and 54): " + x);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        Debug.Log("Y result (hoping for an interger between 0 and 28): " + y);
    }
    public void SetValue(Vector3 worldPosition, int value)
    {
        Debug.Log("setting value");
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    //I'm going to want to have an increase value function as well



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
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                if (gridArray[x, y] != 0)
                {
                    Debug.Log(gridArray[x, y].ToString());
                }
            }
        }
    }
}
