using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTileData : MonoBehaviour
{
    public bool isMoveable = false;
    public bool isBuildable = false;
    public Sprite[] tileSprites; // 타일의 모든 스프라이트를 저장
    private SpriteRenderer spriteRenderer;
    public int tileIndex;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetTileColor(Color _color)
    {
        spriteRenderer.color = _color;
    }

    public void SetTileSprite(int index)
    {
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        spriteRenderer.sprite = tileSprites[index];
    }
}