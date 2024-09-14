using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class records_logic : MonoBehaviour
{
    public Text displayText; //指した手を表示するUI要素
    public Image takenPieceImage; //取った駒を表示するUI要素

    private Dictionary<string, int> takenPieceCount = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()//String txt
    {
        createBoard();
        // StreamReader lines = new StreamReader("01.txt"); //txtに置き換える

        // foreach (string line in lines)
        // {
        //     string[] element = line.Split(',');

        //     displayText.text = element[0];
        //     if(element[4] == 1){
        //         string piece = element[5];
        //         if (takenPieceCount.ContainsKey(piece))
        //         {
        //             takenPieceCount[piece]++;
        //         }
        //         else
        //         {
        //             takenPieceCount.Add(piece, 1);
        //             createNewPieceSlot(piece);
        //         }

                
        //     }
        // }
    }

    void createBoard()
    {
        int boardWidth = 9;
        int boardHeight = 9;
        float slotWidth = 50f;  // スロットの幅
        float slotHeight = 50f; // スロットの高さ


        for(int i = 0; i < boardWidth; i++)
        {
            for(int j = 0; j < boardHeight; j++)
            {
                GameObject newSlot = new GameObject();
                newSlot.transform.SetParent(displayText.transform.parent);
                Image image = newSlot.AddComponent<Image>();
                image.sprite = Resources.Load<Sprite>("Images/将棋版面.png");
                RectTransform rectTransform = newSlot.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(slotWidth, slotHeight); // スロットのサイズを指定
                rectTransform.localScale = new Vector3(1, 1, 1);
                rectTransform.localPosition = new Vector3(i * slotWidth, j * slotHeight, 0); // スロットの位置を設定

            // アスペクト比を維持しながらスロットに収める
                image.preserveAspect = true;

            // スロットを有効化
                image.enabled = true;
            }
        }
    }

    void createNewPieceSlot(string piece)
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
