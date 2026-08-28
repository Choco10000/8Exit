using UnityEngine;
using UnityEngine.SceneManagement; // シーン切り替えに必要な名前空間

public class TitleManager : MonoBehaviour
{
    // ボタンが押されたときに呼び出す関数
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }
}