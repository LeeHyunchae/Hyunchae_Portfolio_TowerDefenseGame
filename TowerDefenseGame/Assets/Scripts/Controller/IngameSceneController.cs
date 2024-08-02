using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameSceneController : MonoBehaviour
{
    [SerializeField] private MapCreator mapCreator;
    [SerializeField] private MonsterController originMonster;

    private void Awake()
    {
        InitMapCreator();
    }

    private void InitMapCreator()
    {
        MapData mapData = TableLoader.LoadFromFile<MapData>("Map/MapData0");

        mapCreator.SetMapData(mapData);
    }

    private void CreateMap()
    {
        mapCreator.CreateMap();

        float mapWidth = mapCreator.GetMapData.wid * 0.5f;

        Camera cam = Camera.main;
        cam.transform.position = new Vector3(mapWidth, mapWidth - 0.5f,cam.transform.position.z);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            CreateMap();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            MonsterController monster = Instantiate<MonsterController>(originMonster);

            int ran = Random.Range(0, 2);

            monster.SetRoute(mapCreator.GetMapData.routes[ran],mapCreator.GetMapData.goalIdx);
            monster.Spawn();
        }
    }
}
