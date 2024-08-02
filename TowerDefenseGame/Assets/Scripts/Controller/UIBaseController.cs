using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBaseController : MonoBehaviour
{
    protected Canvas canvas;
    protected GraphicRaycaster raycaster;
    protected bool isShow = false;

    protected void Awake()
    {
        canvas = GetComponent<Canvas>();
        raycaster = GetComponent<GraphicRaycaster>();

        Init();
    }

    protected virtual void Init()
    {

    }

    public virtual void Show()
    {
        canvas.enabled = true;
        raycaster.enabled = true;
        isShow = true;
    }

    public virtual void Hide()
    {
        canvas.enabled = false;
        raycaster.enabled = false;
        isShow = false;
    }

    public void SetSortOrder(int _sortorder)
    {
        canvas.sortingOrder = _sortorder;
    }

    public bool IsShow() { return isShow; }
}