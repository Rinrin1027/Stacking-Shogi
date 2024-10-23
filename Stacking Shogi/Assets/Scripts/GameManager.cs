using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ShogiBoard shogiBoard;
    [SerializeField] private ShogiPieceController shogiPieceController;
    [SerializeField] private ShogiPromotionManager shogiPromotionManager;
    [SerializeField] private TimerManager timerManager; // TimerManagerの参照

    private bool isPlayerTurn = true; // プレイヤーのターンかどうか
    [SerializeField] private string currentPlayerTag = "Player"; // 現在のプレイヤーのタグ

    // ゲームの初期化
    private void Start()
    {
        InitializeGame();
    }

    // ゲームの初期化処理
    private void InitializeGame()
    {
        shogiBoard.GenerateBoard();
        shogiBoard.PlaceInitialPieces();
    }

    private void Update()
    {
        bool turnEnded = shogiPieceController.HandlePieceSelectionAndMovement();
        if (shogiPieceController.movedPiece != null)
        {
            shogiPromotionManager.HandlePromotion(shogiPieceController.movedPiece, shogiPieceController.movedPieceOriginY, shogiPieceController.movedPieceDestinationY);
        }

        if (turnEnded) SwitchTurn();
    }

    // ターンを切り替える
    public void SwitchTurn()
    {
        isPlayerTurn = !isPlayerTurn; // ターンを切り替え
        currentPlayerTag = isPlayerTurn ? "Player" : "Enemy"; // タグを更新
        timerManager.SwitchTurn(isPlayerTurn); // TimerManagerに現在のターンを伝える
    }

    // 現在のプレイヤーのタグを取得する
    public string GetCurrentPlayerTag()
    {
        return currentPlayerTag;
    }

    // 現在がプレイヤーのターンかどうかを確認する
    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
