using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AutoSave : EditorWindow {
    [MenuItem("MyUtility/AutoSave")]
    public static void ShowWindow() {
        AutoSave autosave = EditorWindow.GetWindow<AutoSave>();
        autosave.Show();
    }
}
