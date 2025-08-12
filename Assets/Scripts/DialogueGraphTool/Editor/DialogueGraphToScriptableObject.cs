using System.IO;
using UnityEditor;
using UnityEngine;

public class DialogueGraphToScriptableObject : Editor
{
    [MenuItem("Assets/Dialogue Graph/Extract to Scriptable Object", false)]
    public static void ExtractToScriptableObject()
    {
        int extractionAttempts = 0;
        
        foreach (Object obj in Selection.objects)
        {
            if (obj is not DialogueData graph) continue;
            
            extractionAttempts++;
            DialogueData graphData = CreateInstance<DialogueData>();
            graphData.EntryNodeID = graph.EntryNodeID;
            graphData.AllNodes = graph.AllNodes;
            string folderPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(obj));
            string fileName = obj.name + "Data.asset";
            
            if (folderPath == null)
            {
                Debug.LogWarning("Folder path is null.");
                continue;
            }
            
            if (File.Exists(Path.Combine(folderPath, fileName)))
            {
                int iteration = 1;
                while (File.Exists(Path.Combine(folderPath, fileName)))
                {
                    fileName = obj.name + $"Data{iteration}.asset";
                    iteration++;
                }
            }
            
            AssetDatabase.CreateAsset(graphData, Path.Combine(folderPath, fileName));
        }
        
        if (extractionAttempts <= 0) Debug.LogWarning("No dialogue graph files were selected.");
    }
}