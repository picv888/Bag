using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CopyPath : MonoBehaviour {

    public class ClipBoard {
        /// <summary>
        /// 将信息复制到剪切板当中
        /// </summary>
        public static void Copy(string format, params object[] args) {
            string result = string.Format(format, args);
            TextEditor editor = new TextEditor();
            editor.text = result;
            editor.OnFocus();
            editor.Copy();
        }
    }

    //获取Asset中文件路径 或者 Scene中gameobject的路径
    //快捷键alt+c
    [MenuItem("MyUtility/CopyPath &c")]
    private static void CopyGameObjectPath() {
        UnityEngine.Object obj = Selection.activeObject;
        if (obj == null) {
            Debug.LogError("You must select Obj first!");
            return;
        }
        //获取
        string result = AssetDatabase.GetAssetPath(obj);
        if (string.IsNullOrEmpty(result))//如果不是资源则在场景中查找
        {
            Transform selectChild = Selection.activeTransform;
            if (selectChild != null) {
                result = selectChild.name;
                while (selectChild.parent != null) {
                    selectChild = selectChild.parent;
                    result = string.Format("{0}/{1}", selectChild.name, result);
                }
            }
        }
        ClipBoard.Copy(result);
        Debug.Log(result);
    }
}
