using System.Collections.Generic;
using UnityEngine;

public class ShogiPieceController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject shogiBoard; // 将棋盤オブジェクト
    private ShogiPieceManager pieceManager; // 駒データの管理クラス
    private GameObject selectedPiece = null; // 現在選択されている駒
    private List<Vector2Int> validMovePositions = new List<Vector2Int>(); // 有効な移動範囲を保存
    private ShogiBoard shogiBoardScript; // ShogiBoardの参照
    [SerializeField] private CapturedPieces playerCapturedPieces; // プレイヤーの持ち駒
    [SerializeField] private CapturedPieces enemyCapturedPieces; // 敵の持ち駒
    private Dictionary<string, CapturedPieces> capturedPieces;

    public LayerMask pieceLayerMask; // 駒を検出するためのレイヤーマスク
    public LayerMask cellLayerMask;  // セルを検出するためのレイヤーマスク
    
    void Awake()
    {
        // ShogiPieceManagerコンポーネントを取得
        pieceManager = shogiBoard.GetComponent<ShogiPieceManager>();

        // ShogiBoardのスクリプト参照を取得
        shogiBoardScript = shogiBoard.GetComponent<ShogiBoard>();

        capturedPieces = new Dictionary<string, CapturedPieces>();
        capturedPieces["Player"] = playerCapturedPieces;
        capturedPieces["Enemy"] = enemyCapturedPieces;
    }

    void Update()
    {
    }

    // 駒の選択と移動を処理
    public bool HandlePieceSelectionAndMovement()
    {
        bool turnEnded = false;
        
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0)) // マウスボタンが押された時
        {
            if (selectedPiece == null)
            {
                // 駒を選択する処理（駒レイヤーのみを対象）
                RaycastHit2D hitPiece = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, pieceLayerMask);


                if (hitPiece.collider != null && hitPiece.collider.gameObject.CompareTag(gameManager.GetCurrentPlayerTag()))
                {
                    Debug.Log($"駒が選択されました: {hitPiece.collider.gameObject.name}");
                    selectedPiece = hitPiece.collider.gameObject; // 駒を選択
                    ShowMoveRange(selectedPiece); // 駒の移動範囲を表示
                }
            }
            else
            {
                // セルをクリックして駒を移動する処理（セルレイヤーを対象）
                RaycastHit2D hitCell = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, cellLayerMask);

                if (hitCell.collider != null)
                {
                    Debug.Log($"セルがクリックされました: {hitCell.collider.gameObject.name}");
                    Vector2Int clickedGridPosition = shogiBoardScript.GetGridPositionFromWorldPosition(mousePos);

                    // 有効な移動範囲か確認
                    if (validMovePositions.Contains(clickedGridPosition))
                    {
                        MovePiece(selectedPiece, clickedGridPosition); // 駒を移動
                        turnEnded = true;
                    }

                    // 前回の駒の移動範囲をリセット
                    ClearMoveRange();

                    // 選択解除
                    selectedPiece = null;
                    validMovePositions.Clear(); // 移動範囲をリセット
                }
            }
        }

        return turnEnded;
    }

    // 前の駒の移動範囲ハイライトをリセット
    void ClearMoveRange()
    {
        foreach (Vector2Int pos in validMovePositions)
        {
            shogiBoardScript.HighlightCell(pos.x, pos.y, false); // ハイライトを元に戻す
        }
        validMovePositions.Clear();
    }

    // 駒の移動範囲を表示する
    void ShowMoveRange(GameObject piece)
    {
        // 新しい駒を選択する前に前回の駒の移動範囲ハイライトをクリア
        ClearMoveRange();

        string pieceName = piece.name; // 駒の名前を取得
        ShogiPieceData pieceData = pieceManager.GetPieceData(pieceName); // 駒の移動データを取得

        if (pieceData != null)
        {
            Vector2Int pieceGridPosition = shogiBoardScript.GetGridPositionFromWorldPosition(piece.transform.position);

            bool isEnemy = piece.tag == "Enemy"; // 駒が敵かどうかを確認

            foreach (var move in pieceData.移動)
            {
                int directionMultiplier = isEnemy ? -1 : 1; // 敵の駒なら移動方向を反転

                for (int i = 1; i <= Mathf.Abs(move.距離); i++)
                {
                    Vector2Int newPosition = pieceGridPosition + new Vector2Int(move.x * i * directionMultiplier, move.y * i * directionMultiplier);

                    // グリッドの範囲内かを確認
                    if (newPosition.x >= 0 && newPosition.x < shogiBoardScript.cols && newPosition.y >= 0 && newPosition.y < shogiBoardScript.rows)
                    {   
                        if (shogiBoardScript.pieceArray[newPosition.x, newPosition.y] != null)
                        {
                            if (shogiBoardScript.pieceArray[newPosition.x, newPosition.y].CompareTag(gameManager.GetCurrentPlayerTag()))
                            {
                                break;
                            }
                            else
                            {
                                AddValidMovePosition(newPosition);
                                break;
                            }
                        }
                        else // 駒がない場合
                        {
                            AddValidMovePosition(newPosition);
                            Debug.Log($"駒 {pieceName} が移動できる範囲: {newPosition}");
                        }
                    }
                }
            }
        }

        // デバッグ用
        Debug.Log($"移動範囲のセル数: {validMovePositions.Count}");
        if (validMovePositions.Count == 0)
        {
            Debug.LogWarning("移動範囲が見つかりません。");
        }
    }

    void AddValidMovePosition(Vector2Int newPosition)
    {
        validMovePositions.Add(newPosition);
        // グリッドをハイライト
        shogiBoardScript.HighlightCell(newPosition.x, newPosition.y, true);
    }

    // 駒を移動する
    void MovePiece(GameObject piece, Vector2Int gridPosition)
    {
        // 駒をグリッドの中央にスナップさせる
        GameObject cell = shogiBoardScript.GetCellAtPosition(gridPosition.x, gridPosition.y);
        if (cell != null)
        {
            Vector2Int originPosition = shogiBoardScript.GetGridPositionFromWorldPosition(piece.transform.position); // 元の位置を取得
            
            piece.transform.position = cell.transform.position; // 駒をセルの位置にスナップ
            Debug.Log($"駒 {piece.name} が {originPosition} から {gridPosition} に移動しました");
            
            // 移動先に相手の駒があった場合は取る
            if (shogiBoardScript.pieceArray[gridPosition.x, gridPosition.y] != null && !shogiBoardScript
                    .pieceArray[gridPosition.x, gridPosition.y].CompareTag(gameManager.GetCurrentPlayerTag()))
            {
                capturedPieces[gameManager.GetCurrentPlayerTag()].AddPiece(shogiBoardScript.pieceArray[gridPosition.x, gridPosition.y].name);
                Destroy(shogiBoardScript.pieceArray[gridPosition.x, gridPosition.y]);
            }
            
            shogiBoardScript.pieceArray[originPosition.x, originPosition.y] = null; // 元の位置の配列をクリア
            shogiBoardScript.pieceArray[gridPosition.x, gridPosition.y] = piece; // 駒を配列に保存

            // 移動が完了したら、ハイライトを元に戻す
            ClearMoveRange();
        }
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
                
                shogiBoardScript.pieceArray[x, y] = piece; // 駒を配列に保存
            }
            else
            {
                Debug.LogError($"セルが見つかりません: ({x}, {y})");
            }
        }
    }

    void LogPieceArray()
    {
        string log = "";

        for (int y = shogiBoardScript.rows - 1; y >= 0; y--)
        {
            for (int x = 0; x < shogiBoardScript.cols; x++)
            {
                if (shogiBoardScript.pieceArray[x, y] != null)
                {
                    log += shogiBoardScript.pieceArray[x, y].name + " ";
                }
                else
                {
                    log += "null ";
                }
            }
            log += "\n";
        }
        Debug.Log(log);
    }
}
