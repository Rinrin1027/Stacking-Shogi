using UnityEngine;

public class ShogiBoard : MonoBehaviour
{
    public GameObject cellPrefab;  // マス目のPrefab
    public int rows = 9;  // 縦のマス数
    public int cols = 9;  // 横のマス数
    public float cellSize = 1.0f;  // 各マスの大きさ
    public ShogiPieceController pieceController; // 駒の管理クラス

    private GameObject[,] boardArray; // 9x9の将棋盤の配列
    public GameObject[,] pieceArray; // 駒の配列
    public Sprite defaultSprite; // 通常のマス目スプライト
    public Sprite highlightedSprite; // ハイライト用のマス目スプライト

    void Awake()
    {
        boardArray = new GameObject[rows, cols];
        pieceArray = new GameObject[rows, cols];
    }

    // 将棋盤を生成する
    public void GenerateBoard()
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

                // スプライトの設定
                SpriteRenderer renderer = cell.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.sprite = defaultSprite;
                }
            }
        }
    }

    // 駒を初期配置する
    public void PlaceInitialPieces()
    {
        // 味方側の駒の配置
        for (int x = 0; x < cols; x++)
        {
            pieceController.PlacePiece(x, 2, "歩兵"); // 2段目に歩を配置
        }
        pieceController.PlacePiece(0, 0, "香車");
        pieceController.PlacePiece(8, 0, "香車");
        pieceController.PlacePiece(1, 0, "桂馬");
        pieceController.PlacePiece(7, 0, "桂馬");
        pieceController.PlacePiece(2, 0, "銀将");
        pieceController.PlacePiece(6, 0, "銀将");
        pieceController.PlacePiece(3, 0, "金将");
        pieceController.PlacePiece(5, 0, "金将");
        pieceController.PlacePiece(4, 0, "玉");
        pieceController.PlacePiece(7, 1, "飛車");
        pieceController.PlacePiece(1, 1, "角行");

        // 敵側の駒の配置
        for (int x = 0; x < cols; x++)
        {
            pieceController.PlacePiece(x, 6, "歩兵", true); // 敵側の歩を6段目に配置（isEnemy = true）
        }
        pieceController.PlacePiece(0, 8, "香車", true);
        pieceController.PlacePiece(8, 8, "香車", true);
        pieceController.PlacePiece(1, 8, "桂馬", true);
        pieceController.PlacePiece(7, 8, "桂馬", true);
        pieceController.PlacePiece(2, 8, "銀将", true);
        pieceController.PlacePiece(6, 8, "銀将", true);
        pieceController.PlacePiece(3, 8, "金将", true);
        pieceController.PlacePiece(5, 8, "金将", true);
        pieceController.PlacePiece(4, 8, "玉", true);
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
    
    // グリッド座標をワールド座標から計算
    public Vector2Int GetGridPositionFromWorldPosition(Vector3 worldPosition)
    {
        Vector2 offset = new Vector2(cellSize * cols / 2, cellSize * rows / 2);
        int x = Mathf.FloorToInt((worldPosition.x + offset.x) / cellSize);
        int y = Mathf.FloorToInt((worldPosition.y + offset.y) / cellSize);
        return new Vector2Int(x, y);
    }

    // グリッドのスプライトを変更
    public void HighlightCell(int x, int y, bool highlight)
    {
        GameObject cell = GetCellAtPosition(x, y);
        if (cell != null)
        {
            SpriteRenderer renderer = cell.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sprite = highlight ? highlightedSprite : defaultSprite;
            }
        }
    }
}
