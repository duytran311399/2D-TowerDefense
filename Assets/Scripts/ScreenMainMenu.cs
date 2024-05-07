using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMainMenu : ScreenLayer
{
    [Header("Panel Main")]
    [SerializeField] private Transform panelMain;
    [SerializeField] private Button btnLevel;

    [Header("Panel Victory")]
    [SerializeField] private Transform panelVictory;
    [SerializeField] private Button btnNext;
    [SerializeField] private Text wave;
    [SerializeField] private Text killer;

    [Header("Panel Lose")]
    [SerializeField] private Transform panelLose;

    private void Start()
    {
        btnNext.onClick.AddListener(() => { ClosePanelVictory(); });
        btnLevel.onClick.AddListener(() => { ScreenManager.Instance.SL_LevelSellector.Open(); Close(); Debug.Log("ssssss"); });
    }
    public void SetupVictory()
    {
        Open();
        panelVictory.gameObject.SetActive(true);
        panelLose.gameObject.SetActive(false);
        wave.text = GameManager.Instance.WayCurrent.ToString();
        killer.text = GameManager.Instance.TotalKillEnemy.ToString();
    }
    public void SetupLose()
    {
        Open();
        panelVictory.gameObject.SetActive(false);
        panelLose.gameObject.SetActive(true);
        wave.text = GameManager.Instance.WayCurrent.ToString();
        killer.text = GameManager.Instance.TotalKillEnemy.ToString();
    }
    void ClosePanelVictory()
    {
        panelVictory.gameObject.SetActive(false);
        panelMain.gameObject.SetActive(true);
    }
    public override void Close()
    {
        base.Close();
    }

    public override void Open()
    {
        base.Open();
    }
}
