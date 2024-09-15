using UnityEngine;
using UnityEngine.UI;

public class ResignManager : MonoBehaviour
{
    public GameManager gameManager; // GameManagerの参照
    public GameObject resignPanel; // 投了確認のパネル
    public Button resignButton; // 投了ボタン
    public Button yesButton; // Yesボタン
    public Button noButton; // Noボタン
    public string buttonTag; // ボタンのタグ（PlayerまたはEnemy）

    void Start()
    {
        // 最初は投了確認パネルを非表示にしておく
        resignPanel.SetActive(false);

        // ボタンのイベントに関数を登録
        resignButton.onClick.AddListener(OnResignButtonPressed);
        yesButton.onClick.AddListener(OnYesButtonPressed);
        noButton.onClick.AddListener(OnNoButtonPressed);
    }

    void Update()
    {
        // ボタンのタグに基づいて、投了ボタンを有効/無効にする
        if (gameManager.GetCurrentPlayerTag() == buttonTag)
        {
            resignButton.interactable = true; // タグが一致していればボタンを有効化
        }
        else
        {
            resignButton.interactable = false; // タグが一致していなければボタンを無効化
        }
    }

    // 投了ボタンが押されたときに呼ばれる
    void OnResignButtonPressed()
    {
        resignPanel.SetActive(true); // パネルを表示
    }

    // Yesボタンが押されたときに呼ばれる
    void OnYesButtonPressed()
    {
        // プレイヤーか敵かに応じて勝者のシーンに遷移
        string winnerScene = gameManager.GetCurrentPlayerTag() == "Player" ? "SecondMoveWin" : "FirstMoveWin";
        UnityEngine.SceneManagement.SceneManager.LoadScene(winnerScene); // 勝者のシーンに遷移
    }

    // Noボタンが押されたときに呼ばれる
    void OnNoButtonPressed()
    {
        resignPanel.SetActive(false); // パネルを非表示にしてゲームを続行
    }
}