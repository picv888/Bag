using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    private PlayerGridUI[] grids;//所有的装备栏
    //所有显示属性的Text
    private Text inspectorText;

    void Awake() {
        //在子物体里获取所有的GridBase这个组件，返回的是一个数组
        grids = gameObject.GetComponentsInChildren<PlayerGridUI>();
        inspectorText = transform.Find("Message/Scroll View/Viewport/InspectorText").GetComponent<Text>();
    }

    private void Start() {
        PlayerData.Instance.updateEvent += UpdatePanel;

        UpdatePanel();
    }

    private void OnDestroy() {
        PlayerData.Instance.updateEvent -= UpdatePanel;
    }

    /// <summary>
    /// 更新界面的方法
    /// </summary>
    void UpdatePanel() {
        //把人物身上装备的物品显示， 所有属性显示

        //先把所有的格子清空
        for (int i = 0; i < grids.Length; i++) {
            grids[i].UpdateItem(-1, "");
        }

        //再把人物身上装备显示在对应的格子上
        for (int i = 0; i < grids.Length; i++) {
            PlayerGridUI grid = grids[i];

            for (int j = 0; j < PlayerData.Instance.Items.Count; j++) {
                //当格子的装备与人物数据里的装备的类型是一致时，证明该装备应该放在这个格子上
                if (grid.gridType == PlayerData.Instance.Items[j].Type) {
                    grid.UpdateItem(PlayerData.Instance.Items[j].Id, PlayerData.Instance.Items[j].IconName);
                }
            }
        }

        string inspectorString = "";
        inspectorString += "攻击力：" + PlayerData.Instance.Atk + "<color=\"green\"> + " + PlayerData.Instance.AddAtk + "</color>\n";
        inspectorString += "防御力：" + PlayerData.Instance.Def + "<color=\"green\"> + " + PlayerData.Instance.AddDef + "</color>\n";
        inspectorString += "暴击率：" + PlayerData.Instance.Thump + "<color=\"green\"> + " + PlayerData.Instance.AddThump + "</color>\n";
        inspectorString += "生命值：" + PlayerData.Instance.Hp + "<color=\"green\"> + " + PlayerData.Instance.AddHp + "</color>\n";
        inspectorString += "魔法值：" + PlayerData.Instance.Mp + "<color=\"green\"> + " + PlayerData.Instance.AddMp + "</color>\n";
        inspectorString += "怒气值：" + PlayerData.Instance.Anger + "<color=\"green\"> + " + PlayerData.Instance.AddAnger + "</color>\n";

        List<SuitData> suits = PlayerData.Instance.Suits;
        if (suits.Count > 0) {
            for (int i = 0; i < suits.Count; i++) {
                inspectorString += "\n\n" + suits[i].ToString();
            }
        }

        inspectorText.text = inspectorString;
        ////加上套装属性

        SetGridSelected(PlayerData.Instance.WillDressType);
    }

    /// <summary>
    /// 设置对应类型的装备格子为被选择状态（显示边框）
    /// </summary>
    public void SetGridSelected(EquipmentType equipmentType) {
        for (int i = 0; i < grids.Length; i++) {
            PlayerGridUI grid = grids[i];
            if (equipmentType == grid.gridType) {
                grid.SetSelect(true);
            }
            else {
                grid.SetSelect(false);
            }
        }
    }
}
