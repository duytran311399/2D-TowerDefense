using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMainMenu : ScreenLayer
{
    [Header("Panel Victory")]
    [SerializeField] private Transform panelVictory;
    [SerializeField] private Button btnNext;
    [SerializeField] private Text wave;
    [SerializeField] private Text killer;
    //[Header("Panel Lose")]

    private void Start()
    {
        btnNext.onClick.AddListener(() => { Close(); });
    }
    public void SetupVictory()
    {
        Open();
        panelVictory.gameObject.SetActive(true);
        wave.text = GameManager.Instance.WayCurrent.ToString();
        killer.text = GameManager.Instance.TotalKillEnemy.ToString();
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
