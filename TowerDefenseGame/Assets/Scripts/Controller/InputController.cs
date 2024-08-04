using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController
{
    private MapData mapData;
    private bool isPlayGame;
    private float mapWidth;
    private float mapHeight;

    public Action<int> OnClickBuildableTileAction;

    public void SetMapData(MapData _mapData)
    {
        mapData = _mapData;
        mapWidth = mapData.wid;
        mapHeight = (mapData.tiles.Length / mapData.wid);

    }

    public void SetIsPlayGame(bool _isPlay) => isPlayGame = _isPlay;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isPlayGame || mapData.tiles == null)
            {
                return;
            }

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int posX = Mathf.RoundToInt(mousePosition.x);
            int posY = Mathf.RoundToInt(mousePosition.y);

            if (posX >= mapWidth || posX < 0 || posY >= mapHeight || posY < 0)
            {
                return;
            }

            int tilelIndex = posY * mapData.wid + posX;

            if (mapData.tiles[tilelIndex].buildable)
            {
                Debug.Log("Buildable Tile");
                OnClickBuildableTileAction?.Invoke(tilelIndex);
            }
        }
    }
}
