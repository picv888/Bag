using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 存储的是背包的基本属性，和背包里的所有的装备
/// </summary>
[System.Serializable]
public class BagData {

    #region 单例

    private static BagData instance;//从文件中反序列化来的
    public static BagData Instance {
        get {
            if (instance == null) {
                instance = new BagData();
                instance.maxCapacity = 50f;
            }
            return instance;
        }
        private set {
            if (value != null) {
                instance = value;
            }
        }
    }
    private BagData() { }

    //虽然是单例，但是还是可以序列化创建到一个对象，提供一个方法使用序列化出来的对象对单例进行初始化
    public static void InitForSerialize(BagData bagData) {
        BagData.Instance = bagData;
        BagData.Instance.UpdatePanel();
    }

    #endregion

    #region 私有变量

    [SerializeField]
    private List<EquipmentData> items;//当前的所有的装备

    [SerializeField]
    private float maxCapacity = 0;//最大容量, 从文件中读取进来的

    private float currentCapacity;//当前容量, 根据当前背包的装备计算出来的

    [SerializeField]
    private int money;//金币,从文件中读取进来的

    #endregion

    #region 事件

    public event Action updateEvent;//定义一个事件，当数据改变时，调用事件通知界面更新

    #endregion

    #region 属性
    public float MaxCapacity {
        get {
            return maxCapacity;
        }
        private set{
            maxCapacity = value;
            if (maxCapacity < 0f) {
                maxCapacity = 0f;
            }
            UpdatePanel();
        }
    }

    public float CurrentCapacity {
        get {
            return currentCapacity;
        }
        private set{
            currentCapacity = value;
            if (currentCapacity < 0f) {
                currentCapacity = 0f;
            }
            UpdatePanel();
        }
    }

    public List<EquipmentData> Items {
        get {
            if (items == null) {
                items = new List<EquipmentData>();
            }
            return items;
        }
        private set {
            if (value != null) {
                items = value;
            }
            UpdatePanel();
        }
    }

    public int Money {
        get {
            return money;
        }
        private set {
            money = value;
            if (money < 0) {
                money = 0;
            }
            UpdatePanel();
        }
    }
    #endregion

    #region 提供一些操作背包装备的方法

    /// <summary>
    /// 使用ID访问背包中的装备
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public EquipmentData GetItem(int id) {
        for (int i = 0; i < Items.Count; i++) {
            if (Items[i].Id == id) {
                return Items[i];
            }
        }
        return null;
    }



    /// <summary>
    /// 添加装备
    /// </summary>
    public void AddItem(EquipmentData data) {
        Items.Add(data);
        //数据变了，通知界面
        UpdatePanel();
    }

    /// <summary>
    /// 删除装备
    /// </summary>
    public void RemoveItem(EquipmentData data) {
        //判断data是否在Items里
        if (Items.Contains(data)) {
            Items.Remove(data);
        }
        UpdatePanel();
    }

    /// <summary>
    /// 卖出背包里所有的装备
    /// </summary>
    public void SellAllItem() {
        int sumPrice = 0;
        for (int i = 0; i < this.Items.Count; i++) {
            sumPrice += Items[i].PriceSell;
        }
        Items = new List<EquipmentData>();
        BagData.Instance.AddMoney(sumPrice);
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
        UpdatePanel();
    }


    public void ItemsSort() {
        items.Sort(ItemSort);
        //物品之间的顺序发生了改变， 通知界面更新
        UpdatePanel();
    }


    #endregion

    #region 修改金币、最大负重的方法
    public void AddMoney(int add) {
        Money += add;

    }

    public void MinusMoney(int minus) {
        Money -= minus;
    }

    public void AddMaxCapacity(float add){
        MaxCapacity += add;
    }

    public void MinusCapacity(float minus){
        MaxCapacity -= minus;
    }

    #endregion

    public void UpdateCurrentCapacity() {
        currentCapacity = 0;
        //把每一件装备的负重累加在一起，就是当前的负重
        for (int i = 0; i < Items.Count; i++) {
            currentCapacity += Items[i].Weight;
        }
    }

    /// <summary>
    /// 通知界面更新
    /// </summary>
    void UpdatePanel() {
        //通知前先重新计算负重
        UpdateCurrentCapacity();
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
