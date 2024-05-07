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

    Coroutine C_TimeCountDown;
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
            PlayNextWave();
            SoundManager.Instance.ButtonClickSound();
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

    public void PlayNextWave()
    {
        if(C_TimeCountDown != null) { StopCoroutine(C_TimeCountDown); }
        GameManager.Instance.SpawnNextWay();
        totalEscapedLabel.text = "Escaped: " + Escaped + "/10";
        SoundManager.Instance.Play(SoundManager.Instance.NewGame);
        currentWaveLabel.text = "Wave " + (WayCurrent + 1);
        timeCoundown.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
    }
    public void SetActivePlayButton(bool isActive)
    {
        playButton.gameObject.SetActive(isActive);
    }
    public void PlayTimeCountDown()
    {
        C_TimeCountDown = StartCoroutine(TimeCountDown());
    }
    IEnumerator TimeCountDown()
    {
        int timeCD = 5;
        timeCoundown.gameObject.SetActive(true);
        while (timeCD > 0)
        {
            timeCoundown.text = timeCD.ToString();
            yield return new WaitForSeconds(1f);
            timeCD--;
        }
        PlayNextWave();
    }
    void Setup()
    {
        timeCoundown.gameObject.SetActive(false);
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
