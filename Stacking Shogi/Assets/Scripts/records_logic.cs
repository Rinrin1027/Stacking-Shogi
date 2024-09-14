using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class records_logic : MonoBehaviour
{
    public Text displayText; //指した手を表示するUI要素
    public Image takenPieceImage; //取った駒を表示するUI要素

    private Dictionary<string, int> takenPieceCount = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start(string txt)
    {
        createBoard();
        StreamReader lines = new StreamReader("01.txt"); //txtに置き換える

        foreach (string line in lines)
        {
            string[] element = line.Split(',');

            displayText.text = element[0];
            if(element[4] == 1){
                string piece = element[5];
                if (takenPieceCount.ContainsKey(piece))
                {
                    takenPieceCount[piece]++;
                }
                else
                {
                    takenPieceCount.Add(piece, 1);
                    createNewPieceSlot(piece);
                }

                
            }
        }
    }

    void createNewPieceSlot(string piece)
    {
        GameObject newSlot = new GameObject();
        newSlot.transform.SetParent(takenPieceImage.transform.parent);
        newSlot.AddComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + piece + ".png");
        newSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        newSlot.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        newSlot.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        newSlot.GetComponent<Image>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
