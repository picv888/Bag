using UnityEngine;
using System.Collections;

[System.Serializable]
public class EquipmentData : ItemData {
    [SerializeField]
    protected float weight;//重量
    [SerializeField]
    protected int suitID;//套装ID, 大于0表示是套装部件
    [SerializeField]
    protected string prefabName;//预制体文件名字
    [SerializeField]
    protected EquipmentType type;//装备的类型

    public float Weight {
        get {
            return weight;
        }
    }

    public int SuitID {
        get {
            return suitID;
        }
    }

    public string PrefabName {
        get {
            return prefabName;
        }
    }

    public EquipmentType Type {
        get {
            return type;
        }
    }

    /// <summary>
    /// 是否是套装的部件
    /// </summary>
    public bool IsSuit() {
        return suitID > 0;
    }

    public static string GetTypeName(EquipmentType type) {
        string typeName = "";
        switch (type) {
            case EquipmentType.Weapon:
                typeName = "武器";
                break;
            case EquipmentType.Cap:
                typeName = "头盔";
                break;
            case EquipmentType.Armour:
                typeName = "铠甲";
                break;
            case EquipmentType.Belt:
                typeName = "腰带";
                break;
            case EquipmentType.Ring:
                typeName = "戒指";
                break;
            case EquipmentType.Headwear:
                typeName = "头饰";
                break;
            case EquipmentType.Necklace:
                typeName = "项链";
                break;
            case EquipmentType.Shoe:
                typeName = "靴子";
                break;
        }
        return typeName;
    }

    public EquipmentData Copy(){
        string json = JsonUtility.ToJson(this);
        EquipmentData e = JsonUtility.FromJson<EquipmentData>(json);
        return e;
    }

    public override bool Equals(object obj) {
        if (!(obj is EquipmentData)) {
            return false;
        }
        EquipmentData d = obj as EquipmentData;
        if (this.Id == d.Id && this.type == d.type && this.SuitID == d.SuitID) {
            return true;
        }
        return false;
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }
}
