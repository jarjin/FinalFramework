using FirClient.Component;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(CPrefabVar))]
public class PrefabEditor : BaseEditor
{
    CPrefabVar mPrefabVar;
    ReorderableList mReordList;

    void OnEnable()
    {
        mPrefabVar = target as CPrefabVar;
        mReordList = this.CreateItemList();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        mReordList.DoLayoutList();
        if (GUILayout.Button("Auto bind"))
        {
            Undo.RecordObject(mPrefabVar, "Auto Bind");
            mPrefabVar.AutoBind();
        }
        if (GUILayout.Button("Clear bind"))
        {
            Undo.RecordObject(mPrefabVar, "Clear Bind");
            mPrefabVar.varData.Clear();
        }
        serializedObject.ApplyModifiedProperties();
    }

    private ReorderableList CreateItemList()
    {
        void OnAddItem(ReorderableList list)
        {
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("name").stringValue = string.Empty;
            element.FindPropertyRelative("type").enumValueIndex = 0;
            element.FindPropertyRelative("lastType").enumValueIndex = 0;
            element.FindPropertyRelative("objValue").objectReferenceValue = null;
        }

        void OnRemoveItem(ReorderableList list)
        {
            if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete the var?", "Yes", "No"))
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(list);

                if (mReordList.index == mPrefabVar.varData.Count - 1)
                {
                    serializedObject.FindProperty("m_selectedIndex").intValue = mReordList.index = mReordList.index - 1;
                }
            }
        }

        void OnSelectItem(ReorderableList list)
        {
            serializedObject.FindProperty("m_selectedIndex").intValue = list.index;
            serializedObject.ApplyModifiedProperties();
            GUI.changed = true;
        }

        void OnReorderItem(ReorderableList list)
        {
            Repaint();
        }

        var reordList = CreateRecordList(serializedObject, "varData", "Prefab Var List", OnReorderItem, OnSelectItem, OnAddItem, OnRemoveItem);
        reordList.index = serializedObject.FindProperty("m_selectedIndex").intValue;
        reordList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            if (reordList.elementHeight == 0) return;
            var e = reordList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 160, EditorGUIUtility.singleLineHeight),
                e.FindPropertyRelative("name"), GUIContent.none);

            EditorGUI.PropertyField(new Rect(rect.x + 160, rect.y, 160, EditorGUIUtility.singleLineHeight), 
                e.FindPropertyRelative("type"), GUIContent.none);

            var varTypes = mPrefabVar.varTypes;
            var typeid = e.FindPropertyRelative("type").enumValueIndex;
            var lastTypeid = e.FindPropertyRelative("lastType").enumValueIndex;
            if (typeid != lastTypeid)
            {
                e.FindPropertyRelative("lastType").enumValueIndex = typeid;
                foreach (var str in varTypes)
                {
                    e.FindPropertyRelative(str).objectReferenceValue = null;
                }
            }
            var varName = mPrefabVar.GetVarNameByType((VarType)typeid);
            EditorGUI.PropertyField(new Rect(rect.x + 320, rect.y, rect.width - 320, EditorGUIUtility.singleLineHeight),
                e.FindPropertyRelative(varName), GUIContent.none);
        };
        return reordList;
    }
}
