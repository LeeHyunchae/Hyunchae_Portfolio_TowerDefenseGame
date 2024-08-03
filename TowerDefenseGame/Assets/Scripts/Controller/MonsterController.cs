using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour, IPoolable
{
    private const float MINDISTANCE = 0.05f;

    private MonsterManager monsterManager;
    private RouteData route;
    private int goalIndex;
    private int moveSpeed = 3;

    private Transform myTransform;
    private Vector2 pos;
    private int curRouteIndex = 0;
    private Vector3 targetPos;

    private Vector2 moveDirection;
    public void Init()
    {
        monsterManager = MonsterManager.getInstance;
        myTransform = gameObject.transform;
        pos = transform.position;

    }

    public void SetRoute(RouteData _routeData,int _goalIndex)
    {
        route = _routeData;
        goalIndex = _goalIndex;
    }

    public void Spawn()
    {
        int posX = route.tileIdxs[curRouteIndex] % 10;
        int posY = route.tileIdxs[curRouteIndex] / 10;

        pos = new Vector2(posX, posY);
        myTransform.position = pos;

        NextRoute();
    }

    private void Update()
    {
        pos.x += moveDirection.x * Time.deltaTime * moveSpeed;
        pos.y += moveDirection.y * Time.deltaTime * moveSpeed;

        myTransform.position = pos;

        float distance = Vector2.Distance(targetPos, myTransform.position);

        if(distance < MINDISTANCE)
        {
            NextRoute();
        }
    }

    private void NextRoute()
    {
        curRouteIndex++;

        int moveIndex;

        if(curRouteIndex > route.tileIdxs.Length)
        {
            //Goal Tile Arrive
            monsterManager.EnqueueMonster(this);
            return;
        }
        else if(curRouteIndex == route.tileIdxs.Length)
        {
            moveIndex = goalIndex;
        }
        else
        {
            moveIndex = route.tileIdxs[curRouteIndex];
        }

        int posX = moveIndex % 10;
        int posY = moveIndex / 10;

        targetPos = new Vector2(posX, posY);

        moveDirection = (targetPos - transform.position).normalized;
    }

    public void OnEnqueue()
    {
        gameObject.SetActive(false);
    }

    public void OnDequeue()
    {
        gameObject.SetActive(true);
        curRouteIndex = 0;
        pos = Vector2.zero;
        targetPos = Vector2.zero;
        myTransform.position = Vector2.zero;
    }

}
