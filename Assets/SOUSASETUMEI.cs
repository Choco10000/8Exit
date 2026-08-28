using UnityEngine;
using TMPro;
public class SOUSASETUMEI : MonoBehaviour
{
    [Header("ランキングUIの親オブジェクト（Panelなど）")]
    [SerializeField] private GameObject SOUSAPanel;
    void Start()
    {
        if (SOUSAPanel != null)
        {
            SOUSAPanel.SetActive(false);
        }
    }

    public void StartSOUSA()
    {
        SOUSAPanel.SetActive(true);
    }
    public void CloseSOUSA()
    {
        if (SOUSAPanel != null)
        {
            SOUSAPanel.SetActive(false); //パネルを非表示
        }
    }
}
