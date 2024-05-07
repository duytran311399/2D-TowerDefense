using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenGamePlay : ScreenLayer
{
    [SerializeField] private Text totalMoneyLabel;
    [SerializeField] private Text currentWaveLabel;
    [SerializeField] public Text totalEscapedLabel;
    [SerializeField] public Text timeCoundown;

    [SerializeField] private Button playButton;
    [SerializeField] private Text playButtonLabel;
    int WayCurrent
    {
        get { return GameManager.Instance.WayCurrent; }
    }
    int Escaped
    {
        get { return GameManager.Instance.TotalEnemyEscaped; }
    }

    public void Start()
    {
        playButton.onClick.AddListener(() => { 
            GameManager.Instance.SpawnNextWay();
            playButtonPressed();
        });
    }
    public void UpdateCoin(int totalMoney)
    {
        totalMoneyLabel.text = totalMoney.ToString();
    }
    public void UpdateEscaped(int escaped)
    {
        totalEscapedLabel.text = "Escaped: " + escaped + "/10";
    }

    public void playButtonPressed()
    {
        totalEscapedLabel.text = "Escaped: " + Escaped + "/10";
        SoundManager.Instance.Play(SoundManager.Instance.NewGame);
        currentWaveLabel.text = "Wave " + (WayCurrent + 1);
        if(GameManager.Instance.C_Spawn != null)
        {
            StopCoroutine(GameManager.Instance.C_Spawn);
        }
        playButton.gameObject.SetActive(false);
    }
    public void SetActivePlayButton(bool isActive)
    {
        playButton.gameObject.SetActive(isActive);
    }
    public IEnumerator TimeCountdown()
    {
        int timeCD = 5;
        while(timeCD > 0)
        {
            timeCoundown.text = timeCD.ToString();
            yield return new WaitForSeconds(1f);
        }
    }
    void Setup()
    {
        playButton.gameObject.SetActive(true);
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
