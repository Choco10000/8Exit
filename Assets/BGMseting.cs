using UnityEngine;
using TMPro;

public class BGMseting : MonoBehaviour
{
    [Header("ランキングUIの親オブジェクト（Panelなど）")]
    [SerializeField] private GameObject BGMPanel;
    void Start()
    {
        if (BGMPanel != null)
        {
            BGMPanel.SetActive(false);
        }
    }

    public void StartBGM()
    {
        BGMPanel.SetActive(true);
    }
    public void CloseBGM()
    {
        if (BGMPanel != null)
        {
            BGMPanel.SetActive(false); //パネルを非表示
        }
    }
}

