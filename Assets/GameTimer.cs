using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("UI設定")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject menuPanel;

    [Header("オーディオ設定 (AudioSource)")]
    [SerializeField] private AudioSource bgmSource; // BGMを再生しているAudioSource
    [SerializeField] private AudioSource[] seSources; // SEを再生しているAudioSource（複数登録可）
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    private float elapsedTime = 0f;
    private bool isRunning = false;
    private bool isPaused = false;

    void Start()
    {
        isRunning = true;
        Time.timeScale = 1f;

        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }

        // BGMスライダー初期化 (デフォルト音量: 1.0)
        if (bgmSlider != null)
        {
            float savedBGM = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
            bgmSlider.value = savedBGM;
            SetBGMVolume(savedBGM);
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        // SEスライダー初期化 (デフォルト音量: 1.0)
        if (seSlider != null)
        {
            float savedSE = PlayerPrefs.GetFloat("SEVolume", 1.0f);
            seSlider.value = savedSE;
            SetSEVolume(savedSE);
            seSlider.onValueChanged.AddListener(SetSEVolume);
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            TogglePause();
        }

        if (isRunning && !isPaused)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    // BGM音量調整（0.0 ~ 1.0）
    public void SetBGMVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    // SE音量調整（0.0 ~ 1.0）
    public void SetSEVolume(float volume)
    {
        if (seSources != null)
        {
            foreach (AudioSource se in seSources)
            {
                if (se != null)
                {
                    se.volume = volume;
                }
            }
        }
        PlayerPrefs.SetFloat("SEVolume", volume);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (menuPanel != null)
        {
            menuPanel.SetActive(isPaused);
        }

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        if (isPaused) TogglePause();
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }

    public void StopTimerAndSave()
    {
        isRunning = false;

        PlayerPrefs.SetFloat("ClearTime", elapsedTime);
        RankingManager.SaveTime(elapsedTime);
        PlayerPrefs.Save();

        Debug.Log($"【GameScene】タイムを記録しました: {elapsedTime}秒");
    }

    public void LoadClearScene()
    {
        Time.timeScale = 1f;
        StopTimerAndSave();
        SceneManager.LoadScene("CLEARScene");
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 100F) % 100F);

        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
        }
    }
}