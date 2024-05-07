using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum gameStatus
{
    next, play, gameover, win
}
public class GameManager : SingletonDontDestroyMono<GameManager> 
{
    [SerializeField] private int totalWaves;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private List<WaySpawnData> waySpawnDatas;
    [SerializeField] private Enemy[] enemies;

    private int waveCurrent = 0;
    private int totalMoney = 10;
    private int totalEnemyEscaped = 0;
    private int totalEscapedOnWave = 0;
    private int totalKilled = 0;
    private int totalKillEnemy = 0;

    public List<Enemy> EnemyList = new List<Enemy>();
    const float spawnDelay = 0.5f;
    public Coroutine C_Spawn;

    public int TotalEnemy
    {
        get { return waySpawnDatas[waveCurrent].spawnData.enemies.Count; }
    }
    public int TotalMoney
    {
        get { return totalMoney; }
        set
        {
            totalMoney = value;
            ScreenManager.Instance.SL_GamePlay.UpdateCoin(totalMoney);
        }
    }
    public int WayCurrent
    {
        get { return waveCurrent; }
    }
    public int TotalEscape
    {
        get { return totalEscapedOnWave; }
        set 
        { 
            totalEscapedOnWave = value;
            ScreenManager.Instance.SL_GamePlay.UpdateEscaped(totalEscapedOnWave + totalEnemyEscaped);
        }
    }
    public int TotalEnemyEscaped
    {
        get { return totalEnemyEscaped; }
    }
    //public int RoundEscaped
    //{
    //    get { return roundEscaped; }
    //    set { roundEscaped = value; }
    //}
    public int TotalKilled
    {
        get { return totalKilled; }
        set { totalKilled = value; }
    }
    public int TotalKillEnemy
    {
        get { return totalKillEnemy; }
    }

    // Use this for initialization
    void Start () {
        totalWaves = waySpawnDatas.Count;
        //audioSource = GetComponent<AudioSource>();
        ShowMenu();
	}
	
	// Update is called once per frame
	void Update () {
        handleEscape();
	}
    public void LoadLevel(int Level)
    {
        SceneManager.LoadScene(Level, LoadSceneMode.Single);
        ScreenManager.Instance.CloseAllScreen();
        ScreenManager.Instance.SL_GamePlay.Open();
    }
    //This will spawn enemies, wait for the given spawnDelay then call itself again to spawn another enemy
    IEnumerator spawn()
    {
        if (EnemyList.Count < TotalEnemy)
        {
            for (int i = 0; i < TotalEnemy; i++)
            {
                Enemy newEnemy = Instantiate(enemies[waySpawnDatas[waveCurrent].GetEnemy(i)]);
                newEnemy.transform.position = spawnPoint.transform.position;
                RegisterEnemy(newEnemy);
                yield return new WaitForSeconds(spawnDelay);
            }
        }
        while(TotalEnemy > TotalKilled + TotalEscape)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("Check Kill All");
        }
        //Debug.Log(TotalEnemy + " == " + TotalEscape + " + " + TotalKilled);
        IsWaveOver();
    }

    ///Register - when enemy spawns
    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }
    ///Unregister - When they escape the screen
    public void UnregisterEnemy()
    {
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

    public void IsWaveOver()
    {
        waveCurrent++;
        ShowMenu();
    }
    public void ShowMenu()
    {
        if (TotalEscape == 10)
        {
            ScreenManager.Instance.SL_MainMenu.SetupLose();
            ScreenManager.Instance.SL_GamePlay.Close();
        }
        else if(waveCurrent == totalWaves)
        {
            ScreenManager.Instance.SL_MainMenu.SetupVictory();
            ScreenManager.Instance.SL_GamePlay.Close();
        }
        else
            ScreenManager.Instance.SL_GamePlay.SetActivePlayButton(true);
    }
    public void SpawnNextWay()
    {
        totalEnemyEscaped += TotalEscape;
        totalKillEnemy += TotalKilled;
        TotalEscape = 0;
        TotalKilled = 0;
        UnregisterEnemy();
        StartCoroutine(spawn());
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
