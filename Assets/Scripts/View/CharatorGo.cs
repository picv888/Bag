using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharatorGo : MonoBehaviour {
    /*
     Weapon,//武器
    Cap,//头盔
    Armour,//铠甲
    Belt,//腰带
    Shoe,//鞋子
    Ring,//戒指
    Necklace,//项链
    Headwear,//头饰
    */

    /*
单手剑 模型名字Weapon开头
    Charactor-Female Black Elf 02/RigPelvis/RigSpine1/RigSpine2/RigRibcage/RigRArm1/RigRArm2/RigRArmPalm/Dummy Prop Right

衣服 模型名字Jacket开头
    Charactor-Female Black Elf 02

头饰 用脸部代替，模型Face开头
    Charactor-Female Black Elf 02/RigPelvis/RigSpine1/RigSpine2/RigRibcage/RigNeck/RigHead/Dummy Prop Face

帽子 模型Hat开头
    Charactor-Female Black Elf 02/RigPelvis/RigSpine1/RigSpine2/RigRibcage/RigNeck/RigHead/Dummy Prop Head

项链 用翅膀代替 Wings  
    Charactor-Female Black Elf 02/RigPelvis/RigSpine1/RigSpine2/RigRibcage/Dummy Prop Back

人物模型 名字Charactor-Female Black Elf 02
    */

    private Transform trans;
    private Transform weaponTrans;//装备头盔的位置
    private Transform armourTrans;//装备铠甲的位置
    private Transform headwearTrans;//头饰
    private Transform capTrans;//帽子
    private Transform NecklaceTrans;//项链
    private string equipmentTag = "___Equipment___";//设置装备的Tag，便于查找装备删除装备
    private int equipmentLayer = 8;//设置装备的层级，使人物专用摄像机能拍到装备

    private void Awake() {
        trans = transform;
        weaponTrans = trans.Find("Charactor-Female Black Elf 02/RigPelvis/RigSpine1/RigSpine2/RigRibcage/RigRArm1/RigRArm2/RigRArmPalm/Dummy Prop Right");
        armourTrans = trans.Find("Charactor-Female Black Elf 02");
        headwearTrans = trans.Find("Charactor-Female Black Elf 02/RigPelvis/RigSpine1/RigSpine2/RigRibcage/RigNeck/RigHead/Dummy Prop Face");
        capTrans = trans.Find("Charactor-Female Black Elf 02/RigPelvis/RigSpine1/RigSpine2/RigRibcage/RigNeck/RigHead/Dummy Prop Head");
        NecklaceTrans = trans.Find("Charactor-Female Black Elf 02/RigPelvis/RigSpine1/RigSpine2/RigRibcage/Dummy Prop Back");
    }

    private void Start() {
        PlayerData.Instance.updateGameObjectEquipment += UpdateGameObjectEquipment;
        UpdateAllEquipment();
    }

    //刷新所有的装备游戏物体
    private void UpdateAllEquipment() {
        List<EquipmentData> equipmentDatas = PlayerData.Instance.Items;
        Debug.Log("UpdateAllEquipment count:" + equipmentDatas.Count);
        for (int i = 0; i < equipmentDatas.Count; i++) {
            UpdateGameObjectEquipment(equipmentDatas[i], true);
        }
    }

    private void UpdateGameObjectEquipment(EquipmentData data, bool isDress) {
        if (data == null) {
            Debug.Log("没有装备数据");
            return;
        }
        Transform t = GetDressTransform(data.Type);
        if(t == null) {
            Debug.Log("作者有点懒，没有找这种类型的装备的模型");
            return;
        }
        //销毁原本的装备 游戏物体
        for (int i = 0; i < t.childCount; i++) {
            Transform chilT = t.GetChild(i);
            if (chilT.CompareTag(equipmentTag)) {
                //找到装备后销毁，跳出循环
                Destroy(chilT.gameObject);
                break;
            }
        }

        //穿上新的装备
        if (isDress) {
            if (string.IsNullOrEmpty(data.PrefabName)) {
                Debug.Log("这个装备没有预制体名字");
            }
            else {
                //启动协程获取装备预制体，穿上装备
                StartCoroutine(DressEquipment(data, t));
            }
        }
    }

    /// <summary>
    /// 异步获取装备预制体，穿上装备
    /// </summary>
    /// <returns>The equipment.</returns>
    /// <param name="data">装备数据</param>
    /// <param name="dressTransform">穿到哪个位置</param>
    IEnumerator DressEquipment(EquipmentData data, Transform dressTransform) {
        ResourceRequest request = Resources.LoadAsync<GameObject>("Prefabs/" + data.PrefabName);
        yield return request;
        if (request.asset == null) {
            Debug.Log("找不到这个装备的预制体");
            yield break;
        }
        if (!(request.asset is GameObject)) {
            Debug.Log("这个装备的预制体有错误");
            yield break;
        }
        GameObject prefab = request.asset as GameObject;
        GameObject go = Instantiate<GameObject>(prefab, dressTransform);
        //修改装备的layer，使人物专用摄像机能拍到装备
        go.layer = equipmentLayer;
        //设置标签，便于删除装备
        go.tag = equipmentTag;
        Debug.Log("装备游戏物体名字：" + go.name);
    }

    //根据装备类型获取装备的位置,如果返回null,这种类型的装备不能穿（没有找模型...）
    private Transform GetDressTransform(EquipmentType type) {
        Transform t;
        switch (type) {
            case EquipmentType.Weapon:
                t = weaponTrans;
                break;
            case EquipmentType.Armour:
                t = armourTrans;
                break;
            case EquipmentType.Headwear:
                t = headwearTrans;
                break;
            case EquipmentType.Cap:
                t = capTrans;
                break;
            case EquipmentType.Necklace:
                t = NecklaceTrans;
                break;
            default:
                t = null;
                break;
        }
        return t;
    }
}
