using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 存储的是商店的基本属性，和商店里的所有的装备
/// </summary>
[System.Serializable]
public class ShopData {

    #region 单例

    private static ShopData instance;//从文件中反序列化来的

    public static ShopData Instance {
        get {
            if (instance == null) {
                instance = new ShopData();
            }
            return instance;
        }
    }
    private ShopData() { }

    #endregion

    #region 私有变量

    [SerializeField]
    private List<EquipmentData> items;//当前的所有的装备
    private int willBuyID = -1;//想要买的装备ID
    #endregion

    #region 属性

    public List<EquipmentData> Items {
        get {
            return items;
        }
        set {
            items = value;
        }
    }

    public int WillBuyID {
        get {
            return willBuyID;
        }
        set {
            int oldValue = willBuyID;
            willBuyID = value;
            if (oldValue != value) {
                UpdatePanel();
            }
        }
    }
    #endregion

    #region 提供一些商店装备的方法

    /// <summary>
    /// 使用ID访问商店中的装备
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public EquipmentData GetItem(int id) {
        for (int i = 0; i < items.Count; i++) {
            if (items[i].Id == id) {
                return items[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 添加装备
    /// </summary>
    public void AddItem(EquipmentData data) {
        items.Add(data);
        //数据变了，通知界面
        UpdatePanel();
    }

    /// <summary>
    /// 删除装备
    /// </summary>
    /// <param name="data"></param>
    public void RemoveItem(EquipmentData data) {
        //判断data是否在Items里
        if (items.Contains(data)) {
            items.Remove(data);
        }
        //数据变了，通知界面
        UpdatePanel();
    }

    /// <summary>
    /// 删除指定ID的装备
    /// </summary>
    /// <param name="id"></param>
    public void RemoveItem(int id) {
        EquipmentData data = GetItem(id);
        if (data != null) {
            RemoveItem(data);
        }
    }

    public void ItemsSort() {
        items.Sort(ItemSort);
        //物品之间的顺序发生了改变， 通知界面更新
        UpdatePanel();
    }

    //根据商店里的装备生成一个新的装备
    public EquipmentData GenerateEquipment(int id) {
        EquipmentData data = GetItem(id);
        data = data.Copy();
        return data;
    }
    #endregion

    /// <summary>u
    /// 通知界面更新
    /// </summary>
    public event Action updateEvent; //当数据改变时通知界面更新
    void UpdatePanel() {
        if (updateEvent != null) {
            updateEvent();
        }
    }

    /// <summary>
    /// 物品的排序，以物品的类型排
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    int ItemSort(EquipmentData a, EquipmentData b) {
        int tempA = (int)a.Type;
        int tempB = (int)b.Type;
        if (tempA < tempB) {
            return -1;
        }
        else if (tempA > tempB) {
            return 1;
        }
        else {
            return 0;
        }
    }
}
