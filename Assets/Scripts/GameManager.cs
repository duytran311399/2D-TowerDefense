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
    public UserData userData;
    [SerializeField] private int totalWaves;
    [SerializeField] private GameObject spawnPoint;
    //[SerializeField] private List<WayData> levelWayData;
    [SerializeField] private List<LevelWaveData> levelWayDatas;
    public LevelWaveData LevelDataCurrent
    {
        get { return levelWayDatas[levelCurrent]; }
    }
    [SerializeField] private Enemy[] enemies;

    public int levelCurrent;

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
        get { return LevelDataCurrent.GetTotalEnemyOnWaveCurrent(waveCurrent); }
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
    public int TotalEscapeOnWave
    {
        get { return totalEscapedOnWave; }
        set 
        { 
            totalEscapedOnWave = value;
            ScreenManager.Instance.SL_GamePlay.UpdateEscaped(totalEnemyEscaped);
        }
    }
    public int TotalEnemyEscaped
    {
        get { return totalEnemyEscaped; }
        set { totalEnemyEscaped = value; }
    }
    //public int RoundEscaped
    //{
    //    get { return roundEscaped; }
    //    set { roundEscaped = value; }
    //}
    public int TotalKilledOnWave
    {
        get { return totalKilled; }
        set { totalKilled = value; }
    }
    public int TotalKillEnemy
    {
        get { return totalKillEnemy; }
        set { totalKillEnemy = value; }
    }
    void Start ()
    {
        LoadUserData();
        ShowMenu();
	}
	void Update () {
        handleEscape();
    }
    void LoadUserData()
    {
        userData = ES3.Load<UserData>("userData", "userData.data", new UserData());
    }
    public void SaveUserData()
    {
        if (userData != null)
            ES3.Save("userData", userData, "userData.data");
    }
    public void LoadLevel(int Level)
    {
        levelCurrent = Level - 1;
        ReloadLevel();
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
                Enemy newEnemy = Instantiate(enemies[LevelDataCurrent.GetEnemy(waveCurrent, i)]);
                newEnemy.transform.position = spawnPoint.transform.position;
                RegisterEnemy(newEnemy);
                yield return new WaitForSeconds(spawnDelay);
            }
        }
        while(TotalEnemy > TotalKilledOnWave + TotalEscapeOnWave)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("Check Kill All");
            if(TotalEnemyEscaped == 10)
            {
                Debug.Log("Losssssssss");
                ScreenManager.Instance.SL_MainMenu.SetupLose();
                ScreenManager.Instance.SL_GamePlay.Close();
                break;
            }
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
        if (userData.hightKiller < TotalKillEnemy) { userData.hightKiller = TotalKillEnemy; }
        if (TotalEnemyEscaped == 10)
        {
            ScreenManager.Instance.SL_MainMenu.SetupLose();
            ScreenManager.Instance.SL_GamePlay.Close();
            userData.totalLose++;
        }
        else if(waveCurrent == totalWaves)
        {
            ScreenManager.Instance.SL_MainMenu.SetupVictory();
            ScreenManager.Instance.SL_GamePlay.Close();
            userData.totalWin++;
        }
        else
            ScreenManager.Instance.SL_GamePlay.SetActivePlayButton(true);
        SaveUserData();
    }
    public void SpawnNextWay()
    {
        TotalEscapeOnWave = 0;
        TotalKilledOnWave = 0;
        UnregisterEnemy();
        StartCoroutine(spawn());
    }
    public void ReloadLevel()
    {
        totalWaves = LevelDataCurrent.wayDatas.Count;
        waveCurrent = 0;
        totalMoney = LevelDataCurrent.cointStart;
        totalEnemyEscaped = 0;
        totalKillEnemy = 0;
        totalKilled = 0;
        totalEscapedOnWave = 0;
        UnregisterEnemy();
    }
    private void handleEscape()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if(TowerManager.Instance != null)
            {
                TowerManager.Instance.disableDragSprite();
                TowerManager.Instance.towerButtonPressed = null;
            }
        }
    }

}
