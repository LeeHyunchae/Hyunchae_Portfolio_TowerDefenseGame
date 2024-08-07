using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuildController
{
    private TowerManager towerManager;
    private TowerController originTowerPrefab;
    private Transform towerParent;

    public void Init()
    {
        towerManager = TowerManager.getInstance;
        originTowerPrefab = Resources.Load<TowerController>("Prefabs/Tower");
    }

    public void CreateTower(List<int> _buildableTileList)
    {
        if(_buildableTileList == null)
        {
            return;
        }

        if (towerParent != null)
        {
            GameObject.Destroy(towerParent);
        }

        towerParent = new GameObject("Towers").transform;


        MapData mapData = MapManager.getInstance.GetMapData;

        List<int> buildableTileList = _buildableTileList;

        int count = buildableTileList.Count;

        int mapWidth = mapData.wid;

        for(int i = 0; i < count; i++)
        {
            int tileIndex = buildableTileList[i];

            int posX = tileIndex % mapWidth;
            int posY = tileIndex / mapWidth;

            TowerController towerController = GameObject.Instantiate<TowerController>(originTowerPrefab, towerParent);
            towerController.transform.position = new Vector2(posX, posY);

            towerManager.AddTowerController(tileIndex, towerController);
        }
    }

    public void OnClickBuildableTile(int _tileIndex)
    {
        TowerController tower = towerManager.GetTower(_tileIndex);

        Debug.Log(tower.transform.position);
    }
}
