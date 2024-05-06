using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gameStatus
{
    next, play, gameover, win
}
public class GameManager : SingletonDontDestroyMono<GameManager> 
{
    [SerializeField] private int totalWaves;
    [SerializeField] private Text totalMoneyLabel;
    [SerializeField] private Text currentWaveLabel;
    [SerializeField] private Text totalEscapedLabel;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private List<WaySpawnData> waySpawnDatas;
    [SerializeField] private Enemy[] enemies;

    [SerializeField] private Text playButtonLabel;
    [SerializeField] private Button playButton;

    private int waveCounter = 0;
    private int waveCurrent = 0;
    private int totalMoney = 10;
    //private int totalEscaped = 0;
    //private int roundEscaped = 0;
    //private int totalKilled = 0;
    //private int enemiesToSpawn = 0;
    private gameStatus currentState = gameStatus.play;
    private AudioSource audioSource;

    public List<Enemy> EnemyList = new List<Enemy>();
    const float spawnDelay = 2f; //Spawn Delay in seconds

    public int TotalMoney
    {
        get { return totalMoney; }
        set
        {
            totalMoney = value;
            totalMoneyLabel.text = totalMoney.ToString();
        }
    }
    //public int TotalEscape
    //{
    //    get { return totalEscaped; }
    //    set { totalEscaped = value; }
    //}
    //public int RoundEscaped
    //{
    //    get { return roundEscaped; }
    //    set { roundEscaped = value; }
    //}
    //public int TotalKilled
    //{
    //    get { return totalKilled; }
    //    set { totalKilled = value; }
    //}
    public AudioSource AudioSource
    {
        get { return audioSource; }
    }
    
    // Use this for initialization
    void Start () {
        totalWaves = waySpawnDatas.Count;
        playButton.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        ShowMenu();
	}
	
	// Update is called once per frame
	void Update () {
        handleEscape();
	}

    //This will spawn enemies, wait for the given spawnDelay then call itself again to spawn another enemy
    IEnumerator spawn()
    {
        if (EnemyList.Count < waySpawnDatas[waveCurrent].spawnData.enemies.Count)
        {
            for (int i = 0; i < waySpawnDatas[waveCurrent].spawnData.enemies.Count; i++)
            {
                Enemy newEnemy = Instantiate(enemies[waySpawnDatas[waveCurrent].spawnData.enemies[i]]);
                newEnemy.transform.position = spawnPoint.transform.position;
                RegisterEnemy(newEnemy);
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        isWaveOver();
    }

    ///Register - when enemy spawns
    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }
    ///Unregister - When they escape the screen
    public void UnregisterEnemy()
    {
        //EnemyList.Remove(enemy);
        //Destroy(enemy.gameObject);
        EnemyList.Clear();
    }
    ///Destroy - At the end of the wave
    public void DestroyAllEnemies()
    {
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

    public void AddMoney(int amount)
    {
        TotalMoney += amount;
    }

    public void SubtractMoney(int amount)
    {
        TotalMoney -= amount;
    }

    public void isWaveOver()
    {
        totalEscapedLabel.text = "Wave Complete " + waveCounter + "/10";
        waveCurrent++;
        setCurrentGameState();
        ShowMenu();
    }

    public void setCurrentGameState()
    {
        if(waveCounter >= 10)
        {
            currentState = gameStatus.gameover;
        }
        else if(waveCounter == 0)
        {
            currentState = gameStatus.play;
        }
        else if(waveCounter >= totalWaves)
        {
            currentState = gameStatus.win;
        }
        else
        {
            currentState = gameStatus.next;
        }
    }

    public void ShowMenu()
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                playButtonLabel.text = "Play Again!";
                AudioSource.PlayOneShot(SoundManager.Instance.Gameover);
                break;
            case gameStatus.next:
                playButtonLabel.text = "Next Wave";
                break;
            case gameStatus.play:
                playButtonLabel.text = "Play";
                break;
            case gameStatus.win:
                playButtonLabel.text = "Play";
                break;
        }
        if (waveCurrent == totalWaves)
            Debug.Log("Win");
        else
            playButton.gameObject.SetActive(true);
    }
    public void playButtonPressed()
    {
        Debug.Log("Play Button Pressed");
        UnregisterEnemy();
        totalEscapedLabel.text = "Wave: " + waveCounter + "/10";
        AudioSource.PlayOneShot(SoundManager.Instance.NewGame);
        currentWaveLabel.text = "Wave " + (waveCounter + 1);
        StartCoroutine(spawn());
        waveCounter += 1;
        playButton.gameObject.SetActive(false);
    }
    private void handleEscape()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TowerManager.Instance.disableDragSprite();
            TowerManager.Instance.towerButtonPressed = null;
        }
    }

}
