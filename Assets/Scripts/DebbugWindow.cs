using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebbugWindow : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    // Start is called before the first frame update
    void Start()
    {

        List<string> parts = new List<string>();
        string[] dirs = System.IO.Directory.GetDirectories("../../");
        foreach (string dir in dirs)
        {
            string[] dirs_2 = System.IO.Directory.GetDirectories(dir);

            parts.Add(String.Join("\n-", dirs_2));

        }
        writeText(String.Join("\n", parts));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void writeText(string text)
    {
        textMeshPro.text = text;   
    }
}
