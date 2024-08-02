using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private const float MINDISTANCE = 0.05f;

    private RouteData route;
    private int goalIndex;
    private int moveSpeed = 3;

    private Vector2 pos;
    private int curRouteIndex = 0;
    private Vector3 targetPos;

    private Vector2 moveDirection;

    public void SetRoute(RouteData _routeData,int _goalIndex)
    {
        route = _routeData;
        goalIndex = _goalIndex;
    }

    public void Spawn()
    {
        int posX = route.tileIdxs[curRouteIndex] % 10;
        int posY = route.tileIdxs[curRouteIndex] / 10;

        transform.position = new Vector2(posX, posY);
        pos = transform.position;

        NextRoute();
    }

    private void Update()
    {

        pos.x += moveDirection.x * Time.deltaTime * moveSpeed;
        pos.y += moveDirection.y * Time.deltaTime * moveSpeed;

        transform.position = pos;

        float distance = Vector2.Distance(targetPos, transform.position);

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
            gameObject.SetActive(false);
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
}
