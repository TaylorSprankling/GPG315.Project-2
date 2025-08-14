using System;
using System.Collections.Generic;

namespace DialogueGraphPlugin
{
    [Serializable]
    public class BranchData
    {
        public string BranchText;
        public List<string> LocalizedText = new();
        public string NextNodeID;
    }
}