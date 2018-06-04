using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NoticeUI : MonoBehaviour {
    #region 单例
    private static NoticeUI instance;
    public static NoticeUI Instance {
        get {
            if (instance == null) {
                GameObject canvas = GameObject.Find("Canvas");
                instance = canvas.transform.Find("NoticeUI").GetComponent<NoticeUI>();
            }
            return instance;
        }
    }
    private NoticeUI() { }
    #endregion

    #region 成员变量
    private Text noticeText;
    private Button cancelBtn;
    private Button sureBtn;
    private Button sureBtn2;
    private RectTransform rectTransform;
    private Action onCancel;
    private Action onSure;
     
    #endregion

    private void Awake() {
        instance = this;
        rectTransform = transform as RectTransform;
        noticeText = rectTransform.Find("NoticeText").GetComponent<Text>();
        cancelBtn = rectTransform.Find("CancelBtn").GetComponent<Button>();
        sureBtn = rectTransform.Find("SureBtn").GetComponent<Button>();
        sureBtn2 = rectTransform.Find("SureBtn2").GetComponent<Button>();
        cancelBtn.onClick.AddListener(CancelBtnClick);
        sureBtn.onClick.AddListener(SureBtnClick);
        sureBtn2.onClick.AddListener(SureBtnClick);
    }

    //有取消按钮、确定按钮的提示页面
    public void ShowNotice(string noticeString, Action onCancel, Action onSure) {
        gameObject.SetActive(true);
        sureBtn.gameObject.SetActive(true);
        sureBtn2.gameObject.SetActive(false);
        cancelBtn.gameObject.SetActive(true);
        noticeText.text = noticeString;
        this.onSure = onSure;
        this.onCancel = onCancel;
        rectTransform.localPosition = Vector2.zero;
    }

    //没有取消按钮、只有确定按钮的提示页面
    public void ShowNotice(string noticeString, Action onSure) {
        gameObject.SetActive(true);
        sureBtn.gameObject.SetActive(false);
        sureBtn2.gameObject.SetActive(true);
        cancelBtn.gameObject.SetActive(false);
        noticeText.text = noticeString;
        this.onSure = onSure;
        this.onCancel = null;
        rectTransform.localPosition = Vector2.zero;
    }

    private void SureBtnClick() {
        gameObject.SetActive(false);
        Debug.Log("OnSure out");
        if (onSure != null) {
            Debug.Log("OnSure in");
            onSure();
        }
    }

    private void CancelBtnClick() {
        gameObject.SetActive(false);
        Debug.Log("OnCancel out");
        if (onCancel != null) {
            Debug.Log("OnCancel in");
            onCancel();
        }
    }
}
