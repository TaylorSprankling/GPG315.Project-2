using System.IO;
using UnityEditor;
using UnityEngine;

namespace DialogueGraphPlugin
{
    public class DialogueDataFromJson : Editor
    {
        [MenuItem("Assets/Dialogue Graph/Import from Json", false)]
        public static void ImportFromJson()
        {
            int importAttempts = 0;
            
            foreach (Object obj in Selection.objects)
            {
                string objPath = AssetDatabase.GetAssetPath(obj);
                
                if (!objPath.EndsWith(".json")) continue;
                
                importAttempts++;
                string json = File.ReadAllText(objPath);
                DialogueData data = CreateInstance<DialogueData>();
                JsonUtility.FromJsonOverwrite(json, data);
                string folderPath = Path.GetDirectoryName(objPath);
                string fileName = obj.name + ".asset";
                
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
                        fileName = obj.name + $"{iteration}.asset";
                        iteration++;
                    }
                }
                
                AssetDatabase.CreateAsset(data, Path.Combine(folderPath, fileName));
            }
            
            if (importAttempts <= 0) Debug.LogWarning("No dialogue data JSON files were selected.");
        }
    }
}