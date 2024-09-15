using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isPlayerTurn = true; // プレイヤーのターンかどうか
    private string currentPlayerTag = "Player"; // 現在のプレイヤーのタグ

    // ゲームの初期化
    private void Start()
    {
        InitializeGame();
    }

    // ゲームの初期化処理
    private void InitializeGame()
    {
        Debug.Log("ゲームを初期化しました。プレイヤーから開始します。");
    }

    // ターンを切り替える
    public void SwitchTurn()
    {
        isPlayerTurn = !isPlayerTurn; // ターンを切り替え
        currentPlayerTag = isPlayerTurn ? "Player" : "Enemy"; // タグを更新
        Debug.Log($"次は {currentPlayerTag} のターンです");
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