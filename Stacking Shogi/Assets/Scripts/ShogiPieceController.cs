using System.Collections.Generic;
using UnityEngine;

public class ShogiPieceController : MonoBehaviour
{
    public GameObject shogiBoard; // 将棋盤オブジェクト
    private ShogiPieceManager pieceManager; // 駒データの管理クラス
    private GameObject selectedPiece = null; // 現在選択されている駒
    private List<Vector3> validMovePositions = new List<Vector3>(); // 有効な移動範囲を保存
    private ShogiBoard shogiBoardScript; // ShogiBoardの参照

    void Start()
    {
        // ShogiPieceManagerコンポーネントを取得
        pieceManager = shogiBoard.GetComponent<ShogiPieceManager>();

        // ShogiBoardのスクリプト参照を取得
        shogiBoardScript = shogiBoard.GetComponent<ShogiBoard>();
    }

    void Update()
    {
        // 駒を選択または移動する処理
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject hitObject = hit.collider.gameObject;

                if (selectedPiece == null)
                {
                    // 駒が選択されていない場合は駒を選択
                    if (hitObject.tag == "Piece") // 駒にタグを付けて判定
                    {
                        selectedPiece = hitObject;
                        ShowMoveRange(selectedPiece);
                    }
                }
                else
                {
                    // 駒が選択されている場合は移動を行う
                    Vector3 newPosition = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0);

                    // 有効な移動範囲か確認
                    if (validMovePositions.Contains(newPosition))
                    {
                        MovePiece(selectedPiece, newPosition);
                        selectedPiece = null;
                        validMovePositions.Clear(); // 移動後に移動範囲をリセット
                    }
                }
            }
        }
    }

    // 駒の移動範囲を表示する
    void ShowMoveRange(GameObject piece)
    {
        validMovePositions.Clear(); // 前の駒の移動範囲をクリア
        string pieceName = piece.name; // 駒の名前を取得
        ShogiPieceData pieceData = pieceManager.GetPieceData(pieceName); // 駒の移動データを取得

        if (pieceData != null)
        {
            Vector3 piecePosition = piece.transform.position;

            foreach (var move in pieceData.移動)
            {
                // 各移動方向に基づいて移動範囲を計算
                for (int i = 1; i <= Mathf.Abs(move.距離); i++)
                {
                    Vector3 newPosition = piecePosition + new Vector3(move.x * i, move.y * i, 0);
                    validMovePositions.Add(newPosition);
                    Debug.Log($"駒 {pieceName} が移動できる範囲: {newPosition}");
                    
                    // ここで移動可能範囲を可視化する処理を追加できます（例: ハイライト）
                }
            }
        }
    }

    // 駒を移動する
    void MovePiece(GameObject piece, Vector3 newPosition)
    {
        // 駒の移動処理
        piece.transform.position = newPosition;
        Debug.Log($"駒 {piece.name} が {newPosition} に移動しました");
        
        // 駒の成り処理などの追加機能もここに追加できます
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

                // 敵側の駒なら180度回転させる
                if (isEnemy)
                {
                    piece.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
            }
            else
            {
                Debug.LogError($"セルが見つかりません: ({x}, {y})");
            }
        }
    }
}
