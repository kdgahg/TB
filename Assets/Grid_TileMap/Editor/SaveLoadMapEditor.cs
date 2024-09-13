using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveLoadMap))]
public class SaveLoadMapEditor : Editor
{
    public override void OnInspectorGUI()  // �޼��� �̸� ����
    {
        DrawDefaultInspector();  // �߸��� �޼���� ����

        SaveLoadMap saveLoadMap = (SaveLoadMap)target;
        if (GUILayout.Button("Load"))
        {
            Debug.Log("Loading tile map");
            saveLoadMap.Load();
        }
        if (GUILayout.Button("Save"))
        {
            Debug.Log("Saving tile map");
            saveLoadMap.Save();
        }
    }
}