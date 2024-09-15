using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RecordsLogic : MonoBehaviour
{
    public Text displayText; //指した手を表示するUI要素
    public Image takenPieceImage; //取った駒を表示するUI要素
    public ShogiPieceController pieceController; // 駒の配置を行うスクリプト

    // Start is called before the first frame update
    void Start()//String txt
    {
        shougiBoard.GenerateBoard();
        shougiBoard.PlaceInitialPieces();
        StreamReader lines = new StreamReader("01.txt"); //txtに置き換える
        string[] line = lines.ReadLine();
        string[,] recordBoard = new string[9, 9]; //盤面を保存する配列
        recordInitialBoard();
        Dictionary<int, string> collectionBoards = new Dictionary<int, string>(); //各盤面とそのIDを保存する辞書
        int previousBoardId = ; //前の盤面を保存
    }

    void recordInitialBoard()
    {
        for(int i = 0; i < 9; i++)
        {
            recordBoard[i,1] = "歩兵";
            recordBoard[i,6] = "歩兵";
        }
        recordBoard[0,0] = "香車";
        recordBoard[8,0] = "香車";
        recordBoard[1,0] = "桂馬";
        recordBoard[7,0] = "桂馬";
        recordBoard[2,0] = "銀将";
        recordBoard[6,0] = "銀将";
        recordBoard[3,0] = "金将";
        recordBoard[5,0] = "金将";
        recordBoard[4,0] = "玉将";
        recordBoard[7,1] = "飛車";
        recordBoard[1,1] = "角行";
        recordBoard[0,8] = "香車";
        recordBoard[8,8] = "香車";
        recordBoard[1,8] = "桂馬";
        recordBoard[7,8] = "桂馬";
        recordBoard[2,8] = "銀将";
        recordBoard[6,8] = "銀将";
        recordBoard[3,8] = "金将";
        recordBoard[5,8] = "金将";
        recordBoard[4,8] = "玉将";
        recordBoard[1,7] = "飛車";
        recordBoard[7,7] = "角行";

        //初期盤面を保存
        collectionBoards.Add(0, recordBoard);
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
