using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private TowerData towerData;
    private Transform myTransform;

    public void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myTransform = gameObject.transform;
    }

    public void SetTowerPosition(Vector2 _pos)
    {
        myTransform.position = _pos;
    }

    public void SetTowerData(TowerData _towerData)
    {
        towerData = _towerData;
    }
}
