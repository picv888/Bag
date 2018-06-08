using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateAssetsBundle{
    [MenuItem("MyUtility/CreateAssets")]
    public static void CreateAB() {
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/AssetsBundle/", BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.StandaloneWindows64);
        Debug.Log("打包AssetsBundle完成");
    }
}
