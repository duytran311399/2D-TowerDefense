using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "WaveData/ WaveData")]
public class WaySpawnData : ScriptableObject
{
    public SpawnEnemyData spawnData;
}
[System.Serializable]
public class SpawnEnemyData
{
    public List<int> enemies;
}
