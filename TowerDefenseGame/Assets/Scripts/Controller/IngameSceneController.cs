using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameSceneController : MonoBehaviour
{

    private MonsterManager monsterManager;
    private Camera mainCam;
    private InputController inputController;
    private MapData mapData;
    private MapCreator mapCreator;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        InitMapCreator();
        monsterManager = MonsterManager.getInstance;
        InitInputController();
    }

    private void InitMapCreator()
    {
        mapData = TableLoader.LoadFromFile<MapData>("Map/MapData0");

        mapCreator = new MapCreator();
        mapCreator.Init();
        mapCreator.SetMapData(mapData);

        CreateMap();
    }

    private void InitInputController()
    {
        inputController = new InputController();
        inputController.SetMapData(mapData);
        inputController.SetIsPlayGame(true);
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
            MonsterController monster = monsterManager.GetMonster();

            int ran = Random.Range(0, 2);

            monster.SetRoute(mapCreator.GetMapData.routes[ran],mapCreator.GetMapData.goalIdx);
            monster.Spawn();
        }

        inputController.Update();
    }
}
