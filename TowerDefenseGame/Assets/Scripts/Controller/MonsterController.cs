using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private RouteData route;
    private int goalIndex;
    private int moveSpeed = 3;

    private Vector2 pos;
    private int curRouteIndex = 0;
    private Vector3 targetPos;
    private float minDistance = 0.05f;

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
        Vector2 direction = (targetPos - transform.position).normalized;

        pos.x += direction.x * Time.deltaTime * moveSpeed;
        pos.y += direction.y * Time.deltaTime * moveSpeed;

        transform.position = pos;

        float distance = Vector2.Distance(targetPos, transform.position);

        if(distance < minDistance)
        {
            NextRoute();
        }
    }

    private void NextRoute()
    {
        curRouteIndex++;

        if(curRouteIndex > route.tileIdxs.Length)
        {
            gameObject.SetActive(false);
            return;
        }
        else if(curRouteIndex == route.tileIdxs.Length)
        {
            int posX = goalIndex % 10;
            int posY = goalIndex / 10;

            targetPos = new Vector2(posX, posY);
        }
        else
        {
            int posX = route.tileIdxs[curRouteIndex] % 10;
            int posY = route.tileIdxs[curRouteIndex] / 10;

            targetPos = new Vector2(posX, posY);
        }

    }
}
