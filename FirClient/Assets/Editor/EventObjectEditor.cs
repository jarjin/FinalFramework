using FirClient.Component;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(CEventObject))]
public class EventObjectEditor : BaseEditor
{
    ReorderableList mReordList;

    void OnEnable()
    {
        mReordList = this.CreateItemList();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        mReordList.DoLayoutList();

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
            element.FindPropertyRelative("type").enumValueIndex = 0;
            element.FindPropertyRelative("value").stringValue = string.Empty;
        }

        void OnRemoveItem(ReorderableList list)
        {
            if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete the wave?", "Yes", "No"))
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(list);

                var targetObj = target as CEventObject;
                if (mReordList.index == targetObj.EventIds.Count - 1)
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
            var targetObj = target as MapInfo;
            int sibilingIdx = 0;
            //foreach (Tilemap tilemap in targetObj.Tilemaps)
            //{
            //    tilemap.transform.SetSiblingIndex(sibilingIdx++);
            //}
            Repaint();
        }

        var reordList = CreateRecordList(serializedObject, "eventIds", "Event Object (EventType and Params)",
                                        OnReorderItem, OnSelectItem, OnAddItem, OnRemoveItem);
        reordList.index = serializedObject.FindProperty("m_selectedIndex").intValue;
        reordList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            if (reordList.elementHeight == 0) return;
            var element = reordList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 160, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("type"), GUIContent.none);

            EditorGUI.PropertyField(new Rect(rect.x + 160, rect.y, rect.width - 160, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("value"), GUIContent.none);
        };
        return reordList;
    }
}
