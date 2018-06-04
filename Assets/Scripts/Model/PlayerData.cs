using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerData {

    #region 单例

    private static PlayerData instance;//这个数据类的单例，从文件中反序列化来的

    public static PlayerData Instance {
        get {
            if (instance == null) {
                instance = new PlayerData();
            }
            return instance;
        }
        private set {
            if (value != null) {
                instance = value;
            }
        }

    }

    private PlayerData() { }

    //虽然是单例，但是还是可以序列化创建到一个对象，提供一个方法使用序列化出来的对象对单例进行初始化
    public static void InitForSerialize(PlayerData playerData) {
        PlayerData.Instance = playerData;
        PlayerData.Instance.UpdatePanel();
    }

    #endregion


    private List<SuitData> suits;//组成套装了增加的套装属性
    private EquipmentType willDressType = EquipmentType.None;//将要穿上的装备类型，拖动物品是根据装备类型使用属性设置这个值

    #region 可序列化的成员变量
    [SerializeField]
    private List<EquipmentData> items;//人物身上穿戴的装备
    [SerializeField]
    private float atk = 0;//基础攻击力
    [SerializeField]
    private float def = 0;//基础防御力
    [SerializeField]
    private float thump = 0;//基础暴击率
    [SerializeField]
    private float hp = 0;//基础血量
    [SerializeField]
    private float mp = 0;//基础魔法
    [SerializeField]
    private float anger = 0;//基础怒气



    //装备的属性加成都是装备的装备计算出来的
    private float addAtk;//装备攻击力加成
    private float addDef;//
    private float addThump;//
    private float addHp;//
    private float addMp;//
    private float addAnger;//

    #endregion

    #region 事件

    //当数据发生改变时，通过该事件通知界面
    public event Action updateEvent;

    //当人身上的装备发生改变时，通知玩家游戏物体更新装备,第一个参数是装备的数据，第二个参数表示： true:穿上 false:脱掉
    public event Action<EquipmentData, bool> updateGameObjectEquipment;

    #endregion

    #region 属性
    public List<EquipmentData> Items {
        get {
            if (items == null) {
                items = new List<EquipmentData>();
            }
            return items;
        }
    }

    public float Atk {
        get {
            return atk;
        }
    }

    public float Def {
        get {
            return def;
        }
    }

    public float Thump {
        get {
            return thump;
        }
    }

    public float Hp {
        get {
            return hp;
        }
    }

    public float Mp {
        get {
            return mp;
        }
    }

    public float Anger {
        get {
            return anger;
        }
    }

    public float AddAtk {
        get {
            return addAtk;
        }
    }

    public float AddDef {
        get {
            return addDef;
        }
    }

    public float AddThump {
        get {
            return addThump;
        }
    }

    public float AddHp {
        get {
            return addHp;
        }
    }

    public float AddMp {
        get {
            return addMp;
        }
    }

    public float AddAnger {
        get {
            return addAnger;
        }
    }

    public List<SuitData> Suits {
        get {
            return suits;
        }
    }

    public EquipmentType WillDressType {
        get {
            return willDressType;
        }
        set {
            EquipmentType old = willDressType;
            willDressType = value;
            if (old != value) {
                UpdatePanel();
            }
        }
    }

    #endregion

    #region 提供一些访问或删除装备的方法

    /// <summary>
    /// 通过装备的ID来访问装备
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public EquipmentData GetItem(int id) {
        for (int i = 0; i < items.Count; i++) {
            if (id == items[i].Id) {
                return items[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 通过装备的类型来访问装备
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public EquipmentData GetItem(EquipmentType type) {
        for (int i = 0; i < items.Count; i++) {
            if (type == items[i].Type) {
                return items[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 添加装备
    /// </summary>
    /// <param name="data"></param>
    public void AddItem(EquipmentData data) {
        items.Add(data);
        //数据发生变化，通知界面
        UpdatePanel();
        //通知更新人物模型的装备
        updateGameObjectEquipment(data, true);
    }

    /// <summary>
    /// 删除装备
    /// </summary>
    /// <param name="data"></param>
    public void RemoveItem(EquipmentData data) {
        if (items.Contains(data)) {
            items.Remove(data);
            //数据发生变化，通知界面
            UpdatePanel();
            //通知更新人物模型的装备
            updateGameObjectEquipment(data, false);
        }
    }

    /// <summary>
    /// 通过ID删除装备
    /// </summary>
    /// <param name="id"></param>
    public void RemoveItem(int id) {
        EquipmentData data = GetItem(id);
        RemoveItem(data);
    }

    /// <summary>
    /// 通过装备类型删除装备
    /// </summary>
    /// <param name="type"></param>
    public void RemoveItem(EquipmentType type) {
        EquipmentData data = GetItem(type);
        RemoveItem(data);
    }

    //获取人物已穿上的套装的名字组成的数组
    public List<string> GetNumberOfComponentForSuit(int suidID) {
        List<string> componentsNameArr = new List<string>();
        for (int i = 0; i < items.Count; i++) {
            EquipmentData temp = items[i];
            if (temp.SuitID == suidID) {
                componentsNameArr.Add(temp.ItemName);
            }
        }
        return componentsNameArr;
    }

    #endregion

    /// <summary>
    /// 计算装备的加成数据, 每次当数据发生改变的时候调用
    /// </summary>
    void UpdateAdditionData() {
        this.addAtk = 0;
        this.addDef = 0;
        this.addThump = 0;
        this.addHp = 0;
        this.addMp = 0;
        this.addAnger = 0;
        this.suits = new List<SuitData>();//套装加成
        //把每一件装备的加成数据加给实际的数据
        for (int i = 0; i < items.Count; i++) {
            EquipmentData item = items[i];
            this.addAtk += item.Atk;
            this.addDef += item.Def;
            this.addThump += item.Thump;
            this.addHp += item.Hp;
            this.addMp += item.Mp;
            this.addAnger += item.Anger;

            SuitData suitData = DataBase.Instance.GetSuitData(item.SuitID);
            //是套装的部件
            if (null != suitData) {
                int numHas = GetNumberOfComponentForSuit(item.SuitID).Count;
                int num = DataBase.Instance.GetNumberOfComponentForSuit(item.SuitID).Count;
                //组成套装了
                if (numHas == num) {
                    //并且还没有增加过该套装属性，则增加套装属性
                    if (!this.IsAddSuit(suitData.Id)) {

                        this.addAtk += suitData.Atk;
                        this.addDef += suitData.Def;
                        this.addThump += suitData.Thump;
                        this.addHp += suitData.Hp;
                        this.addMp += suitData.Mp;
                        this.addAnger += suitData.Anger;
                        this.suits.Add(suitData);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 是否已经增加这个套装属性
    /// </summary>
    bool IsAddSuit(int suitID) {
        for (int i = 0; i < suits.Count; i++) {
            if (suitID == suits[i].Id) {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// 通知界面改变
    /// </summary>
    void UpdatePanel() {
        //通知刷新界面前先重新计算属性加成
        UpdateAdditionData();
        if (updateEvent != null) {
            updateEvent();
        }
    }

}
