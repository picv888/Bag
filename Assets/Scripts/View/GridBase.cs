using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GridBase : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{

    public int itemID = -1;// 规定-1  那么这个格子空的

    public Transform tempParent;//拖动时临时的父物体

    protected Image image;//显示装备的图片
    protected RectTransform rectTransform;
    private Vector2 outPos;//转换之后的坐标
    private Vector2 offset;//鼠标偏移量
    private RectTransform parentRT;

    protected virtual void Awake(){
        rectTransform = transform as RectTransform;
        image = transform.Find("Item").GetComponent<Image>();
        parentRT = tempParent as RectTransform;
    }

    /// <summary>
    /// 更新自己本身的物品
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="iconName"></param>
    public virtual void UpdateItem(int itemID, string iconName)
    {
        if (this.itemID == itemID && itemID >= 0)
        {
            return;
        }

        this.itemID = itemID;

        if (itemID < 0)//没有物品
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;

            if (image.sprite == null || image.sprite.name != iconName)
            {
                Sprite sp = Resources.Load<Sprite>("Texture/Icon/" + iconName);
                image.sprite = sp;
            }
        }
    }


    #region 接口的虚方法

    //开始拖动的虚方法
    protected virtual void BeginDrag(PointerEventData eventData)
    {
        if (itemID < 0) return;
        //Debug.Log("父类：BeginDrag");
        TipsUI.Instance.HideTips();
        image.transform.parent = tempParent;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT,
            eventData.position, eventData.enterEventCamera, out outPos))
        {
            offset = outPos - new Vector2(image.transform.localPosition.x, image.transform.localPosition.y);
        }
    }

    //拖动的虚方法
    protected virtual void Drag(PointerEventData eventData)
    {
        if (itemID < 0) return;
        //Debug.Log("父类：Drag");
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT,
            eventData.position, eventData.enterEventCamera, out outPos))
        {
            image.transform.localPosition = outPos - offset;
        }
    }

    //拖动结束时的虚方法
    protected virtual void EndDrag(PointerEventData eventData)
    {
        if (itemID < 0) return;
        //Debug.Log("父类：EndDrag");
        image.transform.parent = transform;
        image.transform.localPosition = Vector3.zero;
    }

    //点击的虚方法
    protected virtual void Click(PointerEventData eventData)
    {
        //Debug.Log("父类：Click");
    }

    //进入的虚方法
    protected virtual void Enter(PointerEventData eventData)
    {
        //Debug.Log("父类：Enter");
       // Debug.Log("显示信息");
    }

    //出去的虚方法
    protected virtual void Exit(PointerEventData eventData)
    {
        //Debug.Log("父类：Exit");
        //Debug.Log("隐藏信息");
        TipsUI.Instance.HideTips();
    }

    #endregion

    #region 实现的接口

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Drag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDrag(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Click(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Enter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Exit(eventData);


    }

    #endregion
}
