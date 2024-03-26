using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HeatMapScript : MonoBehaviour
{

    [SerializeField] private InputAction position, press;
    [SerializeField] private float swipeResistanceX = 100;
    [SerializeField] private float swipeResistanceY = 200;
    private Vector2 initialPos;
    private Vector2 currentPos => position.ReadValue<Vector2>();
    public LoadGrid grid_manager;

    public GameObject percentage_txt;
    public GameObject hits_txt;
    public GameObject misses_txt;

    private string currMap = "byPercentage";

    private bool isRunning = true;
    // Start is called before the first frame update
    void Start()
    {
        position.Enable();
        press.Enable();
        hits_txt.SetActive(false);
        misses_txt.SetActive(false);
        percentage_txt.SetActive(true);
        grid_manager.LoadHeatMap("byPercentage");
        var hits = GameObject.FindGameObjectsWithTag("Hits");
        var misses = GameObject.FindGameObjectsWithTag("Misses");
        // List<Vector3> hitPositions = new List<Vector3>();
        // List<Vector3> missPositions = new List<Vector3>();
        // foreach (var hit in hits)
        // {
        //     Vector3 position;
        //     Quaternion rotation;
        //     hit.transform.GetPositionAndRotation(out position, out rotation);
        //     hitPositions.Add(position);
        //     hit.SetActive(false);
        // }
        // foreach (var miss in misses)
        // {
        //     Vector3 position;
        //     Quaternion rotation;
        //     miss.transform.GetPositionAndRotation(out position, out rotation);
        //     missPositions.Add(position);
        //     miss.SetActive(false);
        // }
        // create_heat_map(hitPositions, missPositions);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning) return;
        press.performed += _ => { initialPos = currentPos; };
        press.canceled += _ => DetectSwipe();
    }

    private void OnDestroy()
    {
        position.Disable(); // Make sure to disable the input actions when the script is destroyed
        press.Disable();
    }

    private void DetectSwipe()
    {
        Vector2 delta = currentPos - initialPos;
        Vector2 direction = Vector2.zero;

        if (Mathf.Abs(delta.y) > swipeResistanceY)
        {
            //we have swiped if we got here
            direction.y = Mathf.Clamp(delta.y, -1, 1);
            switch_scene(direction);
            isRunning = false;
            return;
        }
        else if (Mathf.Abs(delta.x) > swipeResistanceX)
        {
            direction.x = Mathf.Clamp(delta.x, -1, 1);
            switch_map(direction);
        }
    }

    private void switch_scene(Vector2 direction)
    {
        if (direction.y < 0)
        {
            SceneManager.LoadScene("SwipeScreenScene");
            return;
        }
    }

    private void switch_map(Vector2 direction)
    {
        if (currMap == "byPercentage")
        {
            if (direction.x < 0)
            {
                hits_txt.SetActive(true);
                percentage_txt.SetActive(false);
                grid_manager.LoadHeatMap("byHits");
                currMap = "byHits";
                return;
            }
        }
        else if (currMap == "byHits")
        {

            if (direction.x < 0)
            {
                misses_txt.SetActive(true);
                hits_txt.SetActive(false);
                grid_manager.LoadHeatMap("byMisses");
                currMap = "byMisses";
                return;
            }
            else
            {
                percentage_txt.SetActive(true);
                hits_txt.SetActive(false);
                grid_manager.LoadHeatMap("byPercentage");
                currMap = "byPercentage";
                return;
            }
        }
        else if (currMap == "byMisses")
        {
            if (direction.x > 0)
            {
                hits_txt.SetActive(true);
                misses_txt.SetActive(false);
                grid_manager.LoadHeatMap("byHits");
                currMap = "byHits";
                return;
            }
        }
    }
}
