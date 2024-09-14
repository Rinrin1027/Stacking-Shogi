using UnityEngine;

public class ShogiBoard : MonoBehaviour
{
    public GameObject cellPrefab;  // マス目のPrefab
    public GameObject piecePrefab; // 駒のPrefab
    public int rows = 9;
    public int cols = 9;
    public float cellSize = 1.0f;

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
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                Vector3 position = new Vector3(x * cellSize, 0, y * cellSize);
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
        // 駒の生成
        GameObject piece = Instantiate(piecePrefab);
        piece.transform.position = boardArray[x, y].transform.position + new Vector3(0, 0.5f, 0); // マスの上に配置
        piece.name = pieceName;
        // ここに駒の種類に応じた表示やロジックを追加
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