using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 整个游戏的管理器
/// </summary>
public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(gameObject);
        SaveManager.Instance.InitData();
	}
	
	// Update is called once per frame
	void OnDestroy () {
        SaveManager.Instance.SaveData();
    }
}
