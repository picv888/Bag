using UnityEngine;
using System.Collections;
using System;

public class ShopController {
    #region 单例

    private static ShopController instance;

    public static ShopController Instance {
        get {
            if (instance == null) {
                instance = new ShopController();
            }
            return instance;
        }
    }

    private ShopController() { }

    #endregion

    public void BuyEquipment(int Id, Action<bool, string> callBack) {
        Debug.Log("BuyEquipment");
        NoticeUI.Instance.ShowNotice("是否够买这个装备？", null, () => Buy(Id, callBack));
    }

    private void Buy(int Id, Action<bool, string> callBack) {
        Debug.Log("Buy");
        EquipmentData data = ShopData.Instance.GetItem(Id);
        //判断是否有这个装备
        if (data == null) {
            if (callBack != null) {
                callBack(false, "商店里没有这个装备");
            }
        }
        //判断玩家的钱够不够
        else if (BagData.Instance.Money < data.PriceBuy) {
            if (callBack != null) {
                callBack(false, "购买失败，没钱玩什么游戏！！！");
            }
        }
        //判断玩家的负重够不够
        else if (BagData.Instance.CurrentCapacity + data.Weight > BagData.Instance.MaxCapacity) {
            if (callBack != null) {
                callBack(false, "购买失败，您负重快满了，装不下啦");
            }
        }
        else {
            //扣钱
            BagData.Instance.MinusMoney(data.PriceBuy);
            //生成装备，添加到世界装备，使装备ID更新，然后添加到玩家背包
            EquipmentData dataNew = ShopData.Instance.GenerateEquipment(Id);
            DataBase.Instance.AddEquimentToTheWorld(dataNew);
            Debug.Log("买的新装备ID："+ dataNew.Id);
            BagData.Instance.AddItem(dataNew);
            //回调购买成功
            if (callBack != null) {
                callBack(true, "购买成功");
            }
        }
    }
}

