using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ETileType
{
    HORIZONTAL = 0,
    VERTICAL = 1,
    UP_RIGHT = 2,
    UP_LEFT = 3,
    DOWN_LEFT = 4,
    DOWN_RIGHT = 5,
    DIAGONAL = 6,
    BUILDABLE = 7,
    SPAWN = 8,
    GOAL = 9,
    EMPTY = 10,
    END
}

public class MapManager : Singleton<MapManager>
{
    private const string TILESPRITEPATH = "Sprites/TempImage/Tile_";
    private Dictionary<ETileType, Sprite> tileSpriteDict = new Dictionary<ETileType, Sprite>();

    public override void Init()
    {
        LoadTileSprite();
        base.Init();
    }

    private void LoadTileSprite()
    {
        int count = (int)ETileType.END;

        for(int i = 0; i <count; i ++)
        {
            Sprite tileSprite = Resources.Load<Sprite>(TILESPRITEPATH + i);
            tileSpriteDict.Add((ETileType)i, tileSprite);
        }
    }

    public Sprite GetTileSprite(ETileType _tileType)
    {
        tileSpriteDict.TryGetValue(_tileType, out Sprite tileSprite);

        if(tileSprite == null)
        {
            Debug.Log("TileSprite is Null");
        }

        return tileSprite;
    }
}
