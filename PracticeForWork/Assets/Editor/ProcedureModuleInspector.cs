using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestToggle))]
public class TestToggleEditor : Editor
{
    private List<string> allProcedureTypes;

    private SerializedProperty proceduresProperty;
    private SerializedProperty defaultProcedureProperty;

    private void OnEnable()
    {
        serializedObject.ApplyModifiedProperties();
        //属性  proceduresProperty
        proceduresProperty = serializedObject.FindProperty("procedures");
        //属性  defaultProcedureProperty
        defaultProcedureProperty = serializedObject.FindProperty("defaultProcedure");
        allProcedureTypes = new List<string>() { "时效1", "效果2", "效果3" };
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginDisabledGroup(Application.isPlaying);
        {
            if (allProcedureTypes.Count > 0)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    for (int i = 0; i < allProcedureTypes.Count; i++)
                    {
                        GUI.changed = false;  //用于检测在当前 GUI 帧中是否有任何 GUI 控件的值发生了变化
                        int? index = FindProcedureTypeIndex(allProcedureTypes[i]);
                        bool selected = EditorGUILayout.ToggleLeft(allProcedureTypes[i], index.HasValue);
                        if (GUI.changed)
                        {
                            if (selected)
                            {
                                AddProcedure(allProcedureTypes[i]);
                            }
                            else
                            {
                                RemoveProcedure(index.Value);
                            }
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }
        EditorGUI.EndDisabledGroup();

        if (proceduresProperty.arraySize == 0)
        {
            if (allProcedureTypes.Count == 0)
            {
                EditorGUILayout.HelpBox("Can't find any procedure", UnityEditor.MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("Please select a procedure at least", UnityEditor.MessageType.Info);
            }
        }
        else
        {

            //显示默认状态
            List<string> selectedProcedures = new List<string>();
            for (int i = 0; i < proceduresProperty.arraySize; i++)
            {
                selectedProcedures.Add(proceduresProperty.GetArrayElementAtIndex(i).stringValue);
            }
            selectedProcedures.Sort();
            int defaultProcedureIndex = selectedProcedures.IndexOf(defaultProcedureProperty.stringValue);
            defaultProcedureIndex = EditorGUILayout.Popup("Default Procedure", defaultProcedureIndex, selectedProcedures.ToArray());
            if (defaultProcedureIndex >= 0)
            {
                defaultProcedureProperty.stringValue = selectedProcedures[defaultProcedureIndex];
            }

        }

        serializedObject.ApplyModifiedProperties();
    }

    private void AddProcedure(string procedureType)
    {
        proceduresProperty.InsertArrayElementAtIndex(0);
        proceduresProperty.GetArrayElementAtIndex(0).stringValue = procedureType;
    }

    private void RemoveProcedure(int index)
    {
        string procedureType = proceduresProperty.GetArrayElementAtIndex(index).stringValue;
        if (procedureType == defaultProcedureProperty.stringValue)
        {
            Debug.LogWarning("Can't remove default procedure");
            return;
        }
        proceduresProperty.DeleteArrayElementAtIndex(index);
    }

    private int? FindProcedureTypeIndex(string procedureType)
    {
        for (int i = 0; i < proceduresProperty.arraySize; i++)
        {
            SerializedProperty p = proceduresProperty.GetArrayElementAtIndex(i);
            if (p.stringValue == procedureType)
            {
                return i;
            }
        }
        return null;
    }
}
