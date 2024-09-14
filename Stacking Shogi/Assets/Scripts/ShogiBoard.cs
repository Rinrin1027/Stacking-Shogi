using UnityEngine;

public class ShogiBoard : MonoBehaviour
{
    public GameObject cellPrefab;  // マス目のPrefab
    public int rows = 9;  // 縦のマス数
    public int cols = 9;  // 横のマス数
    public float cellSize = 1.0f;  // 各マスの大きさ
    public ShogiPieceController pieceController; // 駒の管理クラス

    private GameObject[,] boardArray; // 9x9の将棋盤の配列

    void Start()
    {
        boardArray = new GameObject[rows, cols];
        GenerateBoard();
        PlaceInitialPieces(); // 駒を初期配置する
    }

    // 将棋盤を生成する
    void GenerateBoard()
    {
        // グリッド全体の幅と高さ
        float gridWidth = cols * cellSize;
        float gridHeight = rows * cellSize;

        // 中心を(0, 0)にするためのオフセット
        Vector2 offset = new Vector2(gridWidth / 2 - cellSize / 2, gridHeight / 2 - cellSize / 2);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                // オフセットを適用して、中心を(0, 0)に調整
                Vector3 position = new Vector3(x * cellSize - offset.x, y * cellSize - offset.y, 0);
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
                boardArray[x, y] = cell; // 各セルを配列に保存
                cell.name = $"Cell_{x}_{y}"; // デバッグ用に名前をつける
            }
        }
    }

    // 駒を初期配置する
    void PlaceInitialPieces()
    {
        // 味方側の駒の配置
        for (int x = 0; x < cols; x++)
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
        for (int x = 0; x < cols; x++)
        {
            pieceController.PlacePiece(x, 6, "歩", true); // 敵側の歩を6段目に配置（isEnemy = true）
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

    // 特定の座標にあるマス目を取得する関数
    public GameObject GetCellAtPosition(int x, int y)
    {
        if (x >= 0 && x < cols && y >= 0 && y < rows)
        {
            return boardArray[x, y];
        }
        return null;
    }
}
