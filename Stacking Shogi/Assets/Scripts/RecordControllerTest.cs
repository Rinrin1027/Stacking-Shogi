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
    
    
    void Start()
    {
        if (textFile != null)
        {
            ReadTextFile();
        }
    }

    
    void Update()
    {
        DisplayCurrentLine();
    }

    void ReadTextFile()
    {
        StringReader reader = new StringReader(textFile.text);
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            lines.Add(line);
        }
    }

    void DisplayCurrentLine()
    {
        Debug.Log(currentIndex);
        if (currentIndex >= 0 && currentIndex < lines.Count)
        {
            displayText.text = lines[currentIndex];
        }
    }

    public void OnLeftButtonClick()
    {
        currentIndex--;
        slider.value--;
    }

    public void OnRightButtonClick()
    {
        currentIndex++;
        slider.value++;
    }
}
