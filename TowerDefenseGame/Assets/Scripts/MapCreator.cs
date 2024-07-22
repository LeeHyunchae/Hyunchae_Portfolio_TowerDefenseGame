using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    [SerializeField] private GameObject originTilePrefab;
    [SerializeField] private Sprite[] sprites;

    private MapData mapData;

    private GameObject gridParent;

    private void Awake()
    {
        mapData = TableLoader.LoadFromFile<MapData>("Map/MapData0");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CreateMap();
        }
    }

    private void CreateMap()
    {
        //int width = mapData.wid;
        //int height = mapData.tiles.Length / mapData.wid;


        //for (int y = 0; y < height; y++)
        //{
        //    for(int x = 0; x < width; x++)
        //    {
        //        SpriteRenderer tileSprite = Instantiate<GameObject>(originTilePrefab, gridParent.transform).GetComponent<SpriteRenderer>();
        //        tileSprite.transform.position = new Vector2(x, y);

        //        //tileSprite.sprite = mapData.tiles[]
        //    }
        //}
        
        gridParent = new GameObject("Grid");

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
