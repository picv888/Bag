using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : LuaUIBehaviour {
    private RectTransform rectTransform;
    private GridBase[] grids;
    private Text weightText;
    private Text pageText;
    private Text moneyText;
    private Button rightButton;
    private Button leftButton;
    private Button clearUpButton;//整理按钮
    private Button sellAllBtn;//一键出售按钮

    private int currentNum;//当前页数
    private int maxNum;//最大页数， 根据背包里物品数量与当前有多少个格子比较

    protected override void Awake() {
        base.Awake();
        if (luaAwake == null) {
            rectTransform = transform as RectTransform;
            //获取所有的格子
            grids = gameObject.GetComponentsInChildren<GridBase>();
            weightText = rectTransform.Find("CapacityText").GetComponent<Text>();
            pageText = rectTransform.Find("PageText").GetComponent<Text>();
            moneyText = rectTransform.Find("MoneyText").GetComponent<Text>();
            rightButton = rectTransform.Find("RightButton").GetComponent<Button>();
            leftButton = rectTransform.Find("LeftButton").GetComponent<Button>();
            clearUpButton = rectTransform.Find("ClearUpButton").GetComponent<Button>();
            sellAllBtn = rectTransform.Find("SellAllBtn").GetComponent<Button>();

            //按钮注册事件
            rightButton.onClick.AddListener(RightClick);
            leftButton.onClick.AddListener(LeftClick);
            clearUpButton.onClick.AddListener(ClearUpClick);
            sellAllBtn.onClick.AddListener(SellAllBtnClick);
        }
    }


    protected override void Start() {
        base.Start();
        if (luaStart == null) {
            BagData.Instance.updateEvent += UpdatePanel;
            currentNum = 1;//界面一开始，当前页是1
            UpdatePanel();
        }
    }

    /// <summary>
    /// 更新界面
    /// </summary>
    void UpdatePanel() {
        //更新当前页数的物品， 更新当前页数， 更新当前的负重

        //计算当前的最大页数
        maxNum = (int)Mathf.Ceil(BagData.Instance.Items.Count / (float)grids.Length);

        weightText.text = "负重：" + BagData.Instance.CurrentCapacity + "/" +
            BagData.Instance.MaxCapacity;
        pageText.text = currentNum + "/" + maxNum;
        moneyText.text = string.Format("金币:{0}", BagData.Instance.Money);

        //显示当前的页数的物品
        //根据当前页数，确定第一个位置应该排背包数据的里的第几个索引（起始索引）
        //模拟 格子: 20   当第一页是起始索引为0， 当第二页时起始索引为20
        int startIndex = (currentNum - 1) * grids.Length;//（当前页数 - 1） * 格子数量

        //把从起始索引开始，依次的把物品放在对应的格子上
        for (int i = 0; i < grids.Length; i++) {
            //当i= 0时，证明是第一个格子，对应的物品索引 startIndex = startIndex + i
            //当i= 1时，证明是第二各格子，对应的物品索引 startIndex + 1 =  startIndex + i
            //....
            //当i = grids.Length - 1时， 最后一个格子， 对应的物品索引 startIndex + grids.Length - 1 =  startIndex + i

            //如果startIndex + i 超出了物品的数量， 证明这个格子没有物品
            //如果startIndex + i 没有超出物品的数量， 这个这个格子有物品
            if (startIndex + i >= BagData.Instance.Items.Count) {
                //超出了物品的数量，该格子没有物品
                grids[i].UpdateItem(-1, "");
            }
            else {
                grids[i].UpdateItem(BagData.Instance.Items[startIndex + i].Id,
                            BagData.Instance.Items[startIndex + i].IconName);
            }
        }
    }


    /// <summary>
    /// 翻页的右按钮
    /// </summary>
    void RightClick() {
        //判断是当前页是是最后一页
        if (currentNum < maxNum) {
            //不是最后一页
            currentNum++;
            UpdatePanel();//当前页数变了，需要更新一下界面的显示
        }
    }

    /// <summary>
    /// 翻页的左按钮
    /// </summary>
    void LeftClick() {
        //判断当前页数是不是第一页
        if (currentNum > 1) {
            currentNum--;
            UpdatePanel();//当前页数变了，需要更新一下界面的显示
        }
    }

    /// <summary>
    /// 整理按钮
    /// </summary>
    void ClearUpClick() {
        BagController.Instance.ClearUp();
    }

    /// <summary>
    /// 一键出售按钮点击
    /// </summary>
    void SellAllBtnClick() {
        List<EquipmentData> equipments = BagData.Instance.Items;
        if (equipments.Count == 0) {
            //背包里没有装备
            NoticeUI.Instance.ShowNotice("你的背包什么也没有", null);
            return;
        }
        NoticeUI.Instance.ShowNotice("一键出售背包里的所有物品", () => Debug.Log("取消出售了"), SellAllSureCallBack);
    }

    //一键出售提示框，点击确认按钮后的回调
    private void SellAllSureCallBack() {
        List<EquipmentData> equipments = BagData.Instance.Items;
        if (equipments.Count == 0) {
            //背包里没有装备
            NoticeUI.Instance.ShowNotice("你的背包什么也没有", null);
            return;
        }
        BagData.Instance.SellAllItem();
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        if (luaOnDestroy == null) {
            BagData.Instance.updateEvent -= UpdatePanel;
        }
    }
}