using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoadGrid : MonoBehaviour
{
    public static Grid grid;
    static bool spawned = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Loading Grid");
        grid = new Grid(54, 28, 0.5f, new Vector2(-14, -7));
        DontDestroyOnLoad(transform.gameObject);

        // if (spawned)
        // {
        //     Destroy(transform.gameObject);

        // }
        // else
        // {
        //     spawned = true;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            grid.print();
        }
    }

    public void updateGrid(Vector2 position)
    {
        grid.IncreaseValue(Camera.main.ScreenToWorldPoint(position));
    }
}
