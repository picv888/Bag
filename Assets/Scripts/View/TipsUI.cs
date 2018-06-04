using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsUI : MonoBehaviour {

    private Text nameText;
    private Text typeText;
    private Text atkText;
    private Text defText;
    private Text thumpText;
    private Text hpText;
    private Text mpText;
    private Text angerText;
    private Text weightText;
    private Text priceText;

    private static TipsUI instance;

    public static TipsUI Instance {
        get {
            /*
            if (null == instance)
            {
                instance = GameObject.Find("Tips").GetComponent<TipsUI>();
            }
            */
            return instance;
        }
    }

    // Use this for initialization
    void Awake () {
        instance = this;

        nameText = transform.Find("NameText").GetComponent<Text>();
        typeText = transform.Find("TypeText").GetComponent<Text>();
        atkText = transform.Find("AtkText").GetComponent<Text>();
        defText = transform.Find("DefText").GetComponent<Text>();
        thumpText = transform.Find("ThumpText").GetComponent<Text>();
        hpText = transform.Find("HpText").GetComponent<Text>();
        mpText = transform.Find("MpText").GetComponent<Text>();
        angerText = transform.Find("AngerText").GetComponent<Text>();
        weightText = transform.Find("WeightText").GetComponent<Text>();
        priceText = transform.Find("PriceBuyText").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Start () {
        gameObject.SetActive(false);
	}

    /// <summary>
    /// 显示物品信息, 
    /// </summary>
    /// <param name="id">物品的id</param>
    /// <param name="type">物品在哪</param>
    /// <param name="position">物品的位置</param>
    public void ShowTips(int id, ItemGridType type, Vector3 position)
    {
        EquipmentData data = null;
        if (type == ItemGridType.Bag)
        {
            data = BagData.Instance.GetItem(id);
        }
        else if (type == ItemGridType.Player)
        {
            data = PlayerData.Instance.GetItem(id);
        }

        if (data != null)
        {
            gameObject.SetActive(true);
            nameText.text = "名字：" + data.ItemName;
            typeText.text = "类型：" + EquipmentData.GetTypeName(data.Type);
            atkText.text = "攻击力： +" + data.Atk.ToString();
            defText.text = "防御力： +" + data.Def.ToString();
            thumpText.text = "暴击率： +" + data.Thump.ToString();
            hpText.text = "生命值： +" + data.Hp.ToString();
            mpText.text = "魔法值： +" + data.Mp.ToString();
            angerText.text = "怒气值： +" + data.Anger.ToString();
            weightText.text = "负重：" + data.Weight.ToString();
            priceText.text = "价值：" + data.PriceSell;//显示玩家把装备卖出去可以得到多少钱

            transform.position = position;

            Vector2 pivot;
            //鼠标偏右
            if (Input.mousePosition.x > Screen.width / 2f)
            {
                pivot.x = 1;
            }
            else
            {
                pivot.x = 0;
            }

            //鼠标偏上
            if (Input.mousePosition.y > Screen.height / 2f)
            {
                pivot.y = 1;
            }
            else
            {
                pivot.y = 0;
            }
            (transform as RectTransform).pivot = pivot;
        }
    }

    /// <summary>
    /// 显示物品信息, 直接传物品进来
    /// </summary>
    public void ShowTips(EquipmentData data, Vector3 position) {
        if (data != null) {
            gameObject.SetActive(true);
            nameText.text = "名字：" + data.ItemName;
            typeText.text = "类型：" + EquipmentData.GetTypeName(data.Type);
            atkText.text = "攻击力： +" + data.Atk.ToString();
            defText.text = "防御力： +" + data.Def.ToString();
            thumpText.text = "暴击率： +" + data.Thump.ToString();
            hpText.text = "生命值： +" + data.Hp.ToString();
            mpText.text = "魔法值： +" + data.Mp.ToString();
            angerText.text = "怒气值： +" + data.Anger.ToString();
            weightText.text = "负重：" + data.Weight.ToString();
            priceText.text = "价格："+data.PriceBuy;//显示购买装备的价格

            transform.position = position;

            Vector2 pivot;
            //鼠标偏右
            if (Input.mousePosition.x > Screen.width / 2f) {
                pivot.x = 1;
            }
            else {
                pivot.x = 0;
            }

            //鼠标偏上
            if (Input.mousePosition.y > Screen.height / 2f) {
                pivot.y = 1;
            }
            else {
                pivot.y = 0;
            }
            (transform as RectTransform).pivot = pivot;
        }
    }

    /// <summary>
    /// 隐藏属性显示栏
    /// </summary>
    public void HideTips()
    {
        gameObject.SetActive(false);
    }

    public enum ItemGridType
    {
        Bag,
        Player
    }
}
