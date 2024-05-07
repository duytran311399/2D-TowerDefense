using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMainMenu : ScreenLayer
{
    [Header("Panel Main")]
    [SerializeField] private Transform panelMain;
    [SerializeField] private Button btnLevel;
    [SerializeField] private Transform panelGameOver;

    [Header("Panel Victory")]
    [SerializeField] private Transform panelVictory;
    [Header("Panel Lose")]
    [SerializeField] private Transform panelLose;

    [Header("Score")] [Space(10)]
    [SerializeField] private Button btnNext;
    [SerializeField] private Text wave;
    [SerializeField] private Text killer;

    private void Start()
    {
        btnNext.onClick.AddListener(() => { Setup(); });
        btnLevel.onClick.AddListener(() => { ScreenManager.Instance.SL_LevelSellector.Open(); Close();});
    }
    public void SetupVictory()
    {
        Open();
        panelGameOver.gameObject.SetActive(true);
        panelVictory.gameObject.SetActive(true);
        panelLose.gameObject.SetActive(false);
        wave.text = GameManager.Instance.WayCurrent.ToString();
        killer.text = GameManager.Instance.TotalKillEnemy.ToString();
    }
    public void SetupLose()
    {
        Open();
        panelGameOver.gameObject.SetActive(true);
        panelVictory.gameObject.SetActive(false);
        panelLose.gameObject.SetActive(true);
        wave.text = GameManager.Instance.WayCurrent.ToString();
        killer.text = GameManager.Instance.TotalKillEnemy.ToString();
    }

    public void Setup()
    {
        panelGameOver.gameObject.SetActive(false);
        panelMain.gameObject.SetActive(true);
    }
    public override void Close()
    {
        base.Close();
    }

    public override void Open()
    {
        base.Open();
        Setup();
    }
}
