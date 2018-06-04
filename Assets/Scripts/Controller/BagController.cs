using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagController {

    #region 单例

    private static BagController instance;

    public static BagController Instance {
        get {
            if (instance == null) {
                instance = new BagController();
            }
            return instance;
        }
    }

    private BagController() { }

    #endregion

    //第一个参数，是是否完成， 第二个参数， 传递过去的消息
    public delegate void CallBack(bool isFinish, string message);

    #region 处理数据的方法

    /// <summary>
    /// 卖出指定的ID的物品, 规定只能从背包卖
    /// 第二个参数是：当完成或者未完成时，回调调用自己的View中的方法。
    /// </summary>
    /// <param name="id"></param>
    public void SellItem(int id, CallBack callback) {
        //通过ID从背包中找到物品
        EquipmentData data = BagData.Instance.GetItem(id);

        //如果data是null， 背包中没有这个物品，id有问题
        if (data == null) {
            //通知调用这个方法的View 你的ID是有问题的吧。
            //Debug.LogError("通知调用这个方法的View 你的ID是有问题的吧。");
            if (callback != null) {
                callback(false, "ID有问题！");
            }
        }
        else {
            //删除物品
            BagData.Instance.RemoveItem(data);
            ////加钱
            BagData.Instance.AddMoney(data.PriceSell);
            if (callback != null) {
                callback(true, "卖出成功");
            }
        }
    }

    /// <summary>
    /// 装备物品
    /// </summary>
    /// 参数1: 要装备的物品的ID
    /// 参数2: 要装备的位置
    /// 参数3: View的回调函数(通知View的成功了还是失败)
    public void EquipmentItem(int id, EquipmentType gridType, CallBack callback) {
        //判断ID是否存在
        EquipmentData bagItem = BagData.Instance.GetItem(id);
        if (bagItem == null) {
            //背包里没有这个ID的物品
            if (callback != null) {
                callback(false, "背包里没有该ID的物品");
            }
        }
        else {
            //如果背包有这个物品，
            //判断该装备类型和你要装备的格子类型是否一致
            if (bagItem.Type == gridType)//装备类型与格子类型一致，可以装备
            {
                //1. 装备栏有物品， 替换
                //2. 装备栏没有物品， 直接装备

                //首先判断装备栏是否有物品
                EquipmentData playerItem = PlayerData.Instance.GetItem(gridType);
                if (playerItem == null) {
                    //证明该格子上没有装备
                    //先把该装备从背包数据里删除
                    BagData.Instance.RemoveItem(bagItem);
                    //再把物品添加到人物数据里
                    PlayerData.Instance.AddItem(bagItem);
                }
                else {
                    //证明该格子上有装备
                    //先把人物身上的装备先删除
                    PlayerData.Instance.RemoveItem(playerItem);
                    //再把背包里的装备删除
                    BagData.Instance.RemoveItem(bagItem);
                    //把人物身上的装备添加到背包里
                    BagData.Instance.AddItem(playerItem);
                    //把背包里的装备添加到人物身上
                    PlayerData.Instance.AddItem(bagItem);
                }

                if (callback != null) {
                    callback(true, "装备成功");
                }
            }
            else {
                if (callback != null) {
                    callback(false, "请把装备装到正确的位置");
                }
            }
        }
    }

    /// <summary>
    /// 拖动物品，角色装备窗口里对应能装备该物品的格子要提示
    /// </summary>
    public void DragingItem(int id, CallBack callBack) {
        //判断playerData里是否有这个id的装备
        EquipmentData data = BagData.Instance.GetItem(id);
        if (data == null)//空的时候，身上没有这个id的物品
        {
            if (callBack != null) {
                callBack(false, "人物身上没有该ID的物品");
            }
        }
        else {
            PlayerData.Instance.WillDressType = data.Type;
        }
    }

    /// <summary>
    /// 拖动物品结束，角色装备窗口取消提示
    /// </summary>
    public void EndDragItem() {
        PlayerData.Instance.WillDressType = EquipmentType.None;
    }

    /// <summary>
    /// 卸下指定的id的装备，数据一定在playerData里
    /// </summary>
    public void DemountItem(int id, CallBack callback) {
        //判断playerData里是否有这个id的装备
        EquipmentData data = PlayerData.Instance.GetItem(id);

        if (data == null)//空的时候，身上没有这个id的物品
        {
            if (callback != null) {
                callback(false, "人物身上没有该ID的物品");
            }
        }
        else {
            //先把该装备从人物身上删除
            PlayerData.Instance.RemoveItem(data);
            //再把该装备添加到背包数据里去
            BagData.Instance.AddItem(data);
            //通知界面成功
            if (callback != null) {
                callback(true, "成功卸载装备");
            }
        }
    }

    /// <summary>
    /// 丢弃指定的id的装备，数据一定在BagData里
    /// </summary>
    public void AbandonItem(int id, CallBack callback) {
        //判断playerData里是否有这个id的装备
        EquipmentData data = BagData.Instance.GetItem(id);

        if (data == null)//空的时候，身上没有这个id的物品
        {
            if (callback != null) {
                callback(false, "丢弃失败，背包里没有这个装备");
            }
        }
        else {
            //先把该装备从背包删除
            BagData.Instance.RemoveItem(data);
            //通知界面成功
            if (callback != null) {
                callback(true, "已丢弃");
            }
        }
    }


    /// <summary>
    /// 整理背包的方法
    /// </summary>
    public void ClearUp() {
        BagData.Instance.ItemsSort();
    }

    #endregion

}
