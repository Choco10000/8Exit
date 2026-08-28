using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ClearManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultTimerText;

    void Start()
    {
        // PlayerPrefsから保存されたクリアタイムを取得
        float clearTime = PlayerPrefs.GetFloat("ClearTime", 0f);

        // RankingManager の FormatTime を使って綺麗に表示
        if (resultTimerText != null)
        {
            resultTimerText.text = "CLEAR TIME: " + RankingManager.FormatTime(clearTime);
        }
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("TitleScene");
    }
}