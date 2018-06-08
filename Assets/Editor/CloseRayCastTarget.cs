using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
/// <summary>
/// 关闭UI的射线检测工具类
/// </summary>
public class CloseRayCastTarget : MonoBehaviour {

    /// <summary>
    /// 关闭该物体的所有子物体的射线检测
    /// </summary>
    [MenuItem("MyUtility/Close AllChildren RayCast (without self)")]
    private static void CloseSelectGameobjectAllChildRayCast() {
        Transform t = Selection.activeTransform;
        if(t == null) {
            Debug.Log("先选择一个游戏物体");
            return;
        }
        CloseAllChildRayCast(t);
        //Transform selectChild = Selection.activeTransform;
    }

    /// <summary>
    /// 关闭该物体及其所有子物体的射线检测
    /// </summary>
    [MenuItem("MyUtility/Close AllChildren RayCast (include self)")]
    private static void CloseSelectGameobjectAndAllChildRayCast() {
        Transform t = Selection.activeTransform;
        if(t == null) {
            Debug.Log("先选择一个游戏物体");
            return;
        }
        CloseRayCast(t);
        CloseAllChildRayCast(t);
        //Transform selectChild = Selection.activeTransform;
    }

    /// <summary>
    /// 关闭一个物体所有组件的RayCast
    /// </summary>
    private static void CloseRayCast(Transform t) {
        MaskableGraphic[] components = t.GetComponents<MaskableGraphic>();
        for(int i = 0; i < components.Length; i++) {
            MaskableGraphic c = components[i];
            c.raycastTarget = false;
        }
        //t.GetComponents
    }

    private static void CloseAllChildRayCast(Transform t) {
        //遍历所有子物体
        for(int i = 0; i < t.childCount; i++) {
            Transform childTrans = t.GetChild(i);
            CloseRayCast(childTrans);
            CloseAllChildRayCast(childTrans);
        }
    }
}
