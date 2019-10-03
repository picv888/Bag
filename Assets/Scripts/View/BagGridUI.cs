using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using XLua;


public delegate void Func1(string a, string b, string c, string d);

/// <summary>
/// 表示背包的格子
/// </summary>
public class BagGridUI : GridBase {

    protected override void Awake() {
        base.Awake();
        gameObject.tag = "BagGrid";
    }


    protected override void Click(PointerEventData eventData) {
        if (luaClick != null) {
            luaClick(eventData);
            return;
        }
        if (itemID < 0) return;
        //按右键穿上格子里的装备
        if (eventData.button == PointerEventData.InputButton.Right) {
            BagController.Instance.EquipmentItem(itemID, BagData.Instance.GetItem(itemID).Type, CallBck);
        }
    }

    public int sellItemID = -1;//将要卖出、丢弃的装备ID，点击了确认按钮后要用来和当前格子的装备ID比较
    protected override void EndDrag(PointerEventData eventData) {
        base.EndDrag(eventData);

        if (luaEndDrag != null) {
            luaEndDrag(eventData);
            return;
        }

        if (itemID < 0) return;
        GameObject g = eventData.pointerCurrentRaycast.gameObject;
        if (g != null && g.CompareTag("ShopGrid")) {
            //当拖到商店区域时
            //卖出物品
            sellItemID = itemID;
            EquipmentData data = BagData.Instance.GetItem(sellItemID);
            string noticeStr = string.Format("确定卖出这个装备吗？\n你将获得{0}金币", data.PriceSell);
            NoticeUI.Instance.ShowNotice(noticeStr, () => Debug.Log("不卖了" + sellItemID),
                                         () => SellSureCallback(sellItemID));
        }
        else if (g != null && g.CompareTag("PlayerGrid")) {
            Debug.Log("装备物品");
            //获取到鼠标当前检测到的装备栏的类型
            PlayerGridUI grid = eventData.pointerCurrentRaycast.gameObject.GetComponent<PlayerGridUI>();
            BagController.Instance.EquipmentItem(itemID, grid.gridType, CallBck);
        }
        else if (g == null) {
            //丢弃物品
            int sellItemID = itemID;
            NoticeUI.Instance.ShowNotice("确定丢弃这个装备吗？", () => Debug.Log("不丢了" + sellItemID),
                                         () => AbandonSureCallback(sellItemID));
        }
        //当你拖动背包的装备时，对应能装备该物品的装备栏要提示
        BagController.Instance.EndDragItem();
    }

    protected override void Drag(PointerEventData eventData) {
        base.Drag(eventData);
        if (luaDrag != null) {
            luaDrag(eventData);
            return;
        }
        //当你拖动背包的装备时，对应能装备该物品的装备栏要提示
        BagController.Instance.DragingItem(itemID, null);
    }

    protected override void Enter(PointerEventData eventData) {
        if (luaEnter != null) {
            luaEnter(eventData);
            return;
        }
        //eventData.dragging 是否处于拖动状态， 鼠标按下，并且再移动
        if (eventData.dragging) return;
        TipsUI.Instance.ShowTips(itemID, TipsUI.ItemGridType.Bag, transform.position);
    }

    //卖出的确认回调，sellItemID为点击前确定的卖出的装备ID
    private void SellSureCallback(int sellItemID) {
        if (sellItemID != this.itemID) {
            NoticeUI.Instance.ShowNotice("卖出失败，物品不存在", null);
        }
        else {
            BagController.Instance.SellItem(sellItemID, CallBck);
        }
    }

    //丢弃装备确认回调，sellItemID为点击前确定的卖出的装备ID
    private void AbandonSureCallback(int sellItemID) {
        if (sellItemID != this.itemID) {
            NoticeUI.Instance.ShowNotice("丢弃失败，物品不存在", null);
        }
        else {
            BagController.Instance.AbandonItem(sellItemID, CallBck);
        }
    }

    void CallBck(bool isFinish, string message) {
        //暂时测试使用
        if (isFinish) {
            Debug.Log("完成了： " + message);
        }
        else {
            Debug.Log(message);
        }
    }
}