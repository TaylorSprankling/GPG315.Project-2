using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, DialogueGraph.AssetExtension)]
public class DialogueGraphImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        DialogueGraph editorGraph = GraphDatabase.LoadGraph<DialogueGraph>(ctx.assetPath);
        RuntimeDialogueGraph runtimeDialogueGraph = ScriptableObject.CreateInstance<RuntimeDialogueGraph>();
        Dictionary<INode, string> nodeIDMap = new();
        
        foreach (INode node in editorGraph.GetNodes())
        {
            nodeIDMap[node] = Guid.NewGuid().ToString();
        }
        
        StartNode startNode = editorGraph.GetNodes().OfType<StartNode>().FirstOrDefault();
        IPort entryPort = startNode?.GetOutputPorts().FirstOrDefault()?.firstConnectedPort;
        runtimeDialogueGraph.EntryNodeID = entryPort != null ? nodeIDMap[entryPort.GetNode()] : null;
        
        foreach (INode node in editorGraph.GetNodes())
        {
            if (node is StartNode or EndNode) continue;
            
            RuntimeDialogueNode runtimeDialogueNode = new() { NodeID = nodeIDMap[node] };
            
            switch (node)
            {
                case DialogueNode dialogueNode:
                    ProcessDialogueNode(dialogueNode, runtimeDialogueNode, nodeIDMap);
                    runtimeDialogueGraph.AllNodes.Add(runtimeDialogueNode);
                    break;
                
                case BranchingNode branchingNode:
                    ProcessBranchNode(branchingNode, runtimeDialogueNode, nodeIDMap);
                    runtimeDialogueGraph.AllNodes.Add(runtimeDialogueNode);
                    break;
            }
        }
        
        ctx.AddObjectToAsset("RuntimeData", runtimeDialogueGraph);
        ctx.SetMainObject(runtimeDialogueGraph);
    }
    
    private void ProcessDialogueNode(DialogueNode node, RuntimeDialogueNode runtimeNode, Dictionary<INode, string> nodeIDMap)
    {
        runtimeNode.SpeakerPortrait = GetPortValue<Sprite>(node.GetInputPortByName("Speaker Portrait"));
        runtimeNode.SpeakerName = GetPortValue<string>(node.GetInputPortByName("Speaker Name"));
        runtimeNode.DialogueText = GetPortValue<string>(node.GetInputPortByName("Dialogue Text"));
        
        node.GetNodeOptionByName(DialogueNode.LanguagesOptionID).TryGetValue(out int languagesValue);
        for (int i = 1; i < languagesValue; i++)
        {
            runtimeNode.LocalizedName.Add(GetPortValue<string>(node.GetInputPortByName($"L{i}: Speaker Name")));
            runtimeNode.LocalizedText.Add(GetPortValue<string>(node.GetInputPortByName($"L{i}: Dialogue Text")));
        }
        
        IPort nextNodePort = node.GetOutputPortByName("Output").firstConnectedPort;
        
        if (nextNodePort != null)
        {
            runtimeNode.NextNodeID = nodeIDMap[nextNodePort.GetNode()];
        }
    }
    
    private void ProcessBranchNode(BranchingNode node, RuntimeDialogueNode runtimeNode, Dictionary<INode, string> nodeIDMap)
    {
        runtimeNode.SpeakerPortrait = GetPortValue<Sprite>(node.GetInputPortByName("Speaker Portrait"));
        runtimeNode.SpeakerName = GetPortValue<string>(node.GetInputPortByName("Speaker Name"));
        runtimeNode.DialogueText = GetPortValue<string>(node.GetInputPortByName("Dialogue Text"));
        
        node.GetNodeOptionByName(BranchingNode.LanguagesOptionID).TryGetValue(out int languagesValue);
        for (int i = 1; i < languagesValue; i++)
        {
            runtimeNode.LocalizedName.Add(GetPortValue<string>(node.GetInputPortByName($"L{i}: Speaker Name")));
            runtimeNode.LocalizedText.Add(GetPortValue<string>(node.GetInputPortByName($"L{i}: Dialogue Text")));
        }
        
        node.GetNodeOptionByName(BranchingNode.BranchCountOptionID).TryGetValue(out int branchesValue);
        for (int i = 1; i <= branchesValue; i++)
        {
            IPort outputPort = node.GetOutputPortByName($"Branch Output {i}");
            BranchData branchData = new()
            {
                BranchText = GetPortValue<string>(node.GetInputPortByName($"Branch Text {i}")),
                NextNodeID = outputPort.firstConnectedPort != null ? nodeIDMap[outputPort.firstConnectedPort.GetNode()] : null
            };
            for (int j = 1; j < languagesValue; j++)
            {
                branchData.LocalizedText.Add(GetPortValue<string>(node.GetInputPortByName($"L{j}: Branch Text {i}")));
            }
            runtimeNode.Branches.Add(branchData);
        }
    }
    
    private T GetPortValue<T>(IPort port)
    {
        if (port == null) return default;
        
        if (port.isConnected)
        {
            if (port.firstConnectedPort.GetNode() is IVariableNode variableNode)
            {
                variableNode.variable.TryGetDefaultValue(out T value);
                return value;
            }
        }
        
        port.TryGetValue(out T fallbackValue);
        return fallbackValue;
    }
}