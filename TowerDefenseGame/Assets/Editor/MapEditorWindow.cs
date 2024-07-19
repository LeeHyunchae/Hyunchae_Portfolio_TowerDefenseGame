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

    private EditorTileData[,] tiles;

    private GameObject gridParent;

    private List<SpawnTile> spawnTiles = new List<SpawnTile>();

    private int curSpawnTileIndex = -1;

    private bool isSetSpawnRoute = false;

    private bool isSetGoalTile = false;

    private bool isSetBuildableTile = false;

    private int goalTileIndex = -1;

    private List<SpawnTile> editorSpawnTiles = new List<SpawnTile>();

    private List<int> buildableTiles = new List<int>();

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

                tile.GetComponent<EditorTileData>().SetTileSprite((int)ETileShape.EMPTY);
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

        curSpawnTileIndex = -1;
        goalTileIndex = -1;

        isSetSpawnRoute = false;
        isSetGoalTile = false;

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

            int routeTileX = routeTileIndex / gridSize.y;
            int routeTileY = routeTileIndex % gridSize.y;

            EditorTileData routeTile = tiles[routeTileX, routeTileY];

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

            int routeTileX = routeTileIndex / gridSize.y;
            int routeTileY = routeTileIndex % gridSize.y;

            EditorTileData routeTile = tiles[routeTileX, routeTileY];

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
                        int xIndex = goalTileIndex / gridSize.y; 
                        int yIndex = goalTileIndex % gridSize.y; 

                        EditorTileData prevGoalTile = tiles[xIndex, yIndex];
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

            int spawnTileX = spawnTileIndex / gridSize.y;
            int spawnTileY = spawnTileIndex % gridSize.y;

            EditorTileData spawnTile = tiles[spawnTileX, spawnTileY];

            spawnTile.SetTileSprite((int)ETileShape.SPAWN);

            spawnTile.isMoveable = true;

        }
        else if(spawnTiles[curSpawnTileIndex].routeIndexList.Count > 1)
        {
            List<int> routeArr = spawnTiles[curSpawnTileIndex].routeIndexList;

            int prevTileIndex = routeArr[routeArr.Count - 2];

            int prevTileX = prevTileIndex / gridSize.y;
            int prevTileY = prevTileIndex % gridSize.y;

            int curTileIndex = routeArr[routeArr.Count -1];

            int curTileX = curTileIndex / gridSize.y;
            int curTileY = curTileIndex % gridSize.y;

            Vector2 prevTilePos = new Vector2(prevTileX, prevTileY);
            Vector2 curTilePos = new Vector2(curTileX, curTileY);

            EditorTileData curTile = tiles[curTileX, curTileY];

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

                    EditorTileData tileData = tiles[(int)tilePos.x, (int)tilePos.y];
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

                    EditorTileData tileData = tiles[(int)tilePos.x, (int)tilePos.y];
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

        int goalTileX = goalTileIndex / gridSize.y;
        int goalTileY = goalTileIndex % gridSize.y;

        int lastRouteTileIndex = routeArr[routeArr.Count - 1];

        int lastRouteTileX = lastRouteTileIndex / gridSize.y;
        int lastRouteTileY = lastRouteTileIndex % gridSize.y;

        Vector2 goalTilePos = new Vector2(goalTileX, goalTileY);
        Vector2 lastRouteTilePos = new Vector2(lastRouteTileX, lastRouteTileY);

        EditorTileData lastRouteTile = tiles[lastRouteTileX, lastRouteTileY];

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

                EditorTileData tileData = tiles[(int)tilePos.x, (int)tilePos.y];
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

                EditorTileData tileData = tiles[(int)tilePos.x, (int)tilePos.y];
                tileData.SetTileSprite((int)ETileShape.HORIZONTAL);
                editorSpawnTiles[curSpawnTileIndex].routeIndexList.Add(tileData.tileIndex);
                tileData.isMoveable = true;
            }
        }
    }
}