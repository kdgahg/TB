using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveLoadMap))]
public class SaveLoadMapEditor : Editor
{
    public override void OnInspectorGUI()  // 메서드 이름 수정
    {
        DrawDefaultInspector();  // 잘못된 메서드명 수정

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