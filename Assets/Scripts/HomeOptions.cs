using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class HomeOptions : MonoBehaviour
{

    [SerializeField] private InputAction position, press;
    [SerializeField] private float swipeResistance = 300;
    private Vector2 initialPos;
    private Vector2 currentPos => position.ReadValue<Vector2>();
    private bool isRunning = true;

    private void Start()
    {
        Debug.Log("started");


        position.Enable();
        press.Enable();
    }
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
        print("detect swipe");
        Vector2 delta = currentPos - initialPos;
        Vector2 direction = Vector2.zero;

        if (Mathf.Abs(delta.y) > swipeResistance)
        {
            //we have swiped if we got here
            direction.y = Mathf.Clamp(delta.y, -1, 1);
            print("swipe y axis");
            switch_scene(direction);
            isRunning = false;
            return;
        }
    }
    private void switch_scene(Vector2 direction)
    {
        if (direction.y > 0)
        {
            print("pull up to swiping screen");
            SceneManager.LoadScene("SwipeScreenScene");
            return;
        }
    }



    public void ResetGrid()
    {
        File.Delete(Application.dataPath + "/grid_hits.json");
        File.Delete(Application.dataPath + "/grid_misses.json");
    }
}
