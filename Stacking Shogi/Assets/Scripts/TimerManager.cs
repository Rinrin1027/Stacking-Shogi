using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TMP_Text playerTimerText; // プレイヤーのタイマー表示用
    [SerializeField] private TMP_Text enemyTimerText;  // 敵のタイマー表示用

    private float playerTime = 300f; // プレイヤーの初期持ち時間（5分）
    private float enemyTime = 300f;  // 敵の初期持ち時間（5分）

    private float playerExtraTime = 30f; // プレイヤーの追加時間（30秒）
    private float enemyExtraTime = 30f;  // 敵の追加時間（30秒）

    private bool playerInExtraTime = false; // プレイヤーが追加時間に突入しているか
    private bool enemyInExtraTime = false;  // 敵が追加時間に突入しているか

    private bool isPlayerTurn = true; // 現在のターンがプレイヤーかどうか

    public GameManager gameManager; // GameManagerの参照

    // ターンを切り替える
    public void SwitchTurn(bool isPlayer)
    {
        isPlayerTurn = isPlayer;

        // 追加時間に突入している場合、次のターンに30秒を与える
        if (isPlayerTurn)
        {
            if (playerInExtraTime)
            {
                playerExtraTime = 30f;
            }
        }
        else
        {
            if (enemyInExtraTime)
            {
                enemyExtraTime = 30f;
            }
        }
    }

    // タイマーの更新
    private void Update()
    {
        if (isPlayerTurn)
        {
            UpdateTimer(ref playerTime, ref playerExtraTime, ref playerInExtraTime, playerTimerText);
        }
        else
        {
            UpdateTimer(ref enemyTime, ref enemyExtraTime, ref enemyInExtraTime, enemyTimerText);
        }
    }

    // タイマーの更新処理
    private void UpdateTimer(ref float time, ref float extraTime, ref bool inExtraTime, TMP_Text timerText)
    {
        if (!inExtraTime)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                time = 0;
                inExtraTime = true; // 追加時間に移行
            }
        }
        else
        {
            extraTime -= Time.deltaTime;
            if (extraTime <= 0)
            {
                Debug.Log("タイマーがゼロになりました！");
                
                // 現在のプレイヤーのタグに基づいて勝者のシーンに遷移
                string winnerScene = gameManager.GetCurrentPlayerTag() == "Player" ? "SecondMoveWin" : "FirstMoveWin";
                UnityEngine.SceneManagement.SceneManager.LoadScene(winnerScene); // 勝者のシーンに遷移
            }
        }

        UpdateTimerDisplay(timerText, time, extraTime, inExtraTime);
    }

    // タイマーの表示を更新
    private void UpdateTimerDisplay(TMP_Text timerText, float time, float extraTime, bool inExtraTime)
    {
        if (inExtraTime)
        {
            timerText.text = string.Format("{0:00}", Mathf.Ceil(extraTime));
        }
        else
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
