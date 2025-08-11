using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RuntimeDialogueNode
{
    public string NodeID;
    public Sprite SpeakerPortrait;
    public string SpeakerName;
    public string DialogueText;
    public string NextNodeID;
    public List<BranchData> BranchesData = new();
}