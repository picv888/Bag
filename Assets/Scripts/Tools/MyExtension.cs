using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对官方类库的扩展方法
/// </summary>
public static class MyExtension {

    /// <summary>
    /// RectTransform的扩展方法，设置insets
    /// </summary>
    public static void SetInsets(this RectTransform rectTrf, float top, float left, float bottom, float right) {
        if (rectTrf.parent == null) {
            Debug.Log("has no parent");
            return;
        }
        Vector2 parentSize = (rectTrf.parent as RectTransform).rect.size;
        if (top + bottom > parentSize.y) {
            Debug.Log("top + bottom > width of parent!");
            return;
        }
        if (left + right > parentSize.x) {
            Debug.Log("left + right > height of parent!");
            return;
        }
        float width = parentSize.x - left - right;
        float height = parentSize.y - top - bottom;
        rectTrf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, right, width);
        rectTrf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, top, height);
    }


}
