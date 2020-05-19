using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UObject = UnityEngine.Object;

public class CreateScriptEditor : BaseEditor
{
    const string tempViewPath = "/Templates/";
    const string luaCtrlCodePath = "/Scripts/Lua/UIController/";
    const string uiNamesTemplate = @"
local uiNames = {
[DATA]
};
return uiNames;
";

    class CreateScriptAssetAction : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var obj = CreateAssetFromTemplate(pathName, resourceFile);
            ProjectWindowUtil.ShowCreatedAsset(obj);
        }

        internal static UObject CreateAssetFromTemplate(string pathName, string resourceFile)
        {
            string fullName = Path.GetFullPath(pathName);
            string content = File.ReadAllText(resourceFile);
            string fileName = Path.GetFileNameWithoutExtension(pathName);
            if (resourceFile.EndsWith("LuaCtrl.txt"))
            {
                fileName = fileName.Replace("Ctrl", string.Empty);
            }
            content = content.Replace("[NAME]", fileName);
            content = content.Replace("[TIME]", System.DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss dddd"));

            var writer = new StreamWriter(fullName, false, System.Text.Encoding.UTF8);
            writer.Write(content);
            writer.Close();

            AssetDatabase.ImportAsset(pathName);
            AssetDatabase.Refresh();

            return AssetDatabase.LoadAssetAtPath(pathName, typeof(UObject));
        }
    }

    [MenuItem("Assets/Create/Game/Message Handler", false, 80)]
    static void CreateMessageHandler()
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
            ScriptableObject.CreateInstance<CreateScriptAssetAction>(),
            GetSelectedPathOrFallback() + "/Ret Handler.cs",
            null, "Assets/Templates/MessageHandler.txt");
    }

    [MenuItem("Assets/Create/Game/Lua Ctrl", false, 81)]
    static void CreateLuaCtrl()  
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
            ScriptableObject.CreateInstance<CreateScriptAssetAction>(),
            GetSelectedPathOrFallback() + "/ Ctrl.lua",
            null, "Assets/Templates/LuaCtrl.txt");
    }

    public static void CreateLuaCtrlInternal(string panelName)
    {
        var name = panelName.Replace("Panel", string.Empty);
        AddLuaUiName(name);

        var ctrlName = "UI" + name + "Ctrl.lua";
        string luaCtrlFilePath = AppDataPath + luaCtrlCodePath + ctrlName;
        if (File.Exists(luaCtrlFilePath))
        {
            return;
        }
        var tempVileFilePath = AppDataPath + tempViewPath + "LuaCtrl.txt";
        string content = File.ReadAllText(tempVileFilePath);
        content = content.Replace("[NAME]", name);
        content = content.Replace("[TIME]", System.DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss dddd"));

        var writer = new StreamWriter(luaCtrlFilePath, false, System.Text.Encoding.UTF8);
        writer.Write(content);
        writer.Close();
        AssetDatabase.Refresh();
    }

    static void AddLuaUiName(string name)
    {
        List<string> uiNames = new List<string>();
        var luaUiNames = AppDataPath + "/Editor/LuaUiNames.txt";
        if (File.Exists(luaUiNames))
        {
            var lines = File.ReadLines(luaUiNames);
            foreach (var s in lines)
            {
                if (string.IsNullOrEmpty(s))
                {
                    continue;
                }
                if (name == s)
                {
                    return;
                }
                else
                {
                    uiNames.Add(s);
                }
            }
            uiNames.Add(name);
            File.Delete(luaUiNames);
        }
        File.AppendAllLines(luaUiNames, uiNames.ToArray());

        var uiNameText = string.Empty;
        var ctrlMgrText = string.Empty;
        foreach(var l in uiNames)
        {
            uiNameText += "	" + l + " = '" + l + "',\n";
            ctrlMgrText += "	self:AddCtrl(UiNames." + l + ", UI" + l + "Ctrl:new());\n";
        }

        /////////////////////////////////////////////////////////////////////////////////////////
        var ctrlMgrTemplate = AppDataPath + tempViewPath + "CtrlMgr.txt";
        var ctrlMgrContent = File.ReadAllText(ctrlMgrTemplate);
        ctrlMgrContent = ctrlMgrContent.Replace("[DATA]", ctrlMgrText);

        var ctrlMgrFile = AppDataPath + "/Scripts/Lua/Logic/CtrlMgr.lua";
        if (File.Exists(ctrlMgrFile))
        {
            File.Delete(ctrlMgrFile);
        }
        File.WriteAllText(ctrlMgrFile, ctrlMgrContent);

        ///////////////////////////////////////////////////////////////////////////////////////////
        var uiNamesContent = uiNamesTemplate.Replace("[DATA]", uiNameText);
        var uiNamesFile = AppDataPath + "/Scripts/Lua/Common/LuaUiNames.lua";
        if (File.Exists(uiNamesFile))
        {
            File.Delete(uiNamesFile);
        }
        File.WriteAllText(uiNamesFile, uiNamesContent);
    }

    static string GetSelectedPathOrFallback()
    {
        string path = "Assets";
        foreach (var obj in Selection.GetFiltered(typeof(UObject), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
}
