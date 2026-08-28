using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TitleRankingDisplay : MonoBehaviour
{
    [Header("ランキングUIの親オブジェクト（Panelなど）")]
    [SerializeField] private GameObject rankingPanel; //ランキング画面全体を包むオブジェクト

    [Header("1位〜5位のテキストUI")]
    [SerializeField] private TextMeshProUGUI rank1Text;
    [SerializeField] private TextMeshProUGUI rank2Text;
    [SerializeField] private TextMeshProUGUI rank3Text;
    [SerializeField] private TextMeshProUGUI rank4Text;
    [SerializeField] private TextMeshProUGUI rank5Text;
    
    void Start()
    {
        //起動時はランキングパネルを非表示にしておく
        if (rankingPanel != null)
        {
            rankingPanel.SetActive(false);
        }
    }

    //RANKINGボタンを押したときに呼び出すメソッド
    public void OpenRanking()
    {
        DisplayRanking(); //ランキングデータを更新表示
        if (rankingPanel != null)
        {
            rankingPanel.SetActive(true); //パネルを表示
        }
    }

    //CLOSEボタンを押したときに呼び出すメソッド
    public void CloseRanking()
    {
        if (rankingPanel != null)
        {
            rankingPanel.SetActive(false); //パネルを非表示
        }
    }

    //ランキングデータをテキストに反映する処理
    private void DisplayRanking()
    {
        List<float> ranking = RankingManager.GetRanking();
        TextMeshProUGUI[] rankTexts = { rank1Text, rank2Text, rank3Text, rank4Text, rank5Text };

        for (int i = 0; i < rankTexts.Length; i++)
        {
            if (rankTexts[i] == null) continue;

            int rankNumber = i + 1; //順位
            string suffix = GetOrdinalSuffix(rankNumber); //st, nd, rd, th の取得

            if (i < ranking.Count)
            {
                rankTexts[i].text = $"{rankNumber}{suffix} : {RankingManager.FormatTime(ranking[i])}";
            }
            else
            {
                rankTexts[i].text = $"{rankNumber}{suffix} : --:--.--";
            }
        }
    }

    //順位に応じた接尾辞を返すヘルパー関数
    private string GetOrdinalSuffix(int rank)
    {
        switch (rank % 10)
        {
            case 1: return "st";
            case 2: return "nd";
            case 3: return "rd";
            default: return "th";
        }
    }
}