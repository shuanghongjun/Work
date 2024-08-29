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
        //����  proceduresProperty
        proceduresProperty = serializedObject.FindProperty("procedures");
        //����  defaultProcedureProperty
        defaultProcedureProperty = serializedObject.FindProperty("defaultProcedure");
        allProcedureTypes = new List<string>() { "ʱЧ1", "Ч��2", "Ч��3" };
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
                        GUI.changed = false;  //���ڼ���ڵ�ǰ GUI ֡���Ƿ����κ� GUI �ؼ���ֵ�����˱仯
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

            //��ʾĬ��״̬
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
