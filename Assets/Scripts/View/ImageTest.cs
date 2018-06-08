using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTest : MonoBehaviour {
    private Image img;
	// Use this for initialization
	void Start () {
        img = GetComponent<Image>();
        WWW www = new WWW("File://" + Application.streamingAssetsPath + "/AssetsBundle/model");
        AssetBundle bundle = www.assetBundle;
        Sprite sp = bundle.LoadAsset<Sprite>("0005");
        img.sprite = sp;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
