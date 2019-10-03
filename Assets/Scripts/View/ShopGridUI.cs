using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// 商店物品格子
/// </summary>
////
public class ShopGridUI : GridBase {
    public Text itemNameText;
    public Text moneyText;
    public RectTransform borderTrans;
    public ShopUI shopUI;

    protected override void Awake() {
        base.Awake();
        if (scriptEnv != null) {
            luaClick = scriptEnv.Get<Action<PointerEventData>>("Click");
            luaClick = scriptEnv.Get<Action<PointerEventData>>("Click");
            luaClick = scriptEnv.Get<Action<PointerEventData>>("Click");
        }
        if (luaAwake != null) {
            luaAwake();
            return;
        }
        gameObject.tag = "ShopGrid";
        itemNameText = rectTransform.Find("ItemName").GetComponent<Text>();
        moneyText = rectTransform.Find("MoneyText").GetComponent<Text>();
        borderTrans = rectTransform.Find("GridBorder") as RectTransform;
        shopUI = rectTransform.GetComponentInParent<ShopUI>();
    }

    protected override void Click(PointerEventData eventData) {
        //如果格子里面有装备，记录玩家将要买的装备ID，如果没有，记录ID为-1
        EquipmentData data = ShopData.Instance.GetItem(itemID);
        ShopData.Instance.WillBuyID = data == null ? -1 : itemID;
    }

    //重写，使方法实现为空，使商店装备不能拖动
    protected override void BeginDrag(PointerEventData eventData) {

    }

    //重写，使方法实现为空，使商店装备不能拖动
    protected override void Drag(PointerEventData eventData) {

    }

    //重写，使方法实现为空，使商店装备不能拖动
    protected override void EndDrag(PointerEventData eventData) {

    }

    //重写该方法，鼠标放在装备上时显示装备属性
    private Action<PointerEventData> luaEnter;
    protected override void Enter(PointerEventData eventData) {
        //eventData.dragging 是否处于拖动状态， 鼠标按下，并且再移动
        if(eventData.dragging)
            return;
        EquipmentData data = ShopData.Instance.GetItem(itemID);
        TipsUI.Instance.ShowTips(data, transform.position);
    }

    public void SetSelected(bool selected) {
        borderTrans.gameObject.SetActive(selected);
    }

    /// <summary>
    /// 更新自己本身的物品
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="iconName"></param>
    public void UpdateItem(int itemID, string iconName, string nameText, int price) {
        if(this.itemID == itemID && itemID >= 0) {
            return;
        }

        this.itemID = itemID;

        if(itemID < 0)//没有物品
        {
            image.enabled = false;
            itemNameText.enabled = false;
            moneyText.enabled = false;
        }
        else {
            image.enabled = true;
            itemNameText.enabled = true;
            moneyText.enabled = true;
            if(image.sprite == null || image.sprite.name != iconName) {
                Sprite sp = Resources.Load<Sprite>("Texture/Icon/" + iconName);
                image.sprite = sp;
            }
            itemNameText.text = nameText;
            moneyText.text = string.Format("价格:{0}", price);
        }
    }
}
