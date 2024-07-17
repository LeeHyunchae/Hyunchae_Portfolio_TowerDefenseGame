using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnTile
{
    public int startPointIndex;
    public List<int> routeIndexList = new List<int>();
}

public class MapEditorWindow : EditorWindow
{
    private GameObject originTilePrefabObject;
    private EditorTileData originTileData;
    private Vector2Int gridSize;
    private Vector2 cellSize = new Vector2(1, 1);

    private EditorTileData[,] tiles;

    private GameObject gridParent;

    private List<SpawnTile> spawnTiles = new List<SpawnTile>();

    private int curSpawnTileIndex = -1;

    private bool isSetSpawnStartTile = false;
    private bool isSetSpawnRoute = false;

    private bool isSetGoalTile = false;

    private int curSpawnStartIndex = -1;
    private int goalTileIndex = -1;

    [MenuItem("Window/Map Editor")]
    public static void ShowWindow()
    {
        GetWindow<MapEditorWindow>("Map Editor");
    }

    private void OnGUI()
    {
        GUILayout.Space(25);

        originTilePrefabObject = (GameObject)EditorGUILayout.ObjectField("Tile Prefab", originTilePrefabObject, typeof(GameObject), false);

        if (originTilePrefabObject != null)
        {
            originTileData = originTilePrefabObject.GetComponent<EditorTileData>(); 
        }

        GUILayout.Space(25);

        gridSize = EditorGUILayout.Vector2IntField("Grid Size", gridSize);

        GUILayout.Space(25);

        if (GUILayout.Button("Create Grid"))
        {
            CreateGrid();
        }

        GUILayout.Space(10);
        if(tiles != null)
        {
            if (GUILayout.Button("Set Goal Tile"))
            {
                isSetGoalTile = true;
            }

            if (GUILayout.Button("SetEnd Goal Tile"))
            {
                isSetGoalTile = false;
            }
        }
        
        GUILayout.Space(25);

        if(curSpawnTileIndex >= 0)
        {
            GUILayout.Label("CurSpawnPointIndex = " + curSpawnTileIndex);

            GUILayout.Space(10);

            if (GUILayout.Button("Set CurSpawnRoute StartPoint"))
            {
                isSetSpawnStartTile = true;
                isSetSpawnRoute = false;
            }

            if (GUILayout.Button("Set CurSpawnRoute"))
            {
                isSetSpawnStartTile = false;
                isSetSpawnRoute = true;
            }

            if(GUILayout.Button("Cur SpawnRoute Setting End"))
            {
                isSetSpawnStartTile = false;
                isSetSpawnRoute = false;
            }

            if (GUILayout.Button("Clear Cur SpawnRoute Setting"))
            {
                isSetSpawnStartTile = false;
                isSetSpawnRoute = false;

                ClearCurSpawnRouteSetting();
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Add Spawn And Move Route"))
        {
            spawnTiles.Add(new SpawnTile());

            curSpawnTileIndex++;
            curSpawnStartIndex = -1;
        }

        GUILayout.Space(10);

        if (curSpawnTileIndex >= 0)
        {

            if (GUILayout.Button("RemoveLast Spawn And Move Route"))
            {
                if (spawnTiles.Count == 0)
                {
                    Debug.Log("SpawnPointList is Empty");
                    curSpawnTileIndex = -1;
                    return;
                }

                ClearCurSpawnRouteSetting();

                spawnTiles.RemoveAt(spawnTiles.Count - 1);

                curSpawnTileIndex--;

                if(spawnTiles.Count > 0)
                {
                    curSpawnStartIndex = spawnTiles[spawnTiles.Count - 1].startPointIndex;
                }
            }

        }

        GUILayout.Space(25);

        if (GUILayout.Button("Clear Grid"))
        {
            ClearGrid();
        }
    }

    private void CreateGrid()
    {
        if(originTilePrefabObject == null || originTileData == null)
        {
            Debug.Log("Set Origin Tile Prefab");
            return;
        }

        if(gridSize.x == 0 || gridSize.y == 0)
        {
            Debug.Log("Wrong Grid Size");
            return;
        }

        if (tiles != null)
        {
            foreach (var tile in tiles)
            {
                DestroyImmediate(tile);
            }

            DestroyImmediate(gridParent);
        }

        tiles = new EditorTileData[gridSize.x, gridSize.y];
        gridParent = new GameObject("Grid");

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                EditorTileData tile = Instantiate<EditorTileData>(originTileData,gridParent.transform).GetComponent<EditorTileData>();
                tile.transform.position = new Vector3(x * cellSize.x, y * cellSize.y, 0);
                tiles[x, y] = tile;

                int tileIndex = x * gridSize.y + y;

                tile.tileIndex = tileIndex;

                tile.GetComponent<EditorTileData>().SetTileSprite(11);
            }
        }
    }
    private void ClearGrid()
    {
        if (tiles != null)
        {
            foreach (var tile in tiles)
            {
                if (tile != null)
                {
                    DestroyImmediate(tile.gameObject);
                }
            }

            DestroyImmediate(gridParent);
        }

        curSpawnStartIndex = -1;
        curSpawnTileIndex = -1;
        goalTileIndex = -1;

        isSetSpawnStartTile = false;
        isSetSpawnRoute = false;
        isSetGoalTile = false;

        spawnTiles.Clear();
        tiles = null;
        gridParent = null;
    }

    private void ClearCurSpawnRouteSetting()
    {
        if (curSpawnStartIndex != -1)
        {
            int xIndex = curSpawnStartIndex / gridSize.y; // x 인덱스 계산
            int yIndex = curSpawnStartIndex % gridSize.y; // y 인덱스 계산

            EditorTileData prevStartTtile = tiles[xIndex, yIndex];
            prevStartTtile.SetTileColor(Color.white);

            curSpawnStartIndex = -1;
        }

        int count = spawnTiles[curSpawnTileIndex].routeIndexList.Count;

        for(int i = 0; i < count; i ++)
        {
            int tileIndex = spawnTiles[curSpawnTileIndex].routeIndexList[i]; // 예시로 42번 타일에 접근하는 경우

            // tileIndexToAccess를 통해 tiles 배열에서 타일에 접근
            int xIndex = tileIndex / gridSize.y; // x 인덱스 계산
            int yIndex = tileIndex % gridSize.y; // y 인덱스 계산

            EditorTileData routeTile = tiles[xIndex, yIndex];

            routeTile.SetTileColor(Color.white);
        }

        spawnTiles[curSpawnTileIndex].routeIndexList.Clear();
    }


    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Vector2 mousePos = Event.current.mousePosition;
            mousePos.y = sceneView.camera.pixelHeight - mousePos.y; // Flip Y coordinate
            Vector3 worldPos = sceneView.camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, sceneView.camera.nearClipPlane));
            OnMouseClick(worldPos);
        }
    }

    private void OnMouseClick(Vector3 mousePosition)
    {
        mousePosition.z = 0;

        foreach (var tile in tiles)
        {
            if (tile != null && Vector3.Distance(mousePosition, tile.transform.position) < Mathf.Min(cellSize.x, cellSize.y) / 2)
            {
                if(isSetSpawnStartTile)
                {
                    if(curSpawnStartIndex != -1)
                    {
                        int xIndex = curSpawnStartIndex / gridSize.y; // x 인덱스 계산
                        int yIndex = curSpawnStartIndex % gridSize.y; // y 인덱스 계산

                        EditorTileData prevStartTtile = tiles[xIndex, yIndex];
                        prevStartTtile.SetTileColor(Color.white);
                    }

                    curSpawnStartIndex = tile.tileIndex;
                    spawnTiles[curSpawnTileIndex].startPointIndex = tile.tileIndex;
                    tile.SetTileColor(Color.cyan);
                }

                if(isSetSpawnRoute)
                {
                    //Todo Road SpriteSwap
                    spawnTiles[curSpawnTileIndex].routeIndexList.Add(tile.tileIndex);
                    tile.SetTileColor(Color.red);
                }

                if(isSetGoalTile)
                {
                    if(goalTileIndex != -1)
                    {
                        int xIndex = goalTileIndex / gridSize.y; // x 인덱스 계산
                        int yIndex = goalTileIndex % gridSize.y; // y 인덱스 계산

                        EditorTileData prevGoalTile = tiles[xIndex, yIndex];
                        prevGoalTile.SetTileColor(Color.white);
                    }

                    goalTileIndex = tile.tileIndex;
                    tile.SetTileColor(Color.blue);
                }

                //UpdateTileSprites();

                break;
            }
        }
    }

    private void UpdateTileSprites()
    {
        //// 모든 타일의 스프라이트를 업데이트
        //for (int x = 0; x < gridSize.x; x++)
        //{
        //    for (int y = 0; y < gridSize.y; y++)
        //    {
        //        EditorTileData tileData = tiles[x, y].GetComponent<EditorTileData>();
        //        int spriteIndex = GetTileSpriteIndex(x, y);
        //        tileData.SetTileSprite(spriteIndex);
        //    }
        //}
    }

    private int GetTileSpriteIndex(int x, int y)
    {
        //// 주변 타일을 검사하여 스프라이트 인덱스를 결정
        //bool up = y + 1 < gridSize.y && tiles[x, y + 1].GetComponent<EditorTileData>().isStartPoint;
        //bool down = y - 1 >= 0 && tiles[x, y - 1].GetComponent<EditorTileData>().isStartPoint;
        //bool left = x - 1 >= 0 && tiles[x - 1, y].GetComponent<EditorTileData>().isStartPoint;
        //bool right = x + 1 < gridSize.x && tiles[x + 1, y].GetComponent<EditorTileData>().isStartPoint;

        //// 각 타일의 상태에 따라 적절한 스프라이트 인덱스를 반환
        //if (up && down && left && right) return 0;
        //if (up && down && left) return 1;
        //if (up && down && right) return 2;
        //if (up && left && right) return 3;
        //if (down && left && right) return 4;
        //if (up && down) return 5;
        //if (left && right) return 6;
        //if (up) return 7;
        //if (down) return 8;
        //if (left) return 9;
        //if (right) return 10;

        return 11; // 기본 스프라이트 인덱스
    }
}