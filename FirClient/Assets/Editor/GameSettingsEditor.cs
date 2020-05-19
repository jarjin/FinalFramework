using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

[CustomEditor(typeof(GameSettings))]
public class GameSettingsEditor : BaseEditor
{
    private List<ReorderableList> lists = new List<ReorderableList>();

    void OnEnable()
    {
        lists.Add(CreateAtlasList());
        lists.Add(CreateABList());
        lists.Add(CreateDatasList());
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();
        foreach(var list in lists)
        {
            list.DoLayoutList();
        }
        serializedObject.ApplyModifiedProperties();
    }

    private ReorderableList CreateABList()
    {
        void OnAddItem(ReorderableList list)
        {
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("assetPath").stringValue = string.Empty;
            element.FindPropertyRelative("packType").enumValueIndex = 0;
            element.FindPropertyRelative("fileExtName").stringValue = string.Empty;
        }

        void OnRemoveItem(ReorderableList list)
        {
            if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete the item?", "Yes", "No"))
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(list);

                var targetObj = target as GameSettings;
                if (list.index == targetObj.atlasSettings.Count - 1)
                {
                    serializedObject.FindProperty("selectedABIndex").intValue = list.index = list.index - 1;
                }
            }
        }

        void OnSelectItem(ReorderableList list)
        {
            serializedObject.FindProperty("selectedABIndex").intValue = list.index;
            serializedObject.ApplyModifiedProperties();
            GUI.changed = true;
        }

        void OnReorderItem(ReorderableList list)
        {
            var targetObj = target as MapInfo;
            int sibilingIdx = 0;
            //foreach (Tilemap tilemap in targetObj.Tilemaps)
            //{
            //    tilemap.transform.SetSiblingIndex(sibilingIdx++);
            //}
            Repaint();
        }

        var reordList = CreateRecordList(serializedObject, "assetBundlePackSetting", "AssetBundle Pack Settings",
                                OnReorderItem, OnSelectItem, OnAddItem, OnRemoveItem);
        reordList.index = serializedObject.FindProperty("selectedABIndex").intValue;
        reordList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            if (reordList.elementHeight == 0) return;
            var element = reordList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 275, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("assetPath"), GUIContent.none);

            EditorGUI.PropertyField(new Rect(rect.x + rect.width - 273, rect.y, 130, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("packType"), GUIContent.none);

            EditorGUI.PropertyField(new Rect(rect.x + rect.width - 140, rect.y, 140, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("fileExtName"), GUIContent.none);
        };
        return reordList;
    }

    private ReorderableList CreateDatasList()
    {
        void OnAddItem(ReorderableList list)
        {
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("dataPath").stringValue = string.Empty;
            element.FindPropertyRelative("packType").enumValueIndex = 0;
            element.FindPropertyRelative("fileExtName").stringValue = string.Empty;
        }

        void OnRemoveItem(ReorderableList list)
        {
            if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete the item?", "Yes", "No"))
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(list);

                var targetObj = target as GameSettings;
                if (list.index == targetObj.atlasSettings.Count - 1)
                {
                    serializedObject.FindProperty("selectedDatasIndex").intValue = list.index = list.index - 1;
                }
            }
        }

        void OnSelectItem(ReorderableList list)
        {
            serializedObject.FindProperty("selectedDatasIndex").intValue = list.index;
            serializedObject.ApplyModifiedProperties();
            GUI.changed = true;
        }

        void OnReorderItem(ReorderableList list)
        {
            var targetObj = target as MapInfo;
            int sibilingIdx = 0;
            //foreach (Tilemap tilemap in targetObj.Tilemaps)
            //{
            //    tilemap.transform.SetSiblingIndex(sibilingIdx++);
            //}
            Repaint();
        }

        var reordList = CreateRecordList(serializedObject, "datasBundlePackSetting", "Datas Pack Settings",
                                OnReorderItem, OnSelectItem, OnAddItem, OnRemoveItem);
        reordList.index = serializedObject.FindProperty("selectedDatasIndex").intValue;
        reordList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            if (reordList.elementHeight == 0) return;
            var element = reordList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 275, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("dataPath"), GUIContent.none);

            EditorGUI.PropertyField(new Rect(rect.x + rect.width - 273, rect.y, 130, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("packType"), GUIContent.none);

            EditorGUI.PropertyField(new Rect(rect.x + rect.width - 140, rect.y, 140, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("fileExtName"), GUIContent.none);
        };
        return reordList;
    }

    private ReorderableList CreateAtlasList()
    {
        void OnAddItem(ReorderableList list)
        {
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("assetPath").stringValue = string.Empty;
            element.FindPropertyRelative("androidFormat").enumValueIndex = 0;
            element.FindPropertyRelative("iosFormat").enumValueIndex = 0;
            element.FindPropertyRelative("textureSize").enumValueIndex = 0;
            element.FindPropertyRelative("isDynamic").boolValue = false;
        }

        void OnRemoveItem(ReorderableList list)
        {
            if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete the item?", "Yes", "No"))
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(list);

                var targetObj = target as GameSettings;
                if (list.index == targetObj.atlasSettings.Count - 1)
                {
                    serializedObject.FindProperty("selectedAtlasIndex").intValue = list.index = list.index - 1;
                }
            }
        }

        void OnSelectItem(ReorderableList list)
        {
            serializedObject.FindProperty("selectedAtlasIndex").intValue = list.index;
            serializedObject.ApplyModifiedProperties();
            GUI.changed = true;
        }

        void OnReorderItem(ReorderableList list)
        {
            var targetObj = target as MapInfo;
            int sibilingIdx = 0;
            //foreach (Tilemap tilemap in targetObj.Tilemaps)
            //{
            //    tilemap.transform.SetSiblingIndex(sibilingIdx++);
            //}
            Repaint();
        }

        var reordList = CreateRecordList(serializedObject, "atlasSettings", "Atlas Settings",
                                OnReorderItem, OnSelectItem, OnAddItem, OnRemoveItem);
        reordList.index = serializedObject.FindProperty("selectedAtlasIndex").intValue;
        reordList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            if (reordList.elementHeight == 0) return;
            var element = reordList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 337, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("assetPath"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + rect.width - 335, rect.y, 120, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("androidFormat"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + rect.width - 215, rect.y, 120, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("iosFormat"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + rect.width - 95, rect.y, 80, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("textureSize"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + rect.width - 15, rect.y - 1, 15, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("isDynamic"), GUIContent.none);
        };
        return reordList;
    }
}
