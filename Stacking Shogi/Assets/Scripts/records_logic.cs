using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class records_logic : MonoBehaviour
{
    public Text displayText; //指した手を表示するUI要素
    public Image takenPieceImage; //取った駒を表示するUI要素
    public GameObject tilePrefab; // タイル用のプレハブ
    public float tileSize = 1.0f; // タイルのサイズ
    public ShogiPieceController pieceController; // 駒の配置を行うスクリプト

    private Dictionary<string, int> takenPieceCount = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()//String txt
    {
        createBoard();
        setPieces();
        // StreamReader lines = new StreamReader("01.txt"); //txtに置き換える
        int count = 0;

        // foreach (string line in lines)
        // {
        //     string[] element = line.Split(',');
        //     count++;  //手数をカウント
        //指した手を表示
        //     displayText.text = element[0];
        //駒移動
        //     pieceMove(element[1].Parse(),elment[2].Parse(),element[3]);
        //     if(element[4] == 1){
        //         string takenpiece = element[5];
        //         if (takenPieceCount.ContainsKey(takenpiece))
        //         {
        //             takenPieceCount[takenpiece]++;
        //         }
        //         else
        //         {
        //             takenPieceCount.Add(takenpiece, 1);
        //             createNewPieceSlot(takenpiece);
        //         }

                
        //     }
        // }
    }

    void createBoard()
    {
        int boardWidth = 9;
        int boardHeight = 9;
        int boardSize = 9;

        for (int y = 0; y < boardHeight; y++)
        {
            for (int x = 0; x < boardWidth; x++)
            {
                // オフセットを適用して、中心を(0, 0)に調整
                GameObject tile = Instantiate(tilePrefab);
                tile.transform.position = new Vector3(x * boardSize, y * boardSize, 0);
                tile.transform.SetParent(transform);
            }
        }
    }

    void setPieces()
    {
        // // 味方側の駒の配置
        // for (int x = 0; x < 9; x++)
        // {
        //     pieceController.PlacePiece(x, 2, "歩"); // 2段目に歩を配置
        // }
        // pieceController.PlacePiece(0, 0, "香車");
        // pieceController.PlacePiece(8, 0, "香車");
        // pieceController.PlacePiece(1, 0, "桂馬");
        // pieceController.PlacePiece(7, 0, "桂馬");
        // pieceController.PlacePiece(2, 0, "銀");
        // pieceController.PlacePiece(6, 0, "銀");
        // pieceController.PlacePiece(3, 0, "金");
        // pieceController.PlacePiece(5, 0, "金");
        // pieceController.PlacePiece(4, 0, "王");
        // pieceController.PlacePiece(1, 1, "飛車");
        // pieceController.PlacePiece(7, 1, "角行");

        // // 敵側の駒の配置
        // for (int x = 0; x < 9; x++)
        // {
        //     pieceController.PlacePiece(x, 6, "歩", true); // 6段目に歩を配置
        // }
        // pieceController.PlacePiece(0, 8, "香車", true);
        // pieceController.PlacePiece(8, 8, "香車", true);
        // pieceController.PlacePiece(1, 8, "桂馬", true);
        // pieceController.PlacePiece(7, 8, "桂馬", true);
        // pieceController.PlacePiece(2, 8, "銀", true);
        // pieceController.PlacePiece(6, 8, "銀", true);
        // pieceController.PlacePiece(3, 8, "金", true);
        // pieceController.PlacePiece(5, 8, "金", true);
        // pieceController.PlacePiece(4, 8, "王", true);
        // pieceController.PlacePiece(1, 7, "飛車", true);
        // pieceController.PlacePiece(7, 7, "角行", true);
    }

    void pieceMove(int startpoint, int endpoint, string piece)
    {
        if (element[1] == 0)
        {
            takenPieceCount[piece]--;
            if (takenPieceCount[piece] == 0)
            {
                takenPieceCount.Remove(piece);
                //持ち駒削除
            }
        }
        else
        {
            removePiece(startpoint);
            pieceController.PlacePiece(endpoint / 10, endpoint % 10, piece);
        }
    
    }

    void removePiece(int position)
    {
    // ShogiPieceControllerから指定された位置の駒を取得
        GameObject piece = pieceController.GetPieceAtPosition(position/10, position%10);

        if (piece != null)
        {
            // 駒を盤上から削除
            Destroy(piece);
            Debug.Log($"位置 {position} にある駒を削除しました。");
        }
        else
        {
            Debug.LogError($"位置 {position} に駒が存在しません。");
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