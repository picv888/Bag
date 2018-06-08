using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITest : MonoBehaviour {
    private void OnGUI() {
        if(GUILayout.Button("作弊:\n金币 + 10000")){
            BagData.Instance.AddMoney(10000); 
        }
        if (GUILayout.Button("作弊:\n金币 - 10000")) {
            BagData.Instance.MinusMoney(10000);
        }
        if (GUILayout.Button("作弊:\n最大负重 + 50")) {
            BagData.Instance.AddMaxCapacity(50f);
        }
        if (GUILayout.Button("作弊:\n最大负重 - 50")) {
            BagData.Instance.MinusCapacity(50f);
        }
    }
}
