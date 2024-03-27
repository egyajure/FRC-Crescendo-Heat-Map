using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEditor;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;
using UnityEngine.Android;


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
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
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
            Debug.Log("path: " + Path.Combine(Application.persistentDataPath, "grid_hits.json"));
            Debug.Log("destination path: " + Path.Combine(Application.persistentDataPath, saveString + "_hits.json"));

            File.Copy(Path.Combine(Application.persistentDataPath, "grid_hits.json"), Path.Combine(Application.persistentDataPath, saveString + "_hits.json"));
            File.Copy(Path.Combine(Application.persistentDataPath, "grid_misses.json"), Path.Combine(Application.persistentDataPath, saveString + "_misses.json"));

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
            File.Delete(Path.Combine(Application.persistentDataPath, "grid_hits.json"));
            File.Delete(Path.Combine(Application.persistentDataPath, "grid_misses.json"));
            // replacing grid files
            File.Copy(Path.Combine(Application.persistentDataPath, name + "_hits.json"), Path.Combine(Application.persistentDataPath, "grid_hits.json"));
            File.Copy(Path.Combine(Application.persistentDataPath, name + "_misses.json"), Path.Combine(Application.persistentDataPath, "grid_misses.json"));
        }
    }

    public void DeleteGrid()
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
            // deleting files
            File.Delete(Path.Combine(Application.persistentDataPath, name + "_hits.json"));
            File.Delete(Path.Combine(Application.persistentDataPath, name + "_misses.json"));
        }
    }



    private bool fileExists(string name)
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, name + "_hits.json")))
        {
            return true;
        }
        return false;
    }

    private List<string> getFileNames()
    {
        List<string> names = new List<string>();
        string filepath = Application.persistentDataPath;
        if (Directory.Exists(filepath))
        {
            // Get all files in the directory
            string[] files = Directory.GetFiles(filepath);

            // Loop through each file
            foreach (string file in files)
            {
                Debug.Log(file);
                if (file.EndsWith("hits.json"))
                {
                    string[] pathComponents = file.Split('/');
                    string extractedSubstring = pathComponents[pathComponents.Length - 1];
                    string name = extractedSubstring.Substring(0, extractedSubstring.Length - 10);
                    names.Add(name);
                }
            }
        }
        return names;
    }

}
