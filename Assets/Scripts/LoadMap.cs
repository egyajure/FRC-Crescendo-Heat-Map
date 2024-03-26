using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEditor;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;


public class LoadMap : MonoBehaviour
{
    private List<string> FileNames = new List<string>();
    public static string saveString;

    public TMP_InputField saveStringField;
    public TMP_Dropdown loadStringField;
    public GameObject error, wrong_char_error;
    public GameObject homescreen;
    public GameObject savescreen;

    public List<char> invalidChars;

    // Start is called before the first frame update
    void Start()
    {
        saveString = "";
        FileNames = getFileNames();
        loadStringField.AddOptions(FileNames);
        error.SetActive(false);
        wrong_char_error.SetActive(false);


        invalidChars = new List<char>();

        // Control characters (U+0000 through U+001F)
        for (int charCode = 0x00; charCode < 0x20; charCode++)
        {
            invalidChars.Add((char)charCode);
        }

        // Control characters (U+007F through U+009F)
        for (int charCode = 0x7F; charCode < 0xA0; charCode++)
        {
            invalidChars.Add((char)charCode);
        }

        // Additional invalid characters: quote, backslash
        invalidChars.AddRange(new char[] { '"', '\\' });
    }

    public void Copy()
    {
        saveString = saveStringField.text;
        Debug.Log("saveString field" + saveString);
    }

    public void SaveGrid()
    {
        Debug.Log("saving grid");
        if (saveString == "")
        {
            return;
        }
        bool errors = false;
        foreach (char invalidChar in invalidChars)
        {
            if (saveString.Contains(invalidChar))
            {
                Debug.Log("bad character found");
                error.SetActive(false);
                wrong_char_error.SetActive(true);
                errors = true;
            }
        }
        if (fileExists(saveString))
        {
            //print this name has already been used
            wrong_char_error.SetActive(false);
            error.SetActive(true);
            Debug.Log("enter a different name");
        }
        else if (!errors)
        {
            Debug.Log("grid saved");
            FileNames.Add(saveString);
            File.Copy(Application.dataPath + "/grid_hits.json", Application.dataPath + "/savedGrids/" + saveString + "_hits.json");
            File.Copy(Application.dataPath + "/grid_misses.json", Application.dataPath + "/savedGrids/" + saveString + "_misses.json");

            // switch back to homescreen
            savescreen.SetActive(false);
            homescreen.SetActive(true);
        }
    }

    public void LoadGrid()
    {
        int dropdownIndex = loadStringField.value;
        string name = loadStringField.options[dropdownIndex].text;
        Debug.Log("test name is " + name);
        if (!fileExists(name))
        {
            Debug.Log("the file you are looking for does not exist");
        }
        else
        {
            // deleting original files
            File.Delete(Application.dataPath + "/grid_hits.json");
            File.Delete(Application.dataPath + "/grid_misses.json");
            // replacing grid files
            File.Copy(Application.dataPath + "/savedGrids/" + name + "_hits.json", Application.dataPath + "/grid_hits.json");
            File.Copy(Application.dataPath + "/savedGrids/" + name + "_misses.json", Application.dataPath + "/grid_misses.json");
        }
    }

    private bool fileExists(string name)
    {
        if (File.Exists(Application.dataPath + "/savedGrids/" + name + "_hits.json"))
        {
            return true;
        }
        return false;
    }

    private List<string> getFileNames()
    {
        List<string> names = new List<string>();
        string filepath = Application.dataPath + "/savedGrids";
        if (Directory.Exists(filepath))
        {
            // Get all files in the directory
            string[] files = Directory.GetFiles(filepath);

            // Loop through each file
            foreach (string file in files)
            {
                if (file.EndsWith("meta") || file.EndsWith("misses.json"))
                {
                    continue;
                }
                string[] pathComponents = file.Split('/');
                string extractedSubstring = pathComponents[pathComponents.Length - 1];
                string name = extractedSubstring.Substring(0, extractedSubstring.Length - 10);
                names.Add(name);
            }
        }
        return names;
    }

}
