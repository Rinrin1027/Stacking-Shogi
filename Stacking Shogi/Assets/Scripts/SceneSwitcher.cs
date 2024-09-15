using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // シーン名をInspectorで設定できるようにする
    [SerializeField] private string sceneName;

    // ボタンが押されたときに呼び出す関数
    public void OnButtonPress()
    {
        // シーンが正しく設定されているか確認
        if (!string.IsNullOrEmpty(sceneName))
        {
            // シーンをロード
            SceneManager.LoadScene(sceneName);
        }
    }
}