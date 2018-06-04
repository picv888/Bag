using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 作为数据的存储和初始化的一个管理器
/// </summary>
public class SaveManager
{

    #region 单例
    private static SaveManager instance;

    public static SaveManager Instance
    {
        get {
            if (null == instance)
            {
                instance = new SaveManager();
            }
            return instance;
        }
    }

    private SaveManager() { }

    #endregion

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        //初始化人物的数据
        PlayerData playerData = DataBase.Instance.GetPlayerData();
        PlayerData.InitForSerialize(playerData);

        //初始化背包数据
        BagData bagData = DataBase.Instance.GetBagData();
        BagData.InitForSerialize(bagData);

        //初始化商店数据，暂时使用所有装备模板代替（实际上应该只有一部分，不可能商店能卖所有的装备）
        ShopData.Instance.Items = DataBase.Instance.GetEquipmentTemplate();
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    public void SaveData()
    {
        DataBase.Instance.SavePlayerData();
        DataBase.Instance.SaveBagData();
        DataBase.Instance.SaveAllEquipmentInTheWorld();
    }
}
