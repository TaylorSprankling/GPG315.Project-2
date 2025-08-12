using System.IO;
using UnityEditor;
using UnityEngine;

public class DialogueGraphToJson : Editor
{
    [MenuItem("Assets/Dialogue Graph/Extract to Json", false)]
    public static void ExtractToJson()
    {
        foreach (Object obj in Selection.objects)
        {
            if (obj is not RuntimeDialogueGraph graph) continue;
            
            string json = JsonUtility.ToJson(graph, true);
            string folderPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(obj));
            string fileName = obj.name + ".json";
            
            if (folderPath == null) continue;
            
            File.WriteAllText(Path.Combine(folderPath, fileName), json);
            AssetDatabase.Refresh();
        }
    }
}