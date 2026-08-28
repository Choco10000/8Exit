using UnityEngine;
using UnityEngine.UI;

public class TitleAudioController : MonoBehaviour
{
    [Header("BGM設定")]
    [SerializeField] private AudioSource titleBgmSource; // タイトル用BGM
    [SerializeField] private Slider bgmSlider;

    [Header("SE設定")]
    [SerializeField] private AudioSource seAudioSource;  // SE再生用のAudioSource
    [SerializeField] private AudioClip seTestClip;       // スライダー操作時に試聴で鳴らすSE
    [SerializeField] private Slider seSlider;

    void Start()
    {
        // --- BGMの初期化 ---
        if (bgmSlider != null)
        {
            float savedBGM = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
            bgmSlider.value = savedBGM;
            SetBGMVolume(savedBGM);

            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        // --- SEの初期化 ---
        if (seSlider != null)
        {
            float savedSE = PlayerPrefs.GetFloat("SEVolume", 1.0f);
            seSlider.value = savedSE;
            SetSEVolume(savedSE);

            seSlider.onValueChanged.AddListener(SetSEVolume);
        }
    }

    // BGM音量変更
    public void SetBGMVolume(float volume)
    {
        if (titleBgmSource != null)
        {
            titleBgmSource.volume = volume;
        }
        PlayerPrefs.SetFloat("BGMVolume", volume);
        PlayerPrefs.Save();
    }

    // SE音量変更
    public void SetSEVolume(float volume)
    {
        if (seAudioSource != null)
        {
            seAudioSource.volume = volume;
        }
        PlayerPrefs.SetFloat("SEVolume", volume);
        PlayerPrefs.Save();
    }

    // SEスライダー操作時（指を離した時など）に確認用SEを鳴らす処理
    public void PlayTestSE()
    {
        if (seAudioSource != null && seTestClip != null)
        {
            seAudioSource.PlayOneShot(seTestClip);
        }
    }
}