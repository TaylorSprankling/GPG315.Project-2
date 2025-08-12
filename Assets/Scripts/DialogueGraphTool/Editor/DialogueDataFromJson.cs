using System.IO;
using UnityEditor;
using UnityEngine;

public class DialogueDataFromJson : Editor
{
    [MenuItem("Assets/Dialogue Graph/Import from Json", false)]
    public static void ImportFromJson()
    {
        foreach (Object obj in Selection.objects)
        {
            string objPath = AssetDatabase.GetAssetPath(obj);
            if (!objPath.EndsWith(".json")) continue;
            string json = File.ReadAllText(objPath);
            RuntimeDialogueGraph data = CreateInstance<RuntimeDialogueGraph>();
            data = JsonUtility.FromJson<RuntimeDialogueGraph>(json);
            string fileName = obj.name + ".dialoguegraph";
            string folderPath = Path.Combine(objPath, "../../");
            AssetDatabase.AddObjectToAsset(data, Path.Combine(Application.streamingAssetsPath, fileName));
        }
    }
}