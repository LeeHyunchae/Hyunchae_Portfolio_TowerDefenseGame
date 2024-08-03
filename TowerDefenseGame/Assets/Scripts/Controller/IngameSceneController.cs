using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameSceneController : MonoBehaviour
{
    [SerializeField] private MapCreator mapCreator;

    private MonsterManager monsterManager;
    private Camera mainCam;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        InitMapCreator();
        monsterManager = MonsterManager.getInstance;
    }

    private void InitMapCreator()
    {
        MapData mapData = TableLoader.LoadFromFile<MapData>("Map/MapData0");

        mapCreator.SetMapData(mapData);

        CreateMap();
    }

    private void CreateMap()
    {
        mapCreator.CreateMap();

        float mapWidth = mapCreator.GetMapData.wid * 0.5f;

        mainCam = Camera.main;
        mainCam.transform.position = new Vector3(mapWidth, mapWidth - 0.5f,mainCam.transform.position.z);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("몬스터 스폰");
            MonsterController monster = monsterManager.GetMonster();

            int ran = Random.Range(0, 2);

            monster.SetRoute(mapCreator.GetMapData.routes[ran],mapCreator.GetMapData.goalIdx);
            monster.Spawn();
        }
    }
}
