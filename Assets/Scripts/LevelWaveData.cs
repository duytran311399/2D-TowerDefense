using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelData/Level", fileName ="Level_")]
public class LevelWaveData : ScriptableObject
{
    public int cointStart;
    public List<WayData> wayDatas;

    public int GetTotalEnemyOnWaveCurrent(int waveCurrent)
    {
        return wayDatas[waveCurrent].enemies.Count;
    }
    public int GetEnemy(int wayCurrent, int i) { return wayDatas[wayCurrent].enemies[i]; }
}
