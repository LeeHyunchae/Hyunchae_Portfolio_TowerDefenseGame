using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTileData : MonoBehaviour
{
    public bool isMoveable = false;
    public bool isBuildable = false;
    public Sprite[] tileSprites;
    private SpriteRenderer spriteRenderer;
    public int tileIndex;
    public int tileSpriteIndex;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetTileColor(Color _color)
    {
        spriteRenderer.color = _color;
    }

    public void SetTileSprite(int _index)
    {
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        tileSpriteIndex = _index;

        spriteRenderer.sprite = tileSprites[_index];
    }
}