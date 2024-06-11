using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField]
    private ThingSystem thingSystem;
    private const string path = "./save/";

    public void Save()
    {
        try
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            SaveRecur(path, 0, thingSystem);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void SaveRecur(string curPath, int idx, ISavable target)
    {
        List<ISavable> childs = target.GetChilds().ToList();
        if (childs.Count > 0)
        {
            string dirPath = curPath + string.Format(target.SavableName, idx) + "/";
            Directory.CreateDirectory(dirPath);
            for (int i = 0; i < childs.Count; i++)
                SaveRecur(dirPath, i, childs[i]);
        }

        string json = target.GetJSON();
        if (json != null)
        {
            string filePath = curPath + string.Format(target.SavableName, idx) + ".json";
            StreamWriter sw = new(filePath);
            sw.Write(json);
            sw.Close();
        }
    }
}

[CustomEditor(typeof(SaveSystem))]
public class SaveButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SaveSystem system = (SaveSystem)target;
        if (GUILayout.Button("Save"))
            system.Save();
    }
}