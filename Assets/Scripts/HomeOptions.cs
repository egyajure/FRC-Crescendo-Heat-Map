using UnityEngine;
using System.IO;

public class HomeOptions : MonoBehaviour
{
    public void ResetGrid()
    {
        //delete json file
        Debug.Log("deleting grid data");
        File.Delete(Application.dataPath + "/grid.json");
    }
}
