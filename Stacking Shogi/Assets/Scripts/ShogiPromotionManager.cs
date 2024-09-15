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

    // 駒が成れるかを判定する関数
    public bool CanPromote(GameObject piece, int y)
    {
        bool isEnemy = piece.tag == "Enemy";

        if (isEnemy && y <= 2) // 敵がプレイヤー側の手前3列に進んだら成り可能
        {
            return true;
        }
        if (!isEnemy && y >= 6) // プレイヤーが敵陣の手前3列に進んだら成り可能
        {
            return true;
        }

        return false;
    }

    // 駒を成り駒に変換する処理
    public void PromotePiece(GameObject piece)
    {
        string pieceName = piece.name;
        GameObject promotedPiecePrefab = null;

        if (pieceName == "飛車")
        {
            promotedPiecePrefab = pieceManager.promotedRookPrefab;
        }
        else if (pieceName == "角行")
        {
            promotedPiecePrefab = pieceManager.promotedBishopPrefab;
        }
        else if (pieceName != "金" && pieceName != "王")
        {
            promotedPiecePrefab = pieceManager.promotedPawnPrefab;
        }

        if (promotedPiecePrefab != null)
        {
            string promotedPieceName = pieceManager.GetPieceData(pieceName).成り;
            Vector3 currentPosition = piece.transform.position;
            Quaternion currentRotation = piece.transform.rotation;
            bool isEnemy = piece.tag == "Enemy";

            Destroy(piece);
            GameObject promotedPiece = Instantiate(promotedPiecePrefab, currentPosition, currentRotation);
            promotedPiece.tag = isEnemy ? "Enemy" : "Player";
            promotedPiece.name = promotedPieceName;
            
            Vector2Int gridPosition = shogiBoard.GetGridPositionFromWorldPosition(currentPosition);
            shogiBoard.pieceArray[gridPosition.x, gridPosition.y] = promotedPiece;
        }
    }
}