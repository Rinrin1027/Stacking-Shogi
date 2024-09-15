using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordControllerTest : MonoBehaviour
{
    [SerializeField] private TextAsset textFile; // テキストファイル
    [SerializeField] private TextMeshProUGUI displayText; // 表示するText UI
    [SerializeField] private Slider slider;
    [SerializeField] private int currentIndex;
    private List<string> lines = new List<string>();


    // void Start()
    // {
    //     if (textFile != null)
    //     {
    //         ReadTextFile();
    //     }
    // }


    // void Update()
    // {
    //     DisplayCurrentLine();
    // }

    public void ReadTextFile()
    {
        if (textFile != null)
        {
            StringReader reader = new StringReader(textFile.text);
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                lines.Add(line);
            }
        }
    }


    public void DisplayCurrentLine()
    {
        Debug.Log(currentIndex);
        if (currentIndex >= 0 && currentIndex < lines.Count)
        {
            String[] element = lines[currentIndex].Split(",");
            displayText.text = element[0];
        }
    }

    public void OnLeftButtonClick()
    {
        currentIndex--;
        slider.value--;
        Debug.Log("左が押された!");
    }

    public void OnRightButtonClick()
    {
        currentIndex++;
        slider.value++;
        Debug.Log("右が押された!");
    }
}
