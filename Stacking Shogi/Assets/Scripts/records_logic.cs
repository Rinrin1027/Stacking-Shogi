using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class records_logic : MonoBehaviour
{
    public Text displayText; //指した手を表示するUI要素
    public Image takenPieceImage; //取った駒を表示するUI要素
    public ShogiPieceController pieceController; // 駒の配置を行うスクリプト

    int count = 0;

    private Dictionary<string, int> takenPieceCount = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()//String txt
    {
        shougiBoard.GenerateBoard();
        shougiBoard.PlaceInitialPieces();
        StreamReader lines = new StreamReader("01.txt"); //txtに置き換える
        string[] line = lines.Split(',');
        string[,] recordBoard = new string[9, 9];
        Dictionary<int, string> collectionBoards = new Dictionary<int, string>();
        int previousBoardId = ; //前の盤面を保存
    }

    // Update is called once per frame
    void Update()
    {
        // if() //ボタン押したら次の手に進む
        // {
        // }else if() //ボタン押したら前の手に戻る
        // { 
        // }
    }
}
