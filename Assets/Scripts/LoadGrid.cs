using UnityEngine;
using System.IO;

public class IntArrayWrapper
{
    public int[] array;
}

public class LoadGrid : MonoBehaviour
{
    public static Grid hits_grid;
    public static Grid misses_grid;
    [SerializeField] private HeatMapVisual heatMapVisual;

    void Start()
    {
        hits_grid = new Grid(54, 28, 0.5f, new Vector2(-14, -7));
        misses_grid = new Grid(54, 28, 0.5f, new Vector2(-14, -7));
        LoadGridData(hits_grid, true);
        LoadGridData(misses_grid, false);
    }

    public void LoadHeatMap()
    {
        heatMapVisual.SetGrid(hits_grid, misses_grid);
    }

    void OnDestroy()
    {
        SaveGridData(hits_grid, true);
        SaveGridData(misses_grid, false);
    }

    public void updateGrid(Vector2 position, bool hit)
    {
        if (hit == true)
        {
            hits_grid.IncreaseValue(Camera.main.ScreenToWorldPoint(position));
        }
        else
        {
            misses_grid.IncreaseValue(Camera.main.ScreenToWorldPoint(position));
        }

    }

    private void SaveGridData(Grid grid, bool hit)
    {
        int[] temp_grid = two_to_one_dimension_array(grid.gridArray);
        if (hit == true)
        {
            WriteArrayToJsonFile(temp_grid, Application.dataPath + "/grid_hits.json");
        }
        else
        {
            WriteArrayToJsonFile(temp_grid, Application.dataPath + "/grid_misses.json");
        }

    }
    private void LoadGridData(Grid grid, bool hit)
    {
        string filePath;
        if (hit == true)
        {
            filePath = Application.dataPath + "/grid_hits.json";
        }
        else
        {
            filePath = Application.dataPath + "/grid_misses.json";
        }

        if (File.Exists(filePath))
        {
            int[] temp_grid = ReadArrayFromJsonFile(filePath);
            grid.gridArray = one_to_two_dimension_array(temp_grid, 54, 28);
        }
    }

    private int[] two_to_one_dimension_array(int[,] two_dimension)
    {
        int[] one_dimension = new int[two_dimension.GetLength(0) * two_dimension.GetLength(1)];
        // Debug.Log("dimensions: " + two_dimension.GetLength(0) + two_dimension.GetLength(1));
        for (int i = 0; i < two_dimension.GetLength(0); i++)
        {
            // Debug.Log("row number: " + i);
            for (int j = 0; j < two_dimension.GetLength(1); j++)
            {
                // Debug.Log("col number: " + j);
                // Debug.Log("index: " + (two_dimension.GetLength(0) * i + j));
                one_dimension[two_dimension.GetLength(1) * i + j] = two_dimension[i, j];
            }
        }
        return one_dimension;
    }


    private int[,] one_to_two_dimension_array(int[] one_dimension, int numCols, int numRows)
    {
        int[,] two_dimension = new int[numCols, numRows];
        for (int i = 0; i < numCols; i++)
        {
            for (int j = 0; j < numRows; j++)
            {
                int index = i * numRows + j;
                // Debug.Log("index: " + index + " x: " + i + " t: " + j);
                two_dimension[i, j] = one_dimension[index];
            }
        }
        return two_dimension;
    }

    private void print_array(int[] array)
    {
        Debug.Log("test 1d array");
        for (int i = 0; i < array.GetLength(0); i++)
        {
            if (array[i] != 0)
            {
                Debug.Log(i + " " + array[i].ToString());
            }
        }
    }

    private void print_array(int[,] array)
    {
        Debug.Log("test 2d array");
        for (int x = 0; x < array.GetLength(0); x++)
        {
            for (int y = 0; y < array.GetLength(1); y++)
            {
                if (array[x, y] != 0)
                {
                    Debug.Log(array[x, y].ToString() + ", x: " + x + "y: " + y);
                }
            }
        }
    }

    int[] ReadArrayFromJsonFile(string filePath)
    {
        string json = File.ReadAllText(filePath);

        // Deserialize the JSON into IntArrayWrapper
        IntArrayWrapper wrapper = JsonUtility.FromJson<IntArrayWrapper>(json);

        // Return the array from the wrapper
        return wrapper.array;
    }

    void WriteArrayToJsonFile(int[] array, string filePath)
    {
        // Wrap the array inside IntArrayWrapper
        IntArrayWrapper wrapper = new IntArrayWrapper();
        wrapper.array = array;

        string json = JsonUtility.ToJson(wrapper);
        File.WriteAllText(filePath, json);
        // Debug.Log("Array written to JSON file: " + filePath);
    }
}
