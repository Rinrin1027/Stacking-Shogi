using UnityEngine;

public class ShogiBoard : MonoBehaviour
{
    public GameObject cellPrefab; // マス目のPrefab
    public int rows = 9;
    public int cols = 9;
    public float cellSize = 1.0f;

    private GameObject[,] boardArray; // 9x9の将棋盤の配列

    void Start()
    {
        boardArray = new GameObject[rows, cols];
        GenerateBoard();
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
            }
        }
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