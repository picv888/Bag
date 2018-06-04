using UnityEngine;
using System.Collections;

/// <summary>
/// 表示套装属性
/// </summary>
[System.Serializable]
public class SuitData : ItemData {
    [SerializeField]
    private string introduction;
    public override string ToString() {
        return string.Format("{0}: \n攻击力 + {1}\n防御力 + {2}\n暴击率 + {3}\n生命值 + {4}\n魔法值 + {5}\n怒气值 + {6}\n{7}", ItemName, Atk, Def, Thump, Hp, Mp, Anger, introduction);
    }
}
