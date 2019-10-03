using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;
using System.IO;

/// <summary>
/// 该类获取自己或派生类的类名，根据类名调用对应Lua文件
/// </summary>
[LuaCallCSharp]
public class LuaUIBehaviour : MonoBehaviour {
    internal static LuaEnv luaEnv = new LuaEnv(); //所有的LuaUIBehaviour公用一个LuaEnv
    internal static float lastGCTime = 0; //最近清除Lua的未手动释放的LuaBase对象的时间
    internal const float GCInterval = 1;//清理LuaBase对象的周期 

    protected Action luaAwake;
    protected Action luaStart;
    protected Action luaUpdate;
    protected Action luaOnDestroy;

    protected LuaTable scriptEnv;

    protected virtual void Awake() {
        scriptEnv = luaEnv.NewTable(); //当前对象运行Lua代码块的环境变量
        //为环境变量设置元表, __index设置成虚拟机获取的全局环境变量，相当于当前环境变量继承全局环境变量
        LuaTable meta = luaEnv.NewTable();
        meta.Set("__index", luaEnv.Global);
        scriptEnv.SetMetaTable(meta);
        meta.Dispose();

        //当前环境变量增加索引"this"指向当前对象，这样在Lua中旧可以使用this获取C#中的当前对象
        scriptEnv.Set("this", this);

        //获取该类或派生类的类名，根据类名获取同名的Lua文件，然后获取代码
        string className = this.GetType().Name;
        string luaStr = LoadStreamingAssetsFile(className);

        //如果能获取到lua文件则执行lua代码
        if (!string.IsNullOrEmpty(luaStr)) {
            luaEnv.DoString(luaStr, className, scriptEnv);

            scriptEnv.Get("Awake", out luaAwake);
            scriptEnv.Get("Start", out luaStart);
            scriptEnv.Get("Update", out luaUpdate);
            scriptEnv.Get("OnDestroy", out luaOnDestroy);

            if (luaAwake != null) {
                luaAwake();
            }
        }
    }

    protected virtual void Start() {
        if (luaStart != null) {
            luaStart();
        }
    }

    protected virtual void Update() {
        if (luaUpdate != null) {
            luaUpdate();
        }

        //清除Lua的未手动释放的LuaBase对象
        if (Time.time - LuaBehaviour.lastGCTime > GCInterval) {
            luaEnv.Tick();
            LuaBehaviour.lastGCTime = Time.time;
        }
    }

    protected virtual void OnDestroy() {
        if (luaOnDestroy != null) {
            luaOnDestroy();
        }
        luaAwake = null;
        luaOnDestroy = null;
        luaUpdate = null;
        luaStart = null;
        //释放当前lua环境变量前，先设置委托为null
        scriptEnv.Dispose();
    }

    /// <summary>
    /// 根据lua文件名(除掉后面的.lua.txt)，在streamingAssets文件夹里获取文件，然后获取字符串
    /// </summary>
    /// <param name="fileName">lua文件名</param>
    string LoadStreamingAssetsFile(string fileName) {
        //文件所在的绝对路径
        string path = Application.streamingAssetsPath + "/" + fileName + ".lua.txt";

        if (!File.Exists(path)) {
            //如果不存在该文件，直接返回null
            return null;
        }

        //把文件读取进来
        StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8);
        string lua = "";
        try {
            lua = sr.ReadToEnd();
        }
        catch (System.Exception) {

        }
        sr.Close();

        if (lua == "") {
            //如果读取内容为空串，那么返回null，表示未找到该文件
            return null;
        }
        else {
            return lua;
        }
    }
}
