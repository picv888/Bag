using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 连接数据库的类，暂时使用本地JSON数据作为数据库
/// </summary>
public class DataBase {
    #region 单例
    private static DataBase instance;

    public static DataBase Instance {
        get {
            if (null == instance) {
                instance = new DataBase();
            }
            return instance;
        }
    }

    private DataBase() { }

    #endregion

    //从数据库获取所有的装备模板
    private List<EquipmentData> equipmentTemplate;
    public List<EquipmentData> GetEquipmentTemplate() {
        if (equipmentTemplate == null) {
            string json = FileTools.ReadJson(Application.streamingAssetsPath + "/" + "Json/ItemTemps.json");
            equipmentTemplate = JsonUtility.FromJson<Serialization<EquipmentData>>(json).ToList();
        }
        return equipmentTemplate;
    }

    //从数据库获取所有套装的属性
    private List<SuitData> suidDatas;
    private List<SuitData> GetSuitDatas() {
        if (suidDatas == null) {
            string json = FileTools.ReadJson(Application.streamingAssetsPath + "/" + "Json/SuitTemps.json");
            suidDatas = JsonUtility.FromJson<Serialization<SuitData>>(json).ToList();
        }
        return suidDatas;
    }

    //从数据库获取世界的所有装备
    private List<EquipmentData> allEquipments;
    public List<EquipmentData> GetAllEquipmentInTheWorld() {
        if (allEquipments == null) {
            string json = FileTools.ReadJson(Application.streamingAssetsPath + "/" + "Json/AllEquipmentInTheWorld.json");
            if (string.IsNullOrEmpty(json)) {
                allEquipments = new List<EquipmentData>();
            }
            else {
                allEquipments = JsonUtility.FromJson<Serialization<EquipmentData>>(json).ToList();
            }
        }
        return allEquipments;
    }

    //保存游戏世界里的所有装备到数据库
    public void SaveAllEquipmentInTheWorld() {
        List<EquipmentData> list = GetAllEquipmentInTheWorld();
        if (list != null) {
            string json = JsonUtility.ToJson(new Serialization<EquipmentData>(list), true);
            FileTools.WriteJson(Application.streamingAssetsPath + "/" + "Json/AllEquipmentInTheWorld.json", json);
        }
    }

    //判断装备是否存在在这个世界
    private static int searchIndex;//记录上次搜索到时的index 提高搜索性能
    public bool IsEquipmentInTheWorld(EquipmentData e) {
        List<EquipmentData> eList = GetAllEquipmentInTheWorld();
        for (int i = 0; i < eList.Count; i++) {
            int currentSearch = (i + searchIndex) % eList.Count;//当前搜索index
            EquipmentData searchE = eList[currentSearch];
            if (searchE.Equals(e)) {
                searchIndex = currentSearch;
                return true;
            }
        }
        searchIndex = 0;
        return false;
    }

    //过滤掉世界上不存在的装备
    public void RemoveDoNotExist(List<EquipmentData> list) {
        List<EquipmentData> willRemove = new List<EquipmentData>();
        foreach (var item in list) {
            if (!IsEquipmentInTheWorld(item)) {
                willRemove.Add(item);
            }
        }
        foreach (var item in willRemove) {
            list.Remove(item);
        }
    }

    //添加一个装备到游戏世界
    public bool AddEquimentToTheWorld(EquipmentData data) {
        List<EquipmentData> eList = GetAllEquipmentInTheWorld();
        //如果这个装备已经在这个世界上
        if (IsEquipmentInTheWorld(data)) {
            return false;
        }

        //原本世界上没有装备，新装备ID设为0
        if (eList.Count == 0) {
            data.Id = 0;
        }
        else {
            //Id相比最后一个装备的ID + 1
            EquipmentData lastEqui = eList[eList.Count - 1];
            Debug.Log("lastEqui ID:" + lastEqui.Id);
            data.Id = lastEqui.Id + 1;
            Debug.Log("lastEqui ID:" + data.Id);
        }
        eList.Add(data);

        return true;
    }

    //删除一个游戏世界的装备
    public void RemoveEquipmentInTheWorld(EquipmentData data) {
        List<EquipmentData> eList = GetAllEquipmentInTheWorld();
        for (int i = 0; i < eList.Count; i++) {
            if (data.Equals(eList[i])) {
                eList.RemoveAt(i);
                return;
            }
        }
    }

    //从数据库获取玩家的数据
    public PlayerData GetPlayerData() {
        string playerJson = FileTools.ReadJson(Application.streamingAssetsPath + "/Json/PlayerData.json");
        PlayerData playerData;
        if (string.IsNullOrEmpty(playerJson)) {
            playerData = PlayerData.Instance;
        }
        else {
            playerData = JsonUtility.FromJson<PlayerData>(playerJson);
        }
        //过滤掉世界上不存在的装备
        RemoveDoNotExist(playerData.Items);
        return playerData;
    }

    //把玩家的数据保存到数据库
    public void SavePlayerData() {
        string playerJson = JsonUtility.ToJson(PlayerData.Instance, true);
        FileTools.WriteJson(Application.streamingAssetsPath + "/Json/PlayerData.json", playerJson);
    }

    //从数据库获取背包的数据
    public BagData GetBagData() {
        string bagJson = FileTools.ReadJson(Application.streamingAssetsPath + "/Json/Bag.json");
        BagData bagData;
        if (string.IsNullOrEmpty(bagJson)) {
            bagData = BagData.Instance;
        }
        else {
            bagData = JsonUtility.FromJson<BagData>(bagJson);
        }
        //过滤掉世界上不存在的装备
        RemoveDoNotExist(bagData.Items);
        return bagData;
    }

    //把背包的数据保存到数据库
    public void SaveBagData() {
        string bagJson = JsonUtility.ToJson(BagData.Instance, true);
        FileTools.WriteJson(Application.streamingAssetsPath + "/Json/Bag.json", bagJson);
    }

    //根据套装ID获取套装属性
    public SuitData GetSuitData(int suitID) {
        List<SuitData> suits = GetSuitDatas();
        for (int i = 0; i < suits.Count; i++) {
            SuitData suit = suits[i];
            if (suitID == suit.Id) {
                return suit;
            }
        }
        return null;
    }

    //根据套装ID获取套装的所有部件，返回所有的部件的名字组成的数组
    public List<string> GetNumberOfComponentForSuit(int suidID) {
        List<EquipmentData> equipments = GetEquipmentTemplate();
        List<string> componentsNameArr = new List<string>();
        for (int i = 0; i < equipments.Count; i++) {
            EquipmentData equipment = equipments[i];
            if (equipment.SuitID == suidID) {
                componentsNameArr.Add(equipment.ItemName);
            }
        }
        return componentsNameArr;
    }
}
