using System;
using System.Collections.Generic;
using UnityEngine;

public class ShogiPromotionManager : MonoBehaviour
{
    
    public ShogiPieceManager pieceManager;  // 駒データの管理クラス
    public ShogiBoard shogiBoard;  // 将棋盤の管理クラス

    public void HandlePromotion(GameObject piece)
    {
        if (CanPromote(piece, shogiBoard.GetGridPositionFromWorldPosition(piece.transform.position).y))
        {
            Debug.Log("駒を成る");
            PromotePiece(piece);
        }
        else
        {
            Debug.Log("駒を成らない");
        }
    }

    private void Awake()
    {

    }

    // 駒が成れるかを判定する関数
    public bool CanPromote(GameObject piece, int y)
    {
        bool isEnemy = piece.tag == "Enemy";

        if (isEnemy && y <= 2) // 敵がプレイヤー側の手前3列に進んだら成り可能
        {
            return true;
        }
        else if (!isEnemy && y >= 6) // プレイヤーが敵陣の手前3列に進んだら成り可能
        {
            return true;
        }

        return false;
    }

    // 駒を成り駒に変換する処理
    public void PromotePiece(GameObject piece)
    {
        ShogiPieceData data = pieceManager.GetPieceData(piece.name);
        GameObject promotedPiecePrefab = null;

        if (data.成り != "false")
        {
            promotedPiecePrefab = pieceManager.GetPromotedPiecePrefab(data.成り);
        }

        if (promotedPiecePrefab != null)
        {
            Vector3 promotedPiecePosition = piece.transform.position;
            Quaternion promotedPieceRotation = piece.transform.rotation;
            bool isEnemy = piece.tag == "Enemy";

            Destroy(piece);
            GameObject promotedPiece = Instantiate(promotedPiecePrefab, promotedPiecePosition, promotedPieceRotation);
            promotedPiece.tag = isEnemy ? "Enemy" : "Player";
            promotedPiece.name = data.成り;
            
            Vector2Int gridPosition = shogiBoard.GetGridPositionFromWorldPosition(promotedPiecePosition);
            shogiBoard.pieceArray[gridPosition.x, gridPosition.y] = promotedPiece;
        }
    }
}