using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

/// <summary>
/// 工具类，做Json的读取和写入
/// </summary>
public static class FileTools {

    /// <summary>
    /// 读取指定路径的json文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string ReadJson(string path) {
        if (!File.Exists(path)) {
            return "";
        }
        string json = "";

        StreamReader sr = new StreamReader(path, Encoding.UTF8);

        try {
            json = sr.ReadToEnd();
        }
        catch (System.Exception e) {
            Debug.Log(e.ToString());
        }

        sr.Close();

        return json;
    }

    /// <summary>
    /// 把json写入指定的文件a
    /// </summary>
    /// <param name="path"></param>
    /// <param name="json"></param>
    public static void WriteJson(string path, string json) {
        if (!File.Exists(path)) {
            FileStream fs = File.Create(path);
            fs.Close();
        }

        StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8);
        try {
            sw.Write(json);
        }
        catch (System.Exception e) {
            Debug.Log(e.ToString());
        }
        sw.Close();
    }
}
