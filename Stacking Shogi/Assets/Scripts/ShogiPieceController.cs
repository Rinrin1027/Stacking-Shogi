using System.Collections.Generic;
using UnityEngine;

public class ShogiPieceController : MonoBehaviour
{
    public GameObject shogiBoard; // 将棋盤オブジェクト
    private ShogiPieceManager pieceManager; // 駒データの管理クラス
    private GameObject selectedPiece = null; // 現在選択されている駒

    void Start()
    {
        pieceManager = shogiBoard.GetComponent<ShogiPieceManager>();
    }

    void Update()
    {
        // 駒を選択する処理
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
                    selectedPiece = hitObject;
                    ShowMoveRange(selectedPiece);
                }
                else
                {
                    // 駒が選択されている場合は移動を行う
                    Vector3 newPosition = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0);
                    MovePiece(selectedPiece, newPosition);
                    selectedPiece = null;
                }
            }
        }
    }

    // 駒の移動範囲を表示する
    void ShowMoveRange(GameObject piece)
    {
        string pieceName = piece.name; // 駒の名前を取得
        ShogiPieceData pieceData = pieceManager.GetPieceData(pieceName); // 駒の移動データを取得

        if (pieceData != null)
        {
            foreach (var move in pieceData.移動)
            {
                // 駒の現在の位置を取得
                Vector3 piecePosition = piece.transform.position;

                // 各移動方向に基づいて移動範囲を計算
                for (int i = 1; i <= Mathf.Abs(move.距離); i++)
                {
                    Vector3 newPosition = piecePosition + new Vector3(move.x * i, move.y * i, 0);
                    Debug.Log($"駒 {pieceName} が移動できる範囲: {newPosition}");
                    // ここで移動可能範囲を可視化するなどの処理を行う
                }
            }
        }
    }

    // 駒を移動する
    void MovePiece(GameObject piece, Vector3 newPosition)
    {
        // 駒の移動処理
        piece.transform.position = newPosition;
    }
}
