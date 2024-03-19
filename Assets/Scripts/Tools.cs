using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{
    public static TextMesh DisplayText(string text, TextAnchor textAnchor, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.fontSize = fontSize;
        textMesh.color = Color.white;
        textMesh.text = text;
        // textMesh.GetComponent<MeshRenderer>().sortingOrder = .....;
        return textMesh;
    }
}
