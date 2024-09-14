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
        // 味方側の駒の配置
        for (int x = 0; x < 9; x++)
        {
            pieceController.PlacePiece(x, 2, "歩"); // 2段目に歩を配置
        }
        pieceController.PlacePiece(0, 0, "香車");
        pieceController.PlacePiece(8, 0, "香車");
        pieceController.PlacePiece(1, 0, "桂馬");
        pieceController.PlacePiece(7, 0, "桂馬");
        pieceController.PlacePiece(2, 0, "銀");
        pieceController.PlacePiece(6, 0, "銀");
        pieceController.PlacePiece(3, 0, "金");
        pieceController.PlacePiece(5, 0, "金");
        pieceController.PlacePiece(4, 0, "王");
        pieceController.PlacePiece(1, 1, "飛車");
        pieceController.PlacePiece(7, 1, "角行");

        // 敵側の駒の配置
        for (int x = 0; x < 9; x++)
        {
            pieceController.PlacePiece(x, 6, "歩", true); // 6段目に歩を配置
        }
        pieceController.PlacePiece(0, 8, "香車", true);
        pieceController.PlacePiece(8, 8, "香車", true);
        pieceController.PlacePiece(1, 8, "桂馬", true);
        pieceController.PlacePiece(7, 8, "桂馬", true);
        pieceController.PlacePiece(2, 8, "銀", true);
        pieceController.PlacePiece(6, 8, "銀", true);
        pieceController.PlacePiece(3, 8, "金", true);
        pieceController.PlacePiece(5, 8, "金", true);
        pieceController.PlacePiece(4, 8, "王", true);
        pieceController.PlacePiece(1, 7, "飛車", true);
        pieceController.PlacePiece(7, 7, "角行", true);
    }

    void createNewPieceSlot(string piece)
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
