using System.Collections.Generic;
using UnityEngine;

namespace DialogueGraphPlugin
{
    [CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue/Scriptable Object Data")]
    public class DialogueData : ScriptableObject
    {
        public string EntryNodeID;
        public List<RuntimeDialogueNode> AllNodes = new();
    }
}