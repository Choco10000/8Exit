using UnityEngine;
using System.Collections.Generic;

public static class RankingManager // static ÉNÉâÉXÇ…Ç∑ÇÈ
{
    private const int MAX_RANKING_COUNT = 5;
    private const string RANKING_KEY_PREFIX = "RankingTime_";
    // Åö static Çí«â¡
    public static void SaveTime(float newTime)
    {
        List<float> times = GetRanking();
        times.Add(newTime);
        times.Sort();

        for (int i = 0; i < MAX_RANKING_COUNT; i++)
        {
            if (i < times.Count)
            {
                PlayerPrefs.SetFloat(RANKING_KEY_PREFIX + i, times[i]);
            }
        }
        PlayerPrefs.Save();
    }

    public static List<float> GetRanking()
    {
        List<float> times = new List<float>();
        for (int i = 0; i < MAX_RANKING_COUNT; i++)
        {
            if (PlayerPrefs.HasKey(RANKING_KEY_PREFIX + i))
            {
                times.Add(PlayerPrefs.GetFloat(RANKING_KEY_PREFIX + i));
            }
        }
        return times;
    }

    public static string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        int milliseconds = Mathf.FloorToInt((time * 100F) % 100F);
        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
}