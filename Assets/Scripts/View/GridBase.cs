using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XLua;

public class GridBase : LuaUIBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler {

    public int itemID = -1;// 规定-1  那么这个格子空的

    public Transform tempParent;//拖动时临时的父物体

    public Image image;//显示装备的图片
    protected RectTransform rectTransform;
    private Vector2 outPos;//转换之后的坐标
    private Vector2 offset;//鼠标偏移量
    private RectTransform parentRT;

    protected override void Awake() {
        base.Awake();

        luaDrag = scriptEnv.Get<Action<PointerEventData>>("Drag");
        luaExit = scriptEnv.Get<Action<PointerEventData>>("Exit");
        luaClick = scriptEnv.Get<Action<PointerEventData>>("Click");
        luaEnter = scriptEnv.Get<Action<PointerEventData>>("Enter");
        luaEndDrag = scriptEnv.Get<Action<PointerEventData>>("EndDrag");
        luaBeginDrag = scriptEnv.Get<Action<PointerEventData>>("BeginDrag");
        luaUpdateItem = scriptEnv.Get<Action<int, string>>("UpdateItem");

        rectTransform = transform as RectTransform;
        image = transform.Find("Item").GetComponent<Image>();
        parentRT = tempParent as RectTransform;
    }

    /// <summary>
    /// 更新自己本身的物品
    /// </summary>
    protected Action<int, string> luaUpdateItem;
    public virtual void UpdateItem(int itemID, string iconName) {
        if (this.itemID == itemID && itemID >= 0) {
            return;
        }

        this.itemID = itemID;

        if (itemID < 0)//没有物品
        {
            image.enabled = false;
        }
        else {
            image.enabled = true;

            if (image.sprite == null || image.sprite.name != iconName) {
                Sprite sp = Resources.Load<Sprite>("Texture/Icon/" + iconName);
                image.sprite = sp;
            }
        }
    }


    #region 接口的虚方法

    //开始拖动的虚方法
    protected Action<PointerEventData> luaBeginDrag;
    protected virtual void BeginDrag(PointerEventData eventData) {
        if (itemID < 0) return;
        TipsUI.Instance.HideTips();
        image.transform.parent = tempParent;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT,
            eventData.position, eventData.enterEventCamera, out outPos)) {
            offset = outPos - new Vector2(image.transform.localPosition.x, image.transform.localPosition.y);
        }
    }

    //拖动的虚方法
    protected Action<PointerEventData> luaDrag;
    protected virtual void Drag(PointerEventData eventData) {
        if (itemID < 0) return;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT,
            eventData.position, eventData.enterEventCamera, out outPos)) {
            image.transform.localPosition = outPos - offset;
        }
    }

    //拖动结束时的虚方法
    protected Action<PointerEventData> luaEndDrag;
    protected virtual void EndDrag(PointerEventData eventData) {
        if (itemID < 0) return;
        image.transform.parent = transform;
        image.transform.localPosition = Vector3.zero;
    }

    //点击的虚方法
    protected Action<PointerEventData> luaClick;
    protected virtual void Click(PointerEventData eventData) {
        
    }

    //进入的虚方法
    protected Action<PointerEventData> luaEnter;
    protected virtual void Enter(PointerEventData eventData) {
        
    }

    //出去的虚方法
    protected Action<PointerEventData> luaExit;
    protected virtual void Exit(PointerEventData eventData) {
        TipsUI.Instance.HideTips();
    }

    #endregion

    #region 实现的接口
    public void OnBeginDrag(PointerEventData eventData) {
        BeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData) {
        Drag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData) {
        EndDrag(eventData);
    }

    public void OnPointerClick(PointerEventData eventData) {
        Click(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Enter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData) {
        Exit(eventData);
    }

    #endregion
}
