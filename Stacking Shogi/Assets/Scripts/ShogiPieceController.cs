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
                        selectedPiece = hitPiece.collider.gameObject; // 駒を選択
                        isCapturedPiece = false;
                        ShowMoveRange(selectedPiece); // 駒の移動範囲を表示
                    }
                    else if (hitPiece.collider.gameObject.CompareTag("Captured" + gameManager.GetCurrentPlayerTag()))
                    {
                        // 持ち駒が選択された
                        if (capturedPieces[gameManager.GetCurrentPlayerTag()].HasPiece(hitPiece.collider.gameObject.name))
                        {
                            selectedPiece = hitPiece.collider.gameObject; // 駒を選択
                            isCapturedPiece = true;
                            ShowPutRange(selectedPiece); // 持ち駒の打てる範囲を表示
                        }
                        else
                        {
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

    // 駒の移動範囲を表示
    void ShowMoveRange(GameObject piece)
    {
        ClearMoveRange(); // 前回の駒の移動範囲ハイライトをリセット
        string pieceName = piece.name; 
        ShogiPieceData pieceData = pieceManager.GetPieceData(pieceName);

        if (pieceData != null)
        {
            Vector2Int pieceGridPosition = shogiBoardScript.GetGridPositionFromWorldPosition(piece.transform.position);
            bool isEnemy = piece.tag == "Enemy"; 

            foreach (var move in pieceData.移動)
            {
                int directionMultiplier = isEnemy ? -1 : 1; 
                for (int i = 1; i <= Mathf.Abs(move.距離); i++)
                {
                    Vector2Int newPosition = pieceGridPosition + new Vector2Int(move.x * i * directionMultiplier, move.y * i * directionMultiplier);

                    if (newPosition.x >= 0 && newPosition.x < shogiBoardScript.cols && newPosition.y >= 0 && newPosition.y < shogiBoardScript.rows)
                    {   
                        GameObject targetPiece = shogiBoardScript.pieceArray[newPosition.x, newPosition.y];

                        if (targetPiece != null && (targetPiece.name == "玉将" ||
                                                    (targetPiece.name == pieceName && targetPiece.CompareTag(gameManager.GetCurrentPlayerTag())) || 
                                                    IsFourCharacter(targetPiece.name)) && targetPiece.CompareTag(gameManager.GetCurrentPlayerTag()))
                        {
                            break; // 玉将、同じ駒、四文字の駒に移動不可
                        }

                        if (targetPiece != null && targetPiece.CompareTag(gameManager.GetCurrentPlayerTag()))
                        {
                            if (pieceName != "玉将" && !IsFourCharacter(pieceName)) 
                            {
                                AddValidMovePosition(newPosition, true); 
                            }
                            break;
                        }
                        else if (targetPiece != null && !targetPiece.CompareTag(gameManager.GetCurrentPlayerTag()))
                        {
                            AddValidMovePosition(newPosition);
                            break; 
                        }
                        else
                        {
                            AddValidMovePosition(newPosition);
                        }
                    }
                }
            }
        }
    }

    // 合成された駒が四文字かどうかを判定
    bool IsFourCharacter(string pieceName)
    {
        return pieceName.Length == 4;
    }

    // 駒を移動させる処理
    void MovePiece(GameObject piece, Vector2Int gridPosition)
    {
        GameObject cell = shogiBoardScript.GetCellAtPosition(gridPosition.x, gridPosition.y);
        if (cell != null)
        {
            Vector2Int originPosition = shogiBoardScript.GetGridPositionFromWorldPosition(piece.transform.position);
            piece.transform.position = cell.transform.position;
            shogiBoardScript.pieceArray[originPosition.x, originPosition.y] = null;

            GameObject targetPiece = shogiBoardScript.pieceArray[gridPosition.x, gridPosition.y];
            
            if (targetPiece != null && targetPiece.CompareTag(gameManager.GetCurrentPlayerTag()))
            {
                GameObject combinedPiece = pieceManager.GetCombinedPiecePrefab(piece.name, targetPiece.name, piece.CompareTag("Enemy"));

                if (combinedPiece != null)
                {
                    Destroy(piece);
                    Destroy(targetPiece);
                    combinedPiece.transform.position = cell.transform.position;
                    shogiBoardScript.pieceArray[gridPosition.x, gridPosition.y] = combinedPiece;
                }
            }
            else if (targetPiece != null && !targetPiece.CompareTag(gameManager.GetCurrentPlayerTag()))
            {
                if (targetPiece.name == "玉将")
                {
                    EndGameForKingCapture(targetPiece.tag);
                }
                else
                {
                    shogiBoardScript.pieceArray[gridPosition.x, gridPosition.y] = piece;
                    capturedPieces[gameManager.GetCurrentPlayerTag()].AddPiece(targetPiece.name);
                    Destroy(targetPiece);
                }
            }
            else
            {
                shogiBoardScript.pieceArray[gridPosition.x, gridPosition.y] = piece;
            }

            ClearMoveRange();
            movedPiece = shogiBoardScript.pieceArray[gridPosition.x, gridPosition.y];
        }
    }

    // 持ち駒の打てる範囲を表示
    void ShowPutRange(GameObject piece)
    {
        ClearMoveRange();
        string pieceName = piece.name; 
        ShogiPieceData pieceData = pieceManager.GetPieceData(pieceName);

        if (pieceData != null)
        {
            int frontMin = shogiBoardScript.rows - 1;
            foreach (var move in pieceData.移動)
            {
                frontMin = Math.Min(frontMin, move.y);
            }
            
            for (int y = 0; y < Math.Min(shogiBoardScript.rows, shogiBoardScript.rows - frontMin); y++)
            {
                for (int x = 0; x < shogiBoardScript.cols; x++)
                {
                    Vector2Int candidateGridPosition = gameManager.GetCurrentPlayerTag() == "Player"
                        ? new Vector2Int(x, y)
                        : new Vector2Int(x, shogiBoardScript.rows - 1 - y);

                    if (shogiBoardScript.pieceArray[candidateGridPosition.x, candidateGridPosition.y] == null)
                    {
                        if (pieceName == "歩兵" && !NoHuhyou(candidateGridPosition.x))
                        {
                            continue;
                        }

                        AddValidMovePosition(candidateGridPosition);
                    }
                }
            }
        }
    }

    // 合成対象を味方にするためのメソッド
    void AddValidMovePosition(Vector2Int newPosition, bool isFriendly = false)
    {
        validMovePositions.Add(newPosition);
        shogiBoardScript.HighlightCell(newPosition.x, newPosition.y, true, isFriendly);
    }

    // 玉将が取られた際のゲーム終了処理
    void EndGameForKingCapture(string capturedKingTag)
    {
        string winnerScene = (capturedKingTag == "Player") ? "SecondMoveWin" : "FirstMoveWin";
        UnityEngine.SceneManagement.SceneManager.LoadScene(winnerScene);
    }

    // 駒を新しく配置する処理
    public void PlacePiece(int x, int y, string pieceName, bool isEnemy = false)
    {
        GameObject piecePrefab = pieceManager.GetPiecePrefab(pieceName);

        if (piecePrefab != null)
        {
            GameObject cell = shogiBoardScript.GetCellAtPosition(x, y);
            if (cell != null)
            {
                GameObject piece = Instantiate(piecePrefab);
                piece.transform.position = cell.transform.position;
                piece.name = pieceName;
                piece.tag = isEnemy ? "Enemy" : "Player";

                if (isEnemy)
                {
                    piece.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
                
                shogiBoardScript.pieceArray[x, y] = piece;
            }
            else
            {
            }
        }
    }

    // 指定した列に自分の歩兵がないかどうか調べる
    bool NoHuhyou(int x)
    {
        for (int y = 0; y < shogiBoardScript.rows; y++)
        {
            if (shogiBoardScript.pieceArray[x, y] != null && shogiBoardScript.pieceArray[x, y].name == "歩兵" && shogiBoardScript.pieceArray[x, y].CompareTag(gameManager.GetCurrentPlayerTag())) 
            {
                return false;
            }
        }

        return true;
    }
}
