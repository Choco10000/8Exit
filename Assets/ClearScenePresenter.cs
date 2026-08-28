using UnityEngine;
using UnityEngine.UI; // UI（Image）を使うために必要

public class ClearScenePresenter : MonoBehaviour
{
    [Header("UIの背景用Image")]
    [SerializeField] private Image backgroundImage;

    [Header("BGM再生用のAudioSource")]
    [SerializeField] private AudioSource bgmAudioSource;

    [Header("ゴール1用の表示設定")]
    [SerializeField] private Sprite goal1BackgroundSprite; // 背景画像1
    [SerializeField] private AudioClip goal1BGM;           // 音楽1

    [Header("ゴール2用の表示設定")]
    [SerializeField] private Sprite goal2BackgroundSprite; // 背景画像2
    [SerializeField] private AudioClip goal2BGM;           // 音楽2

    private void Start()
    {
        // 直前にクリアしたゴールのIDを取得（デフォルトは1）
        int goalID = GoalManager.LastGoalID;

        if (goalID == 2)
        {
            // ★ゴール2でクリアした場合
            ApplyClearData(goal2BackgroundSprite, goal2BGM);
        }
        else
        {
            // ★ゴール1でクリアした場合（または通常）
            ApplyClearData(goal1BackgroundSprite, goal1BGM);
        }
    }

    private void ApplyClearData(Sprite bgSprite, AudioClip bgmClip)
    {
        // 背景画像の変更
        if (backgroundImage != null && bgSprite != null)
        {
            backgroundImage.sprite = bgSprite;
        }

        // BGMの変更と再生
        if (bgmAudioSource != null && bgmClip != null)
        {
            bgmAudioSource.clip = bgmClip;
            bgmAudioSource.Play();
        }
    }
}