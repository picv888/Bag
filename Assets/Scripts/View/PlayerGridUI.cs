using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerGridUI : GridBase {

    public EquipmentType gridType;

    private Text text;
    private GameObject border;
	// Use this for initialization
	protected override void Awake () {
        base.Awake();
        text = transform.Find("Text").GetComponent<Text>();
        border = transform.Find("GridBorder").gameObject;
        gameObject.tag = "PlayerGrid";
        text.text = EquipmentData.GetTypeName(gridType);
    }

    public override void UpdateItem(int itemID, string iconName)
    {
        base.UpdateItem(itemID, iconName);
        if (itemID >= 0)//有装备
        {
            text.enabled = false;//有装备时，把装备栏的文字隐藏
        }
        else
        {
            text.enabled = true;
        }
    }

    protected override void BeginDrag(PointerEventData eventData)
    {
        if (itemID < 0) return;
        base.BeginDrag(eventData);
        text.enabled = true;//开始拖动时，显示文字
    }

    protected override void Click(PointerEventData eventData)
    {
        if (itemID < 0) return;
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("卸下： " + itemID);
            BagController.Instance.DemountItem(itemID, CallBck);
        }
    }

    protected override void EndDrag(PointerEventData eventData)
    {
        if (itemID < 0) return;
        base.EndDrag(eventData);
        text.enabled = false;//开始拖动时，显示文字
        if (eventData.pointerCurrentRaycast.gameObject != null && 
            eventData.pointerCurrentRaycast.gameObject.CompareTag("BagGrid"))
        {
            Debug.Log("卸下装备");
            BagController.Instance.DemountItem(itemID, CallBck);
        }
    }

    protected override void Enter(PointerEventData eventData)
    {
        //eventData.dragging 是否处于拖动状态， 鼠标按下，并且再移动
        if (eventData.dragging) return;
        TipsUI.Instance.ShowTips(itemID, TipsUI.ItemGridType.Player, transform.position);
    }

    /// <summary>
    /// 设置是否被选择，被选择时显示边框
    /// </summary>
    public void SetSelect(bool selected){
        border.SetActive(selected);
    }

    void CallBck(bool isFinish, string message)
    {
        //暂时测试使用
        if (isFinish)
        {
            Debug.Log("完成了： " + message);
        }
        else
        {
            Debug.LogError(message);
        }

    }

}
