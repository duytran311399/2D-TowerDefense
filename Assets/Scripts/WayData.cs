using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "WaveData/ WaveData")]
public class WayData : ScriptableObject
{
    public List<int> enemies;
}
[System.Serializable]
public class SpawnEnemyData
{
}
