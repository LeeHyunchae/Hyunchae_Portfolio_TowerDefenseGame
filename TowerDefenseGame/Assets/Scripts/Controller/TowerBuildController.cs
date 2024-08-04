using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuildController
{
    private Dictionary<int, TowerController> towerDict = new Dictionary<int, TowerController>();

    private MapData mapData;

    public void SetMapData(MapData _mapData)
    {
        mapData = _mapData;
    }
}
