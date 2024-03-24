using UnityEngine;
using System.IO;

public class HomeOptions : MonoBehaviour
{
    public void ResetGrid()
    {
        File.Delete(Application.dataPath + "/grid_hits.json");
        File.Delete(Application.dataPath + "/grid_misses.json");
    }
}
