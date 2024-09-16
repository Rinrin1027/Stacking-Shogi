using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShogiPromotionManager : MonoBehaviour
{
    public ShogiPieceManager pieceManager;  // 駒データの管理クラス
    public ShogiBoard shogiBoard;  // 将棋盤の管理クラス
    public GameObject promotionPanel; // 成りを選択するパネル
    public Button selectButton; // 選択ボタン
    public Button confirmButton; // 確認ボタン
    
    public Vector2 origin; // 駒の初期位置
    public float offset; // 駒の間隔
    public float pieceSize; // 駒の大きさ
    
    private GameObject piece; // 成る駒
    List<string> selections = new List<string>(); // 成り選択肢
    List<GameObject> selectionObjects = new List<GameObject>(); // 成り選択肢のオブジェクト
    public int selectedIndex = 0; // 選択された駒のインデックス

    private void Start()
    {
        promotionPanel.SetActive(false); // 最初は非表示
        selectButton.onClick.AddListener(OnSelected); // 選択ボタンに関数を登録
        confirmButton.onClick.AddListener(OnConfirmed); // 確認ボタンに関数を登録
        
    }

    public void HandlePromotion(GameObject piece)
    {
        // 成り可能ならば成り選択を開始
        if (CanPromote(piece, shogiBoard.GetGridPositionFromWorldPosition(piece.transform.position).y))
        {
            this.piece = piece;
            AskPromotion();
        }
    }

    // 駒が成れるかを判定する関数
    public bool CanPromote(GameObject piece, int y)
    {
        bool isEnemy = piece.tag == "Enemy";
        
        if (pieceManager.GetPieceData(piece.name).成り.Count == 0) // 成れない駒は成り不可
        {
            return false;
        }
        
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

    // 成り選択を開始する
    void AskPromotion()
    {
        selectionObjects.Clear();
        selections.Clear();
        selectedIndex = 0;
        ShogiPieceData data = pieceManager.GetPieceData(piece.name);
        selections.Add(piece.name);

        for (int i = 0; i < data.成り.Count; i++)
        {
            selections.Add(data.成り[i]);
        }
        
        for (int i = 0; i < selections.Count; i++)
        {
            GameObject selectionObject = Instantiate(pieceManager.GetPiecePrefab(selections[i]), promotionPanel.transform);
            selectionObject.transform.position = promotionPanel.transform.position + new Vector3(origin.x + i * offset, origin.y, 0);
            selectionObject.transform.localScale = new Vector3(pieceSize, pieceSize, pieceSize);
            selectionObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            selectionObject.name = selections[i];
            if (i > 0)
            {
                selectionObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            }
            selectionObjects.Add(selectionObject);
        }
        
        promotionPanel.SetActive(true); // 成り選択パネルを表示
    }
    
    void OnSelected()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        for (int i = 0; i < selectionObjects.Count; i++)
        {
            if (selectionObjects[i].GetComponent<Collider2D>().OverlapPoint(mousePos))
            {
                selectedIndex = i;
                selectionObjects[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1.0f);
            }
            else
            {
                selectionObjects[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            }
        }
    }
    
    void OnConfirmed()
    {
        if (selectedIndex != 0) // そのままでなければ成り処理を行う
        {
            string selectedPieceName = selections[selectedIndex];
            GameObject promotedPiecePrefab = pieceManager.GetPiecePrefab(selectedPieceName);

            if (promotedPiecePrefab != null)
            {
                Vector3 promotedPiecePosition = piece.transform.position;
                Quaternion promotedPieceRotation = piece.transform.rotation;
                bool isEnemy = piece.tag == "Enemy";

                Destroy(piece);
                GameObject promotedPiece =
                    Instantiate(promotedPiecePrefab, promotedPiecePosition, promotedPieceRotation);
                promotedPiece.tag = isEnemy ? "Enemy" : "Player";
                promotedPiece.name = selectedPieceName;

                Vector2Int gridPosition = shogiBoard.GetGridPositionFromWorldPosition(promotedPiecePosition);
                shogiBoard.pieceArray[gridPosition.x, gridPosition.y] = promotedPiece;
            }
        }

        promotionPanel.SetActive(false); // 成り選択パネルを非表示にする
        selections.Clear();
        foreach (GameObject selectionObject in selectionObjects)
        {
            Destroy(selectionObject);
        }
    }
}