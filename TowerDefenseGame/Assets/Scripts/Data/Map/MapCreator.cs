using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    [SerializeField] private GameObject originTilePrefab;
    [SerializeField] private Sprite[] sprites;

    private MapData mapData;

    private GameObject gridParent;

    public void SetMapData(MapData _mapData)
    {
        mapData = _mapData;
    }

    public MapData GetMapData => mapData;

    public void CreateMap()
    {
        if(gridParent != null)
        {
            Destroy(gridParent);
        }

        gridParent = new GameObject("Map");

        int count = mapData.tiles.Length;
        int height = 0;

        for(int i = 0; i < count; i ++)
        {
            SpriteRenderer tileSprite = Instantiate<GameObject>(originTilePrefab, gridParent.transform).GetComponent<SpriteRenderer>();

            int posX = i % 10;

            if (i != 0 && i % 10 == 0)
            {
                height++;
            }

            tileSprite.transform.position = new Vector2(posX, height);

            int spriteIndex = mapData.tiles[i].imageIdx;

            tileSprite.sprite = sprites[spriteIndex];

        }
    }
}
