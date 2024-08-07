using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator
{
    private SpriteRenderer originTile;
    private MapManager mapManager;

    private Transform gridParent;

    public void Init()
    {
        originTile = Resources.Load<SpriteRenderer>("Prefabs/BaseTile");
        mapManager = MapManager.getInstance;
    }

    public List<int> CreateMap()
    {
        MapData mapData = mapManager.GetMapData;

        List<int> buildableTileIndexList = new List<int>();

        if(gridParent != null)
        {
            GameObject.Destroy(gridParent);
        }

        gridParent = new GameObject("Map").transform;

        int count = mapData.tiles.Length;
        int height = 0;

        for(int i = 0; i < count; i ++)
        {
            SpriteRenderer tileSprite = GameObject.Instantiate(originTile,gridParent);

            int posX = i % 10;

            if (i != 0 && i % 10 == 0)
            {
                height++;
            }

            tileSprite.transform.position = new Vector2(posX, height);

            ETileType tileType = (ETileType)mapData.tiles[i].imageIdx;

            tileSprite.sprite = mapManager.GetTileSprite(tileType);

            if(mapData.tiles[i].buildable)
            {
                buildableTileIndexList.Add(i);
            }

        }

        return buildableTileIndexList;
    }
}
