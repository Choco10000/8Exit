using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalManager : MonoBehaviour
{
    // ★どちらのゴールでクリアしたかを保存する静的変数（シーン切り替え後も値が残る）
    public static int LastGoalID { get; private set; } = 1;

    [Header("ゴールの識別番号（1 または 2）")]
    [SerializeField] private int goalID = 1;

    [Header("判定対象のタグ")]
    [SerializeField] private string targetTag = "Player";

    [Header("遷移先のシーン名")]
    [SerializeField] private string clearSceneName = "CLEARScene";

    [Header("フェード用のCanvasGroup（オプション）")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    [Header("クリア時のフェードアウト時間（秒）")]
    [SerializeField] private float fadeDuration = 1.0f;

    private bool isCleared = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag) && !isCleared)
        {
            StartCoroutine(ClearSequence(collision));
        }
    }

    public IEnumerator ClearSequence(Collider2D playerCollider)
    {
        isCleared = true;

        // ★今回入ったゴールのIDを記憶する
        LastGoalID = goalID;

        // プレイヤーの移動を停止
        PlayerMovement playerMovementScript = playerCollider.GetComponent<PlayerMovement>();
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        Rigidbody2D rb = playerCollider.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        Debug.Log($"🎉 ゴール {goalID} に到達しました！");

        // ★追加：タイマーを止めてタイムを保存する
        GameTimer gameTimer = FindFirstObjectByType<GameTimer>(); // Unity 2023以降
        // ※古いUnityバージョンの場合は FindObjectOfType<GameTimer>() に書き換えてください
        if (gameTimer != null)
        {
            gameTimer.StopTimerAndSave();
        }
        else
        {
            Debug.LogWarning("GameTimer がシーン内に見つかりませんでした！");
        }

        // フェードアウト（画面を白/黒にする）
        if (fadeCanvasGroup != null)
        {
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
                yield return null;
            }
            fadeCanvasGroup.alpha = 1;
        }

        // CLEARSceneへ移行（Inspectorで設定した clearSceneName を使用）
        SceneManager.LoadScene(clearSceneName);
    }
}