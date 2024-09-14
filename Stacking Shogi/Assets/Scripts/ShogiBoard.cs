using UnityEngine;

public class ShogiBoard : MonoBehaviour
{
    public GameObject cellPrefab; // マス目のPrefab
    public int rows = 9;
    public int cols = 9;
    public float cellSize = 1.0f;

    void Start()
    {
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
                Instantiate(cellPrefab, position, Quaternion.identity);
            }
        }
    }
}