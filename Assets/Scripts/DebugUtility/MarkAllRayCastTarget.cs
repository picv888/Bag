#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 把所有能接受射线检测的物体用蓝色边框标记出来
/// </summary>
public class MarkAllRayCastTarget : MonoBehaviour {
    static Vector3[] fourCorners = new Vector3[4];
    void OnDrawGizmos() {
        foreach (MaskableGraphic g in FindObjectsOfType<MaskableGraphic>()) {
            if (g.raycastTarget) {
                RectTransform rectTransform = g.transform as RectTransform;
                rectTransform.GetWorldCorners(fourCorners);
                Gizmos.color = Color.blue;
                for (int i = 0; i < 4; i++)
                    Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);
            }
        }
    }
}
#endif
