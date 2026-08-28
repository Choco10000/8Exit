using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int currentPoints = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddPoints(int amount)
    {
        currentPoints += amount;
        Debug.Log($"<color=green>【ポイント獲得】</color> +{amount} pt （現在の合計: {currentPoints} pt）");
    }

    //ポイントを0にリセットする関数
    public void ResetPoints()
    {
        currentPoints = 0;
        Debug.Log("<color=red>【ポイントリセット】</color> ");
    }

    public int GetCurrentPoints()
    {
        return currentPoints;
    }
}