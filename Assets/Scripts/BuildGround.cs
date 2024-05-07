using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGround : MonoBehaviour
{
    public Tower towerBuild;
    public bool IsBuilded
    {
        get { return towerBuild != null; }
    }
    public Tower GetTowerOnSide()
    {
        return towerBuild;
    }
    public void RegeterBuilderTower(Tower tower)
    {
        towerBuild = tower;
    }
    public void OnDestroyTower()
    {
        if (towerBuild != null)
        {
            Destroy(towerBuild.gameObject);
            towerBuild = null;
        }
    }
}
