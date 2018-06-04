using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BagGridUI : GridBase {

    protected override void Awake() {
        base.Awake();
        gameObject.tag = "BagGrid";
    }

    protected override void Click(PointerEventData eventData) {
        if (itemID < 0) return;
        if (eventData.button == PointerEventData.InputButton.Right) {
            Debug.Log("装备： " + itemID);
            BagController.Instance.EquipmentItem(itemID, BagData.Instance.GetItem(itemID).Type, CallBck);
        }
    }

    protected override void EndDrag(PointerEventData eventData) {
        if (itemID < 0) return;
        base.EndDrag(eventData);
        GameObject g = eventData.pointerCurrentRaycast.gameObject;
        if (g != null && g.CompareTag("ShopGrid"))
        {
            //当拖到商店区域时
            //卖出物品
            //遵守MVC应该写在BagController的，但是为了偷懒。。写在这懒得改了
            int sellItemID = itemID;
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
            //遵守MVC应该写在BagController的，但是为了偷懒。。写在这懒得改了
            int sellItemID = itemID;
            EquipmentData data = BagData.Instance.GetItem(sellItemID);
            NoticeUI.Instance.ShowNotice("确定丢弃这个装备吗？", () => Debug.Log("不丢了" + sellItemID),
                                         () => AbandonSureCallback(sellItemID));
        }
        //当你拖动背包的装备时，对应能装备该物品的装备栏要提示
        BagController.Instance.EndDragItem();
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

    protected override void Drag(PointerEventData eventData) {
        base.Drag(eventData);
        //当你拖动背包的装备时，对应能装备该物品的装备栏要提示
        BagController.Instance.DragingItem(itemID, null);
    }

    protected override void Enter(PointerEventData eventData) {
        //eventData.dragging 是否处于拖动状态， 鼠标按下，并且再移动
        if (eventData.dragging) return;
        TipsUI.Instance.ShowTips(itemID, TipsUI.ItemGridType.Bag, transform.position);
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