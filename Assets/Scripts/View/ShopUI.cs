using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour {
    private ShopGridUI[] grids;
    private Text pageText;
    private Button rightButton;
    private Button leftButton;
    private Button buyButton;

    private int currentNum;//当前页数
    private int maxNum;//最大页数， 根据背包里物品数量与当前有多少个格子比较

    void Awake() {
        //获取所有的格子
        grids = gameObject.GetComponentsInChildren<ShopGridUI>();
        
        pageText = transform.Find("PageText").GetComponent<Text>();
        rightButton = transform.Find("RightButton").GetComponent<Button>();
        leftButton = transform.Find("LeftButton").GetComponent<Button>();
        buyButton = transform.Find("BuyBtn").GetComponent<Button>();

        //按钮注册事件
        rightButton.onClick.AddListener(RightClick);
        leftButton.onClick.AddListener(LeftClick);
        buyButton.onClick.AddListener(BuyBtnClick);
    }

    private void Start() {
        ShopData.Instance.updateEvent += UpdatePanel;
        currentNum = 1;//界面一开始，当前页是1
        UpdatePanel();
    }

    private void OnDestroy() {
        ShopData.Instance.updateEvent -= UpdatePanel;
    }

    /// <summary>
    /// 更新界面
    /// </summary>
    void UpdatePanel() {
        //更新当前页数的物品， 更新当前页数， 更新当前的负重

        //计算当前的最大页数
        maxNum = (int)Mathf.Ceil(ShopData.Instance.Items.Count / (float)grids.Length);

        pageText.text = currentNum + "/" + maxNum;

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
            if (startIndex + i >= ShopData.Instance.Items.Count) {
                //超出了物品的数量，该格子没有物品
                grids[i].UpdateItem(-1, "", "", 0);
            }
            else {
                EquipmentData data = ShopData.Instance.Items[startIndex + i];
                grids[i].UpdateItem(data.Id, data.IconName, data.ItemName, data.PriceBuy);
            }
        }

        //使选中的格子的边框高亮
        SetGridSelected();
    }


    /// <summary>
    /// 翻页的右按钮
    /// </summary>
    void RightClick() {
        //判断是当前页是是最后一页
        if (currentNum < maxNum) {
            //不是最后一页
            currentNum++;
            ShopData.Instance.WillBuyID = -1;
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
            ShopData.Instance.WillBuyID = -1;
            UpdatePanel();//当前页数变了，需要更新一下界面的显示
        }
    }

    /// <summary>
    /// 购买按钮
    /// </summary>
    void BuyBtnClick() {
        int selectedGridID = ShopData.Instance.WillBuyID;
        if (selectedGridID >= 0) {
            ShopController.Instance.BuyEquipment(selectedGridID, CallBack);
        }
    }

    private void CallBack(bool isFinished, string str) {
        //输出购买情况
        Debug.Log(string.Format("{0}，{1}", isFinished, str));
        if (!isFinished) {
            NoticeUI.Instance.ShowNotice(str, null);
        }
    }

    //显示被选择的格子
    private void SetGridSelected() {
        //根据想要买的装备ID计算格子ID
        int selectedGridID = ShopData.Instance.WillBuyID;
        selectedGridID = selectedGridID % this.grids.Length;
        for (int i = 0; i < grids.Length; i++) {
            ShopGridUI gi = grids[i];
            gi.SetSelected(i == selectedGridID);
        }
    }
}
