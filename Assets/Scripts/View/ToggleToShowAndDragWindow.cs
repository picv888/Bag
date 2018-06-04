using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//挂载在UI物体上，提供一个开关的UI窗口的方法，可以使物体可以被拖动
public class ToggleToShowAndDragWindow : MonoBehaviour, IPointerClickHandler, IDragHandler {
    RectTransform rectTransform;

    private void Start() {
        rectTransform = transform as RectTransform;
    }

    //如果物体正在显示则隐藏物体，如果物体在隐藏，则显示到最前面
    public void ToggleToShow() {
        bool isActive = gameObject.activeSelf;
        if (isActive) {
            gameObject.SetActive(false);
        }
        else {
            gameObject.SetActive(true);
            //下一帧再显示到前面，防止物体未初始化
            StartCoroutine(BringViewToFront());
        }
    }

    //下一帧将视图显示到前面
    IEnumerator BringViewToFront() {
        yield return null;
        rectTransform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta;
        if (rectTransform.parent.childCount - 1 != rectTransform.GetSiblingIndex()) {
            rectTransform.SetAsLastSibling();
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log(gameObject.name + "OnPointerClick in ToggleToShowAndDragWindow");
        rectTransform.SetAsLastSibling();
    }
}
