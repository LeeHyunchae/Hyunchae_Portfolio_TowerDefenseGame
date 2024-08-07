using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameSceneController : MonoBehaviour
{

    private MonsterManager monsterManager;
    private Camera mainCam;
    private InputController inputController;
    private MapCreator mapCreator;
    private MapManager mapManager;
    private TowerBuildController towerBuildController;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        mapManager = MapManager.getInstance;
        monsterManager = MonsterManager.getInstance;
        InitMap();
        InitInputController();
    }

    private void InitMap()
    {
        mapCreator = new MapCreator();
        mapCreator.Init();

        towerBuildController = new TowerBuildController();
        towerBuildController.Init();

        CreateMap();
    }

    private void InitInputController()
    {
        inputController = new InputController();
        inputController.SetMapData(mapManager.GetMapData);
        inputController.SetIsPlayGame(true);

        inputController.OnClickBuildableTileAction = towerBuildController.OnClickBuildableTile;
    }

    private void CreateMap()
    {
        List<int> builableTileList = mapCreator.CreateMap();
        towerBuildController.CreateTower(builableTileList);

        float mapWidth = mapManager.GetMapData.wid * 0.5f;

        mainCam = Camera.main;
        mainCam.transform.position = new Vector3(mapWidth, mapWidth - 0.5f,mainCam.transform.position.z);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            MonsterController monster = monsterManager.GetMonster();

            int ran = Random.Range(0, 2);

            monster.SetRoute(mapManager.GetMapData.routes[ran],mapManager.GetMapData.goalIdx);
            monster.Spawn();
        }

        inputController.Update();
    }
}
