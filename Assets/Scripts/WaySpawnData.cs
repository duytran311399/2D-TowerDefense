using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "WaveData/ WaveData")]
public class WaySpawnData : ScriptableObject
{
    public SpawnEnemyData spawnData;

    public int GetEnemy(int i)
    {
        return spawnData.enemies[i];
    }
}
[System.Serializable]
public class SpawnEnemyData
{
    public List<int> enemies;
}
