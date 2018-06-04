using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该类用于存储装备、套装的基本属性
/// </summary>
[System.Serializable]
public class ItemData {
    #region 私有变量

    [SerializeField]
    protected int id;//作为物品的唯一标识
    [SerializeField]
    protected string itemName;//物品的名字
    [SerializeField]
    protected string iconName;//物品显示图片的名字
    [SerializeField]
    protected float atk;//攻击力加成
    [SerializeField]
    protected float def;//防御力加成
    [SerializeField]
    protected float thump;//暴击率加成
    [SerializeField]
    protected float hp;//血量加成
    [SerializeField]
    protected float mp;//加成
    [SerializeField]
    protected float anger;//怒气加成
    [SerializeField]
    protected int priceBuy;//买入加钱
    [SerializeField]
    protected int priceSell;//卖出的价钱
    #endregion

    #region 属性
    public virtual int Id {
        get {
            return id;
        }
        set{
            id = value;
        }
    }

    public virtual string ItemName {
        get {
            return itemName;
        }

    }

    public virtual string IconName {
        get {
            return iconName;
        }

    }

    public virtual float Atk {
        get {
            return atk;
        }

    }

    public virtual float Def {
        get {
            return def;
        }

    }

    public virtual float Thump {
        get {
            return thump;
        }

    }

    public virtual float Hp {
        get {
            return hp;
        }

    }

    public virtual float Mp {
        get {
            return mp;
        }

    }

    public virtual float Anger {
        get {
            return anger;
        }
    }

    public virtual int PriceBuy {
        get {
            return priceBuy;
        }
    }

    public  int PriceSell {
        get {
            return priceSell;
        }
    }
    #endregion
}
