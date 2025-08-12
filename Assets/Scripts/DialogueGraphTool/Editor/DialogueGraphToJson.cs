using System.IO;
using UnityEditor;
using UnityEngine;

public class DialogueGraphToJson : Editor
{
    [MenuItem("Assets/Dialogue Graph/Extract to Json", false)]
    public static void ExtractToJson()
    {
        int extractionAttempts = 0;
        
        foreach (Object obj in Selection.objects)
        {
            if (obj is not DialogueData dialogueData) continue;
            
            extractionAttempts++;
            string json = JsonUtility.ToJson(dialogueData, true);
            string folderPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(obj));
            string nameString = "Data";
            
            if (obj.name.EndsWith("Data"))
            {
                nameString = "";
            }
            
            string fileName = obj.name + $"{nameString}.json";
            
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
                    fileName = obj.name + $"{nameString}{iteration}.json";
                    iteration++;
                }
            }
            
            File.WriteAllText(Path.Combine(folderPath, fileName), json);
            AssetDatabase.Refresh();
        }
        
        if (extractionAttempts <= 0) Debug.LogWarning("No dialogue graph files were selected.");
    }
}