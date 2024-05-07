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
    private int TotalWavesCurrent   // Tổng số wave trong level hiện tại
    {
        get { return LevelDataCurrent.wayDatas.Count; }
    }
    [SerializeField] private GameObject spawnPoint;
    //[SerializeField] private List<WayData> levelWayData;
    [SerializeField] private List<LevelWaveData> levelWayDatas; // Level Data (Chứa danh sách các level)
    public LevelWaveData LevelDataCurrent   // Level hiện tại
    {
        get { return levelWayDatas[levelCurrent]; }
    }
    [SerializeField] private Enemy[] enemies;   // Enemy Object list

    public int levelCurrent;    // Level hiện tại

    private int waveCurrent = 0;    // wave hiện tại
    private int totalMoney = 10;    // Tiền
    private int totalEnemyEscaped = 0;  // Tổng số enemy chạy thoát
    private int totalEscapedOnWave = 0; // Tổng số enemy chạy thoát trong wave hiện tại
    private int totalKilledOnWave = 0;  // Tổng số enemy đã hạ ở wave hiện tại      
    private int totalKillEnemy = 0; // Tổng số enemy đã hạ ở level hiện tại

    public List<Enemy> EnemyList = new List<Enemy>();   // EnemyList ở wave hiện tại
    const float spawnDelay = 0.5f;  // thời gian giữa các enemy đc spawn

    public int TotalEnemyOnWave
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
        get { return totalKilledOnWave; }
        set { totalKilledOnWave = value; }
    }
    public int TotalKillEnemy
    {
        get { return totalKillEnemy; }
        set { totalKillEnemy = value; }
    }
    void Start ()
    {
        LoadUserData();
        CheckStateGame();
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
        levelCurrent = Level - 1;   // Level 1 ứng vs level đầu tiên trong list LevelData => levelCurrent = 0
        ReloadLevel();
        SceneManager.LoadScene(Level, LoadSceneMode.Single);    // Load Scene 1 (trong build setting)
        ScreenManager.Instance.CloseAllScreen();
        ScreenManager.Instance.SL_GamePlay.Open();
    }
    //This will spawn enemies, wait for the given spawnDelay then call itself again to spawn another enemy
    IEnumerator spawn()
    {
        for (int i = 0; i < TotalEnemyOnWave; i++)  // Spawn Enemy 
        {
            Enemy newEnemy = Instantiate(enemies[LevelDataCurrent.GetEnemy(waveCurrent, i)]);       // Tạo đối tượng Enemy
            newEnemy.transform.position = spawnPoint.transform.position;                            
            RegisterEnemy(newEnemy);                                                                // Đăng ký vào danh sách Enemy
            yield return new WaitForSeconds(spawnDelay);                                            // Đợi sau spawnDelay rồi tiếp tục vòng lặp
        }
        while (TotalEnemyOnWave > TotalKilledOnWave + TotalEscapeOnWave)   // Check điều kiện lặp
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("Check Kill All");
            if(TotalEnemyEscaped == 10)                 // Khi tổng enemy chạy thoát = 10 (10 đứa chạy thoát)
            {
                Debug.Log("Lose State");                // Thua
                ScreenManager.Instance.SL_MainMenu.SetupLose();
                ScreenManager.Instance.SL_GamePlay.Close();
                SoundManager.Instance.Play(SoundManager.Instance.Gameover);
                break;
            }
        }
        IsWaveOver();                                   // Wave kêt thúc
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
        CheckStateGame();
    }
    public void CheckStateGame()
    {
        if (userData.hightKiller < TotalKillEnemy) { userData.hightKiller = TotalKillEnemy; }
        if (TotalEnemyEscaped == 10)
        {
            ScreenManager.Instance.SL_MainMenu.SetupLose();
            ScreenManager.Instance.SL_GamePlay.Close();
            userData.totalLose++;
        }
        else if(waveCurrent == TotalWavesCurrent)
        {
            ScreenManager.Instance.SL_MainMenu.SetupVictory();
            ScreenManager.Instance.SL_GamePlay.Close();
            userData.totalWin++;
        }
        else
        {
            ScreenManager.Instance.SL_GamePlay.SetActivePlayButton(true);
            ScreenManager.Instance.SL_GamePlay.PlayTimeCountDown();
        }
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
        waveCurrent = 0;
        totalMoney = LevelDataCurrent.cointStart;
        totalEnemyEscaped = 0;
        totalKillEnemy = 0;
        totalKilledOnWave = 0;
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
