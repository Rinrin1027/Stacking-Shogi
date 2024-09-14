using System.Collections.Generic;
using UnityEngine;

public class ShogiPieceController : MonoBehaviour
{
    public GameObject shogiBoard; // 将棋盤オブジェクト
    private ShogiPieceManager pieceManager; // 駒データの管理クラス
    private GameObject selectedPiece = null; // 現在選択されている駒
    private List<Vector2Int> validMovePositions = new List<Vector2Int>(); // 有効な移動範囲を保存
    private ShogiBoard shogiBoardScript; // ShogiBoardの参照

    private bool isPlayerTurn = true; // プレイヤーのターンかどうか
    private string currentPlayerTag = "Player"; // 現在のプレイヤーの駒のタグ（PlayerかEnemy）

    void Start()
    {
        // ShogiPieceManagerコンポーネントを取得
        pieceManager = shogiBoard.GetComponent<ShogiPieceManager>();

        // ShogiBoardのスクリプト参照を取得
        shogiBoardScript = shogiBoard.GetComponent<ShogiBoard>();
    }

    void Update()
    {
        HandlePieceSelectionAndMovement();
    }

    // 駒の選択と移動を処理
    void HandlePieceSelectionAndMovement()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0)) // マウスボタンが押された時
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject hitObject = hit.collider.gameObject;

                // 駒をクリックした場合
                if (selectedPiece == null && hitObject.tag == currentPlayerTag)
                {
                    selectedPiece = hitObject; // 駒を選択
                    ShowMoveRange(selectedPiece); // 駒の移動範囲を表示
                }
                // 駒が選択されていて、別の場所をクリックした場合
                else if (selectedPiece != null)
                {
                    Vector2Int clickedGridPosition = GetGridPositionFromWorldPosition(mousePos);

                    // 有効な移動範囲か確認
                    if (validMovePositions.Contains(clickedGridPosition))
                    {
                        MovePiece(selectedPiece, clickedGridPosition); // 駒を移動
                        SwitchTurn(); // ターンを切り替える
                    }

                    // 選択解除
                    selectedPiece = null;
                    validMovePositions.Clear(); // 移動範囲をリセット
                }
            }
        }
    }

    // グリッド座標をワールド座標から計算
    Vector2Int GetGridPositionFromWorldPosition(Vector3 worldPosition)
    {
        Vector2 offset = new Vector2(shogiBoardScript.cellSize * shogiBoardScript.cols / 2, shogiBoardScript.cellSize * shogiBoardScript.rows / 2);
        int x = Mathf.RoundToInt((worldPosition.x + offset.x) / shogiBoardScript.cellSize);
        int y = Mathf.RoundToInt((worldPosition.y + offset.y) / shogiBoardScript.cellSize);
        return new Vector2Int(x, y);
    }

    // 駒の移動範囲を表示する
    void ShowMoveRange(GameObject piece)
    {
        validMovePositions.Clear(); // 前の駒の移動範囲をクリア
        string pieceName = piece.name; // 駒の名前を取得
        ShogiPieceData pieceData = pieceManager.GetPieceData(pieceName); // 駒の移動データを取得

        if (pieceData != null)
        {
            Vector2Int pieceGridPosition = GetGridPositionFromWorldPosition(piece.transform.position);

            foreach (var move in pieceData.移動)
            {
                for (int i = 1; i <= Mathf.Abs(move.距離); i++)
                {
                    Vector2Int newPosition = pieceGridPosition + new Vector2Int(move.x * i, move.y * i);

                    // グリッドの範囲内かを確認
                    if (newPosition.x >= 0 && newPosition.x < shogiBoardScript.cols && newPosition.y >= 0 && newPosition.y < shogiBoardScript.rows)
                    {
                        validMovePositions.Add(newPosition);
                        Debug.Log($"駒 {pieceName} が移動できる範囲: {newPosition}");
                    }
                }
            }
        }
    }

    // 駒を移動する
    void MovePiece(GameObject piece, Vector2Int gridPosition)
    {
        // 駒をグリッドの中央にスナップさせる
        GameObject cell = shogiBoardScript.GetCellAtPosition(gridPosition.x, gridPosition.y);
        if (cell != null)
        {
            piece.transform.position = cell.transform.position; // 駒をセルの位置にスナップ
            Debug.Log($"駒 {piece.name} が {gridPosition} に移動しました");
        }
    }

    // ターンを切り替える
    void SwitchTurn()
    {
        isPlayerTurn = !isPlayerTurn; // ターンを切り替え

        // 現在のプレイヤーのタグを更新
        currentPlayerTag = isPlayerTurn ? "Player" : "Enemy"; // タグを更新

        Debug.Log($"次は {currentPlayerTag} のターンです");
    }

    // 駒を初期配置する関数
    public void PlacePiece(int x, int y, string pieceName, bool isEnemy = false)
    {
        // 駒の名前に応じたPrefabを取得
        GameObject piecePrefab = pieceManager.GetPiecePrefab(pieceName);

        if (piecePrefab != null)
        {
            // ShogiBoardのGetCellAtPositionを使ってセルを取得
            GameObject cell = shogiBoardScript.GetCellAtPosition(x, y);
            if (cell != null)
            {
                // 駒を生成して盤上に配置
                GameObject piece = Instantiate(piecePrefab);
                piece.transform.position = cell.transform.position; // 駒をセルの位置に配置
                piece.name = pieceName; // 駒の名前を設定

                // プレイヤーか敵かに応じてタグを設定
                piece.tag = isEnemy ? "Enemy" : "Player";

                if (isEnemy)
                {
                    piece.transform.rotation = Quaternion.Euler(0, 0, 180); // 敵の駒を回転
                }
            }
            else
            {
                Debug.LogError($"セルが見つかりません: ({x}, {y})");
            }
        }
    }
}
