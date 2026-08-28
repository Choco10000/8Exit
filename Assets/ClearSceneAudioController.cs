using UnityEngine;
using UnityEngine.UI;

public class ClearSceneAudioController : MonoBehaviour
{
    [Header("BGM設定")]
    [SerializeField] private AudioSource clearBgmSource; // CLEARScene用BGM
    [SerializeField] private Slider bgmSlider;          // BGM用スライダー

    void Start()
    {
        // 他のシーン（GameSceneやTitleScene）で保存された音量を読み込む
        float savedBGM = PlayerPrefs.GetFloat("BGMVolume", 1.0f);

        // BGMの音量を適用
        if (clearBgmSource != null)
        {
            clearBgmSource.volume = savedBGM;
        }

        // スライダーの初期位置を合わせる
        if (bgmSlider != null)
        {
            bgmSlider.value = savedBGM;
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }
    }

    // BGM音量変更時の処理
    public void SetBGMVolume(float volume)
    {
        if (clearBgmSource != null)
        {
            clearBgmSource.volume = volume;
        }
        // 音量を保存（全シーン共通の "BGMVolume" キー）
        PlayerPrefs.SetFloat("BGMVolume", volume);
        PlayerPrefs.Save();
    }
}