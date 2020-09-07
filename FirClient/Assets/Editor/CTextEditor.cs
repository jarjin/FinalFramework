using UnityEngine;
using UnityEditor;
using FirClient.Component;

[CustomEditor(typeof(CText))]
public class CTextEditor : BaseEditor
{
    [MenuItem ("FixChecker/替换所有的文本框组件")]
    static void ReplaceAllText () 
    {
        var path = "Assets/GameObject.prefab";
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        var gameObj = GameObject.Instantiate<GameObject>(prefab);
        var components = gameObj.transform.GetComponentsInChildren<CText>();
        for (int i = 0; i < components.Length; i++) 
        {
            components[i].guid = System.Guid.NewGuid().ToString();
            Debug.Log(components[i].guid + " " + components[i]);
        }
        PrefabUtility.SaveAsPrefabAsset(gameObj, path);
        AssetDatabase.Refresh();
    }
}
