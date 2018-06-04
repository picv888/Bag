using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInspectorCharactorImageUI : MonoBehaviour, IDragHandler {
    private Transform charactorCametaTransform;

    private void Awake() {
        charactorCametaTransform = GameObject.Find("CharatorCamera").transform;
    }

    public void OnDrag(PointerEventData eventData) {
        charactorCametaTransform.RotateAround(charactorCametaTransform.parent.position, charactorCametaTransform.parent.up, 1f * eventData.delta.x);
    }
}
