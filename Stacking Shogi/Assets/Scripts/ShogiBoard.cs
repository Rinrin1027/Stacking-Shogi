using UnityEngine;

public class ShogiBoard : MonoBehaviour
{
    public GameObject cellPrefab;  // マス目のPrefab
    public int rows = 9;  // 縦のマス数
    public int cols = 9;  // 横のマス数
    public float cellSize = 1.0f;  // 各マスの大きさ

    private GameObject[,] boardArray; // 9x9の将棋盤の配列

    void Start()
    {
        boardArray = new GameObject[rows, cols];
        GenerateBoard();
        PlaceInitialPieces(); // 駒を配置する
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
        // 例として歩兵を2段目に配置
        for (int x = 0; x < cols; x++)
        {
            PlacePiece(x, 2, "歩"); // 2段目に歩を配置
        }

        // 例: 香車を配置
        PlacePiece(0, 0, "香車");
        PlacePiece(8, 0, "香車");

        // その他の駒も同様に配置
    }

    // 特定の座標に駒を配置する関数
    void PlacePiece(int x, int y, string pieceName)
    {
        // 駒を生成して配置するロジック
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