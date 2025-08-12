using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

[Serializable]
public class BranchingNode : Node
{
    public const string BranchCountOptionID = "Branch Count";
    public const string LanguagesOptionID = "Languages";
    
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        base.OnDefinePorts(context);
        
        INodeOption branchesOption = GetNodeOptionByName(BranchCountOptionID);
        branchesOption.TryGetValue(out int branchesPortCount);
        INodeOption option = GetNodeOptionByName(LanguagesOptionID);
        option.TryGetValue(out int languagesPortCount);
        
        context.AddInputPort("Input").WithConnectorUI(PortConnectorUI.Arrowhead).Build();
        context.AddInputPort<Sprite>("Speaker Portrait").Build();
        context.AddInputPort<string>("Speaker Name").Build();
        context.AddInputPort<string>("Dialogue Text").Build();
        
        for (int i = 1; i <= branchesPortCount; i++)
        {
            context.AddInputPort<string>($"Branch Text {i}").Build();
            context.AddOutputPort($"Branch Output {i}").WithConnectorUI(PortConnectorUI.Arrowhead).Build();
        }
        
        for (int i = 1; i < languagesPortCount; i++)
        {
            context.AddInputPort<string>($"L{i}: Speaker Name").Build();
            context.AddInputPort<string>($"L{i}: Dialogue Text").Build();
            for (int j = 1; j <= branchesPortCount; j++)
            {
                context.AddInputPort<string>($"L{i}: Branch Text {j}").Build();
            }
        }
    }
    
    protected override void OnDefineOptions(INodeOptionDefinition context)
    {
        base.OnDefineOptions(context);
        context.AddNodeOption(BranchCountOptionID, defaultValue: 2, attributes: new Attribute[] { new DelayedAttribute() });
        context.AddNodeOption(LanguagesOptionID, defaultValue: 1, attributes: new Attribute[] { new DelayedAttribute() });
    }
}