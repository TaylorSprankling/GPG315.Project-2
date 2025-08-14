using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueGraphPlugin
{
    [Serializable]
    public class RuntimeDialogueNode
    {
        public string NodeID;
        public Sprite SpeakerPortrait;
        public string SpeakerName;
        public List<string> LocalizedName = new();
        public string DialogueText;
        public List<string> LocalizedText = new();
        public string NextNodeID;
        public List<BranchData> Branches = new();
    }
}