using System.IO;
using UnityEditor;
using UnityEngine;

public class DialogueGraphToScriptableObject : Editor
{
    [MenuItem("Assets/Dialogue Graph/Extract to Scriptable Object", false)]
    public static void ExtractToScriptableObject()
    {
        foreach (Object obj in Selection.objects)
        {
            if (obj is not RuntimeDialogueGraph graph) continue;
            
            RuntimeDialogueGraph graphData = CreateInstance<RuntimeDialogueGraph>();
            
            graphData.EntryNodeID = graph.EntryNodeID;
            graphData.AllNodes = graph.AllNodes;
            
            string folderPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(obj));
            string fileName = obj.name + "Data.asset";
            
            if (folderPath == null) continue;
            
            AssetDatabase.CreateAsset(graphData, Path.Combine(folderPath, fileName));
        }
    }
}