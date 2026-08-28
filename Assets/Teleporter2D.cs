using System.Collections;
using UnityEngine;

public class Teleporter2D : MonoBehaviour
{
    [System.Serializable]
    public class TeleportDestinationInfo
    {
        public string name = "出口の名称";
        public Transform destinationTransform;
        public bool givesPoints = false;

        [Header("出現確率")]
        [Range(0, 100)] public int weight = 10;
    }

    [Header("テレポート先の候補（複数登録）")]
    [SerializeField] private TeleportDestinationInfo[] teleportDestinations;

    [Header("条件達成時：特定のテレポート先")]
    [SerializeField] private Transform specialDestination;

    [Header("条件：必要なポイント数")]
    [SerializeField] private int requiredPoints = 50;

    [Header("通常ワープ（条件未達成）のときもポイントをリセットする？")]
    [SerializeField] private bool resetPointsOnNormalWarp = false;

    [Header("当たりワープで獲得するポイント量")]
    [SerializeField] private int pointsToGive = 10;

    [Header("テレポート対象のタグ")]
    [SerializeField] private string targetTag = "Player";

    [Header("フェード用のCanvasGroup")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    [Header("フェードにかける時間（秒）")]
    [SerializeField] private float fadeDuration = 1.0f;

    [Header("フェードイン後の硬直時間（秒）")]
    [SerializeField] private float postFadeInDelay = 1.5f;

    [Header("★効果音（SE）設定")]
    [SerializeField] private AudioClip teleportStartSE;
    [SerializeField] private AudioClip teleportEndSE;
    [Range(0f, 1f)][SerializeField] private float seVolume = 1.0f;

    private AudioSource audioSource; //AudioSourceコンポーネント用
    private bool isTeleporting = false;

    private void Awake()
    {
        //AudioSourceを取得。無ければ自動でアタッチして2D設定にする
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        //2Dサウンドとして再生するための設定
        audioSource.spatialBlend = 0f;
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag) && !isTeleporting)
        {
            if (fadeCanvasGroup != null)
            {
                StartCoroutine(TeleportSequence(collision));
            }
        }
    }

    private IEnumerator TeleportSequence(Collider2D playerCollider)
    {
        isTeleporting = true;
        Transform playerTransform = playerCollider.transform;

        //テレポート開始時の効果音
        PlaySE(teleportStartSE);

        //物理速度をゼロにする
        Rigidbody2D rb = playerCollider.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        //PlayerMovementスクリプトを一時停止
        PlayerMovement playerMovementScript = playerCollider.GetComponent<PlayerMovement>();
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        //フェードアウト
        yield return StartCoroutine(Fade(0, 1));

        int currentPoints = GameManager.Instance != null ? GameManager.Instance.GetCurrentPoints() : 0;
        Transform chosenTransform = null;
        bool walkedIntoSpecial = false;
        bool isLuckyDestination = false;

        //条件分岐
        if (currentPoints >= requiredPoints && specialDestination != null)
        {
            chosenTransform = specialDestination;
            walkedIntoSpecial = true;
        }
        else
        {
            if (teleportDestinations != null && teleportDestinations.Length > 0)
            {
                TeleportDestinationInfo selectedInfo = GetWeightedRandomDestination();
                if (selectedInfo != null)
                {
                    chosenTransform = selectedInfo.destinationTransform;
                    isLuckyDestination = selectedInfo.givesPoints;
                }
            }
        }

        //プレイヤーの座標を移動
        if (chosenTransform != null)
        {
            playerTransform.position = chosenTransform.position;

            if (GameManager.Instance != null)
            {
                if (walkedIntoSpecial)
                {
                    GameManager.Instance.ResetPoints();
                }
                else
                {
                    if (isLuckyDestination)
                    {
                        GameManager.Instance.AddPoints(pointsToGive);
                    }
                    else if (resetPointsOnNormalWarp)
                    {
                        GameManager.Instance.ResetPoints();
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.2f);

        //フェードイン
        yield return StartCoroutine(Fade(1, 0));

        //テレポート出現時の効果音
        PlaySE(teleportEndSE);

        //明るくなった後の硬直時間
        yield return new WaitForSeconds(postFadeInDelay);

        //移動スクリプトを再開
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }

        isTeleporting = false;
    }

    //2D音量でしっかり鳴らす再生メソッド
    private void PlaySE(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, seVolume);
        }
    }

    private TeleportDestinationInfo GetWeightedRandomDestination()
    {
        int totalWeight = 0;
        foreach (var dest in teleportDestinations)
        {
            totalWeight += dest.weight;
        }

        if (totalWeight <= 0) return teleportDestinations[0];

        int randomValue = Random.Range(0, totalWeight);
        foreach (var dest in teleportDestinations)
        {
            if (randomValue < dest.weight)
            {
                return dest;
            }
            randomValue -= dest.weight;
        }

        return teleportDestinations[0];
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = endAlpha;
    }
}