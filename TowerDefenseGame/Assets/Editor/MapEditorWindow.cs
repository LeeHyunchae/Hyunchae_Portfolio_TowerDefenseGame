using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnTile
{
    public List<int> routeIndexList = new List<int>();
}

public enum ETileShape
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
    EMPTY = 10
}

public class MapEditorWindow : EditorWindow
{
    private GameObject originTilePrefabObject;
    private EditorTileData originTileData;
    private Vector2Int gridSize;
    private Vector2 cellSize = new Vector2(1, 1);

    private EditorTileData[] tiles;

    private GameObject gridParent;

    private List<SpawnTile> spawnTiles = new List<SpawnTile>();

    private int curSpawnTileIndex = -1;

    private bool isSetSpawnRoute = false;

    private bool isSetGoalTile = false;

    private bool isSetBuildableTile = false;

    private int goalTileIndex = -1;

    private List<SpawnTile> editorSpawnTiles = new List<SpawnTile>();

    private List<int> buildableTiles = new List<int>();

    private int mapUid = -1;

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

            if (GUILayout.Button("SettingEnd Goal Tile"))
            {
                isSetGoalTile = false;
            }
        }
        
        GUILayout.Space(25);

        if(curSpawnTileIndex >= 0)
        {
            GUILayout.Label("CurSpawnPointIndex = " + curSpawnTileIndex);

            GUILayout.Space(10);

            if (GUILayout.Button("Set CurSpawnRoute"))
            {
                isSetSpawnRoute = true;
            }

            if(GUILayout.Button("Setting End Cur SpawnRoute"))
            {
                isSetSpawnRoute = false;
                SetLineToGoalTile();
            }

            if (GUILayout.Button("Clear Cur SpawnRoute Setting"))
            {
                isSetSpawnRoute = false;

                ClearCurSpawnRouteSetting();
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Add Spawn And Move Route"))
        {
            spawnTiles.Add(new SpawnTile());

            curSpawnTileIndex++;

            editorSpawnTiles.Add(new SpawnTile());
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
                editorSpawnTiles.RemoveAt(editorSpawnTiles.Count - 1);

                curSpawnTileIndex--;

            }

        }

        if (GUILayout.Button("Set Buildable Tile"))
        {
            isSetBuildableTile = true;
        }

        if (GUILayout.Button("Setting End Buildable Tile"))
        {
            isSetBuildableTile = false;
        }

        if (GUILayout.Button("Clear Buildable Tiles"))
        {
            ClearBuildableTiles();
        }

        GUILayout.Space(25);

        if (GUILayout.Button("Clear Grid"))
        {
            ClearGrid();
        }

        mapUid = EditorGUILayout.IntField("Map ID", mapUid);

        if (GUILayout.Button("Save Map"))
        {
            SaveMapData();
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
            ClearGrid();
        }

        tiles = new EditorTileData[gridSize.x * gridSize.y];
        gridParent = new GameObject("Grid");

        int count = tiles.Length;

        for(int i = 0; i <count; i ++)
        {
            EditorTileData tile = Instantiate<EditorTileData>(originTileData, gridParent.transform).GetComponent<EditorTileData>();

            int posX = i % gridSize.x;
            int posY = i / gridSize.y;

            tile.transform.position = new Vector2(posX,posY);

            tile.tileIndex = i;

            tile.GetComponent<EditorTileData>().SetTileSprite((int)ETileShape.EMPTY);

            tiles[i] = tile;
        }

        //for (int y = 0; y < gridSize.y; y++)
        //{
        //    for (int x = 0; x < gridSize.x; x++)
        //    {
        //        EditorTileData tile = Instantiate<EditorTileData>(originTileData, gridParent.transform).GetComponent<EditorTileData>();
        //        tile.transform.position = new Vector3(x * cellSize.x, y * cellSize.y, 0);
        //        tiles[x, y] = tile;

        //        int tileIndex = x * gridSize.y + y;

        //        tile.tileIndex = tileIndex;

        //        tile.GetComponent<EditorTileData>().SetTileSprite((int)ETileShape.EMPTY);
        //    }
        //}
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

        curSpawnTileIndex = -1;
        goalTileIndex = -1;

        isSetSpawnRoute = false;
        isSetGoalTile = false;
        isSetBuildableTile = false;

        editorSpawnTiles.Clear();
        buildableTiles.Clear();
        spawnTiles.Clear();
        tiles = null;
        gridParent = null;
    }

    private void ClearCurSpawnRouteSetting()
    {
        spawnTiles[curSpawnTileIndex].routeIndexList.Clear();

        List<int> routeList = editorSpawnTiles[curSpawnTileIndex].routeIndexList;

        int count = routeList.Count;

        for(int i = 0; i < count; i ++)
        {
            int routeTileIndex = routeList[i];

            EditorTileData routeTile = tiles[routeTileIndex];

            routeTile.SetTileSprite((int)ETileShape.EMPTY);
            routeTile.isMoveable = false;
        }

        routeList.Clear();
    }

    private void ClearBuildableTiles()
    {
        int count = buildableTiles.Count;

        for (int i = 0; i < count; i++)
        {
            int routeTileIndex = buildableTiles[i];

            EditorTileData routeTile = tiles[routeTileIndex];

            routeTile.SetTileSprite((int)ETileShape.EMPTY);
            routeTile.isBuildable = false;
        }

        buildableTiles.Clear();
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
            Vector2 mousePos = e.mousePosition;
            Ray worldRay = HandleUtility.GUIPointToWorldRay(mousePos);
            Plane plane = new Plane(Vector3.forward, Vector3.zero);
            float distance;
            if (plane.Raycast(worldRay, out distance))
            {
                Vector3 worldPos = worldRay.GetPoint(distance);
                worldPos.z = 0;
                OnMouseClick(worldPos);
            }
        }
    }

    private void OnMouseClick(Vector3 mousePosition)
    {
        if(tiles == null)
        {
            return;
        }

        mousePosition.z = 0;

        foreach (var tile in tiles)
        {
            if (tile != null && Vector3.Distance(mousePosition, tile.transform.position) < Mathf.Min(cellSize.x, cellSize.y) / 2)
            {
                if(isSetSpawnRoute)
                {
                    if (tile.tileIndex == goalTileIndex)
                    {
                        Debug.Log("Duplicate GoalTile");
                        return;
                    }

                    spawnTiles[curSpawnTileIndex].routeIndexList.Add(tile.tileIndex);
                    editorSpawnTiles[curSpawnTileIndex].routeIndexList.Add(tile.tileIndex);
                    UpdateRouteTile();
                    //tile.SetTileColor(Color.red);
                }

                if(isSetGoalTile)
                {
                    if(goalTileIndex != -1)
                    {
                        
                        EditorTileData prevGoalTile = tiles[goalTileIndex];
                        prevGoalTile.SetTileSprite((int)ETileShape.EMPTY);
                    }

                    goalTileIndex = tile.tileIndex;
                    tile.SetTileSprite((int)ETileShape.GOAL);
                }

                if(isSetBuildableTile)
                {
                    if(tile.isMoveable)
                    {
                        Debug.Log("This Tile is Move Route Tile");
                        return;
                    }

                    buildableTiles.Add(tile.tileIndex);
                    tile.isBuildable = true;
                    tile.SetTileSprite((int)ETileShape.BUILDABLE);
                }

                break;
            }
        }
    }

    private void UpdateRouteTile()
    {
        if(spawnTiles[curSpawnTileIndex].routeIndexList.Count == 1)
        {
            int spawnTileIndex = spawnTiles[curSpawnTileIndex].routeIndexList[0];

            EditorTileData spawnTile = tiles[spawnTileIndex];

            spawnTile.SetTileSprite((int)ETileShape.SPAWN);

            spawnTile.isMoveable = true;

        }
        else if(spawnTiles[curSpawnTileIndex].routeIndexList.Count > 1)
        {
            List<int> routeArr = spawnTiles[curSpawnTileIndex].routeIndexList;

            int prevTileIndex = routeArr[routeArr.Count - 2];

            int prevTileX = prevTileIndex % gridSize.x;
            int prevTileY = prevTileIndex / gridSize.x;

            int curTileIndex = routeArr[routeArr.Count -1];

            int curTileX = curTileIndex % gridSize.x;
            int curTileY = curTileIndex / gridSize.x;

            Vector2 prevTilePos = new Vector2(prevTileX, prevTileY);
            Vector2 curTilePos = new Vector2(curTileX, curTileY);

            EditorTileData curTile = tiles[curTileIndex];

            Vector2 directionVector = curTilePos - prevTilePos;

            bool isVertical = directionVector.y != 0;
            bool isHorizontal = directionVector.x != 0;

            if (isVertical)
            {
                int count = (int)Mathf.Abs(directionVector.y) - 1;

                curTile.SetTileSprite((int)ETileShape.VERTICAL);

                for (int i = 0; i < count; i++)
                {
                    int increase = directionVector.y > 0 ? i + 1 : (i + 1) * -1;

                    Vector2 tilePos = prevTilePos;
                    tilePos.y += increase;

                    int tileIndex = (int)tilePos.y * gridSize.x + (int)tilePos.x;

                    EditorTileData tileData = tiles[tileIndex];
                    tileData.SetTileSprite((int)ETileShape.VERTICAL);
                    editorSpawnTiles[curSpawnTileIndex].routeIndexList.Add(tileData.tileIndex);
                    tileData.isMoveable = true;
                }
            }
            else if (isHorizontal)
            {
                int count = (int)Mathf.Abs(directionVector.x) - 1;

                curTile.SetTileSprite((int)ETileShape.HORIZONTAL);

                for (int i = 0; i < count; i++)
                {
                    int increase = directionVector.x > 0 ? i + 1 : (i + 1) * -1;

                    Vector2 tilePos = prevTilePos;
                    tilePos.x += increase;

                    int tileIndex = (int)tilePos.y * gridSize.x + (int)tilePos.x;

                    EditorTileData tileData = tiles[tileIndex];
                    tileData.SetTileSprite((int)ETileShape.HORIZONTAL);
                    editorSpawnTiles[curSpawnTileIndex].routeIndexList.Add(tileData.tileIndex);
                    tileData.isMoveable = true;
                }
            }
        }
    }

    private void SetLineToGoalTile()
    {
        List<int> routeArr = spawnTiles[curSpawnTileIndex].routeIndexList;

        int goalTileIndex = this.goalTileIndex;

        int goalTileX = goalTileIndex % gridSize.x;
        int goalTileY = goalTileIndex / gridSize.x;

        int lastRouteTileIndex = routeArr[routeArr.Count - 1];

        int lastRouteTileX = lastRouteTileIndex % gridSize.x;
        int lastRouteTileY = lastRouteTileIndex / gridSize.x;

        Vector2 goalTilePos = new Vector2(goalTileX, goalTileY);
        Vector2 lastRouteTilePos = new Vector2(lastRouteTileX, lastRouteTileY);

        EditorTileData lastRouteTile = tiles[lastRouteTileIndex];

        Vector2 directionVector = goalTilePos - lastRouteTilePos;

        bool isVertical = directionVector.y != 0;
        bool isHorizontal = directionVector.x != 0;

        if (isVertical)
        {
            int count = (int)Mathf.Abs(directionVector.y) - 1;

            lastRouteTile.SetTileSprite((int)ETileShape.VERTICAL);

            for (int i = 0; i < count; i++)
            {
                int increase = directionVector.y > 0 ? i + 1 : (i + 1) * -1;

                Vector2 tilePos = lastRouteTilePos;
                tilePos.y += increase;

                int tileIndex = (int)tilePos.y * gridSize.x + (int)tilePos.x;

                EditorTileData tileData = tiles[tileIndex];
                tileData.SetTileSprite((int)ETileShape.VERTICAL);
                editorSpawnTiles[curSpawnTileIndex].routeIndexList.Add(tileData.tileIndex);
                tileData.isMoveable = true;
            }
        }
        else if (isHorizontal)
        {
            int count = (int)Mathf.Abs(directionVector.x) - 1;

            lastRouteTile.SetTileSprite((int)ETileShape.HORIZONTAL);

            for (int i = 0; i < count; i++)
            {
                int increase = directionVector.x > 0 ? i + 1 : (i + 1) * -1;

                Vector2 tilePos = lastRouteTilePos;
                tilePos.x += increase;

                int tileIndex = (int)tilePos.y * gridSize.x + (int)tilePos.x;

                EditorTileData tileData = tiles[tileIndex];
                tileData.SetTileSprite((int)ETileShape.HORIZONTAL);
                editorSpawnTiles[curSpawnTileIndex].routeIndexList.Add(tileData.tileIndex);
                tileData.isMoveable = true;
            }
        }
    }

    private void SaveMapData()
    {
        if(mapUid == -1)
        {
            Debug.Log("Input MapUID");
            return;
        }
        else if(tiles == null)
        {
            Debug.Log("Map Data Null");
        }


        TileData[] tileDatas = new TileData[tiles.Length];

        int count = tiles.Length;

        foreach(EditorTileData editorTileData in tiles)
        {
            TileData tileData = new TileData()
            {
                imageIdx = editorTileData.tileSpriteIndex,
                moveable = editorTileData.isMoveable,
                buildable = editorTileData.isBuildable
            };

            tileDatas[editorTileData.tileIndex] = tileData;
        }

        count = spawnTiles.Count;

        RouteData[] routeDatas = new RouteData[count];

        for(int i = 0; i < count; i++)
        {
            RouteData routeData = new RouteData
            {
                tileIdxs = spawnTiles[i].routeIndexList.ToArray()
            };

            routeDatas[i] = routeData;
        }

        MapData mapData = new MapData()
        {
            id = mapUid,
            wid = gridSize.x,
            goalIdx = goalTileIndex,
            tiles = tileDatas,
            routes = routeDatas
        };

        TableLoader.SaveToJson("Map", mapData, "MapData" + mapUid);

    }
}