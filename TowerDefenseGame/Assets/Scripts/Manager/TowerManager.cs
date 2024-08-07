using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    private Dictionary<int, TowerController> towerDict = new Dictionary<int, TowerController>();

    public override void Init()
    {
        base.Init();
    }

    public void AddTowerController(int _tileIndex, TowerController _tower)
    {
        if(towerDict.ContainsKey(_tileIndex))
        {
            Debug.Log("Tile Key Duplicated");
            return;
        }

        towerDict.Add(_tileIndex, _tower);
    }

    public TowerController GetTower(int _tileIndex)
    {
        towerDict.TryGetValue(_tileIndex, out TowerController tower);

        if(tower == null)
        {
            Debug.Log("Wrong Tile Index");
        }

        return tower;
    }
}
