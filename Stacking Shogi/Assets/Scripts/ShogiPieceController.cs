using System;
using System.Collections.Generic;
using UnityEngine;

public class ShogiPieceController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject shogiBoard; // 将棋盤オブジェクト
    private ShogiPieceManager pieceManager; // 駒データの管理クラス
    private GameObject selectedPiece = null; // 現在選択されている駒
    private bool isCapturedPiece = false; // 持ち駒を選択しているかどうか
    private List<Vector2Int> validMovePositions = new List<Vector2Int>(); // 有効な移動範囲を保存
    public GameObject movedPiece = null;
    private ShogiBoard shogiBoardScript; // ShogiBoardの参照
    [SerializeField] private CapturedPieces playerCapturedPieces; // プレイヤーの持ち駒
    [SerializeField] private CapturedPieces enemyCapturedPieces; // 敵の持ち駒
    private Dictionary<string, CapturedPieces> capturedPieces;
    public AudioClip moveSoundEffect; // 駒を動かす時のSE
    private AudioSource audioSource;

    public LayerMask pieceLayerMask; // 駒を検出するためのレイヤーマスク
    public LayerMask cellLayerMask;  // セルを検出するためのレイヤーマスク
    
    void Awake()
    {
        // ShogiPieceManagerコンポーネントを取得
        pieceManager = shogiBoard.GetComponent<ShogiPieceManager>();

        // ShogiBoardのスクリプト参照を取得
        shogiBoardScript = shogiBoard.GetComponent<ShogiBoard>();
        // AudioSourceを取得
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // AudioSourceが存在しない場合は自動で追加
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // 持ち駒オブジェクトを辞書で管理
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
        movedPiece = null;
        bool turnEnded = false;
        
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0)) // マウスボタンが押された時
        {
            if (selectedPiece == null)
            {
                // 駒を選択する処理（駒レイヤーのみを対象）
                RaycastHit2D hitPiece = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, pieceLayerMask);
                
                if (hitPiece.collider != null)
                {
                    if (hitPiece.collider.gameObject.CompareTag(gameManager.GetCurrentPlayerTag()))
                    {
                        // 盤上の駒が選択された
                        Debug.Log($"駒が選択されました: {hitPiece.collider.gameObject.name}");
                        selectedPiece = hitPiece.collider.gameObject; // 駒を選択
                        isCapturedPiece = false;
                        ShowMoveRange(selectedPiece); // 駒の移動範囲を表示
                    }
                    else if (hitPiece.collider.gameObject.CompareTag("Captured" + gameManager.GetCurrentPlayerTag()))
                    {
                        // 持ち駒が選択された
                        if (capturedPieces[gameManager.GetCurrentPlayerTag()].HasPiece(hitPiece.collider.gameObject.name))
                        {
                            Debug.Log($"持ち駒が選択されました: {hitPiece.collider.gameObject.name}");
                            selectedPiece = hitPiece.collider.gameObject; // 駒を選択
                            isCapturedPiece = true;
                            ShowPutRange(selectedPiece); // 持ち駒の打てる範囲を表示
                        }
                        else
                        {
                            Debug.Log("その駒は持っていません");
                        }
                    }
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
                        if (isCapturedPiece)
                        {
                            // 持ち駒なら駒を新規配置
                            PlacePiece(clickedGridPosition.x, clickedGridPosition.y, selectedPiece.name, selectedPiece.CompareTag("CapturedEnemy"));
                            capturedPieces[gameManager.GetCurrentPlayerTag()].RemovePiece(selectedPiece.name);
                        }
                        else
                        {
                            // 盤上の駒ならその駒を移動
                            MovePiece(selectedPiece, clickedGridPosition); 
                        }

                        // SEを再生する
                        PlayMoveSound();
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

    // 駒を動かす時のSEを再生する
    void PlayMoveSound()
    {
        if (moveSoundEffect != null && audioSource != null)
        {
            audioSource.PlayOneShot(moveSoundEffect); // SEを再生
        }
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
                        GameObject targetPiece = shogiBoardScript.pieceArray[newPosition.x, newPosition.y];
                        
                        // 味方の駒があれば合成可能範囲として追加
                        if (targetPiece != null && targetPiece.CompareTag(gameManager.GetCurrentPlayerTag()))
                        {
                            AddValidMovePosition(newPosition, true); // 味方の上に移動可能な位置として追加
                            break; // それ以上進めない
                        }
                        else if (targetPiece != null && targetPiece.CompareTag("Enemy"))
                        {
                            // 敵の駒があれば、その駒を取ることができるので追加
                            AddValidMovePosition(newPosition);
                            break; // それ以上進めない
                        }
                        else
                        {
                            // 駒がない場合は移動範囲に追加
                            AddValidMovePosition(newPosition);
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

    // 持ち駒の打てる範囲を表示する
    void ShowPutRange(GameObject piece)
    {
        // 新しい駒を選択する前に前回の駒の移動範囲ハイライトをクリア
        ClearMoveRange();
        
        string pieceName = piece.name; // 駒の名前を取得
        ShogiPieceData pieceData = pieceManager.GetPieceData(pieceName); // 駒の移動データを取得

        if (pieceData != null)
        {
            int frontMin = shogiBoardScript.rows - 1; // 最小前進マス数
            foreach (var move in pieceData.移動)
            {
                if (move.y < frontMin) frontMin = move.y;
            }

            for (int r = 0; r < Math.Min(shogiBoardScript.rows, shogiBoardScript.rows - frontMin); r++)
            {
                for (int c = 0; c < shogiBoardScript.cols; c++)
                {
                    // 打てるマス候補
                    Vector2Int candidateGridPosition = piece.CompareTag("CapturedPlayer")
                        ? new Vector2Int(c, r)
                        : new Vector2Int(c, shogiBoardScript.rows - 1 - r);
                    
                    // 駒がない
                    if (shogiBoardScript.pieceArray[candidateGridPosition.x, candidateGridPosition.y] == null)
                    {
                        if (pieceName == "歩兵")
                        {
                            // 歩兵は2歩禁止
                            if (NoHuhyou(candidateGridPosition.x))
                            {
                                AddValidMovePosition(candidateGridPosition);
                            }
                        }
                        else
                        {
                            AddValidMovePosition(candidateGridPosition);
                        }
                    }
                }
            }
        }
    }

    void AddValidMovePosition(Vector2Int newPosition, bool isFriendly = false)
    {
        validMovePositions.Add(newPosition);
        // グリッドをハイライト（味方の駒がある場合は特別なスプライトを使用）
        shogiBoardScript.HighlightCell(newPosition.x, newPosition.y, true, isFriendly);
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
        
            // 移動先に相手の駒があった場合
            GameObject targetPiece = shogiBoardScript.pieceArray[gridPosition.x, gridPosition.y];
            if (targetPiece != null && !targetPiece.CompareTag(gameManager.GetCurrentPlayerTag()))
            {
                // 玉将が取られた場合、ゲーム終了処理を実行
                if (targetPiece.name == "玉将")
                {
                    Debug.Log("玉将が取られました。ゲーム終了処理を実行します。");
                    // 玉将が取られた場合の特別な処理（ゲーム終了など）
                    EndGameForKingCapture(targetPiece.tag);
                    Destroy(targetPiece); // 王を削除
                }
                else
                {
                    // 玉将以外の駒を持ち駒に追加
                    capturedPieces[gameManager.GetCurrentPlayerTag()].AddPiece(targetPiece.name);
                    Destroy(targetPiece); // 玉将以外の駒を削除
                }
            }
        
            shogiBoardScript.pieceArray[originPosition.x, originPosition.y] = null; // 元の位置の配列をクリア
            shogiBoardScript.pieceArray[gridPosition.x, gridPosition.y] = piece; // 駒を配列に保存

            // 移動が完了したら、ハイライトを元に戻す
            ClearMoveRange();
            
            movedPiece = piece;
        }
    }

    // 玉将が取られた際のゲーム終了処理を実行する関数
    void EndGameForKingCapture(string capturedKingTag)
    {
        // 玉将が取られたプレイヤーを確認し、勝者のシーンに遷移する
        string winnerScene = (capturedKingTag == "Player") ? "SecondMoveWin" : "FirstMoveWin";
        Debug.Log($"{capturedKingTag} の王が取られました。{winnerScene} に遷移します。");

        // 勝利シーンに遷移
        UnityEngine.SceneManagement.SceneManager.LoadScene(winnerScene);
    }

    // 駒を新しく配置する関数
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

    // 指定した列に自分の歩兵がないかどうか調べる
    bool NoHuhyou(int x)
    {
        for (int y = 0; y < shogiBoardScript.rows; y++)
        {
            if (shogiBoardScript.pieceArray[x, y] != null && 
                shogiBoardScript.pieceArray[x, y].name == "歩兵" &&
                shogiBoardScript.pieceArray[x, y].CompareTag(gameManager.GetCurrentPlayerTag())) 
            {
                return false;
            }
        }

        return true;
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
