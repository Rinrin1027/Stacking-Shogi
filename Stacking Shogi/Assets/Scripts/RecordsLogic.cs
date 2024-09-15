using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordsLogic : MonoBehaviour
{
    [SerializeField]private Text displayText; //指した手を表示するUI要素
    [SerializeField]private GameObject takenPieceImage; //取った駒を表示するUI要素
    [SerializeField]private ShogiPieceController pieceController; // 駒の配置を行うスクリプト

    [SerializeField]public ShogiBoard shogiBoard; //将棋盤のスクリプト

    private string[,] recordBoard = new string[9,9]; //盤面を保存する配列
    private Dictionary<int, string[,]> collectionBoards = new Dictionary<int, string[,]>(); //盤面を保存する辞書
    private int count = 0; //手数を数える

    // Start is called before the first frame update
    void Start()//String txt
    {
        shogiBoard.GenerateBoard();
        shogiBoard.PlaceInitialPieces();
        
        recordInitialBoard();
        generatedRecord();
        int previousBoardId = 0; //前の盤面を保存
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

    void generatedRecord()
    {
        foreach()
        {
            // recordBoard[,] = null;
            // recordBoard[,] = piece;
            // count++;

            collectionBoards.Add(count, recordBoard);
        }
        // //手数を数える
        // int count = 0;
        // //手数を表示
        // displayText.text = "手数: " + count;
        // //盤面を保存
        // collectionBoards.Add(count, recordBoard);
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
