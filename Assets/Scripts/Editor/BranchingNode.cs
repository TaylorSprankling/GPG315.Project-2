using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

[Serializable]
public class BranchingNode : Node
{
    private const string OptionID = "Branch Count";
    
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        base.OnDefinePorts(context);
        context.AddInputPort("Input").WithConnectorUI(PortConnectorUI.Arrowhead).Build();
        context.AddInputPort<Sprite>("Speaker Portrait").Build();
        context.AddInputPort<string>("Speaker Name").Build();
        context.AddInputPort<string>("Dialogue Text").Build();
        for (int i = 0; i < 4; i++)
        {
            // can't find any way to add blank spacers to make nodes look nice :(
            context.AddOutputPort($"blank space {i}").WithDisplayName(string.Empty).Build();
        }
        INodeOption option = GetNodeOptionByName(OptionID);
        option.TryGetValue(out int portCount);
        for (int i = 1; i <= portCount; i++)
        {
            context.AddInputPort<string>($"Branch Text {i}").Build();
            context.AddOutputPort($"Branch Output {i}").WithConnectorUI(PortConnectorUI.Arrowhead).Build();
        }
    }
    
    protected override void OnDefineOptions(INodeOptionDefinition context)
    {
        base.OnDefineOptions(context);
        context.AddNodeOption(OptionID, defaultValue: 2, attributes: new Attribute[] { new DelayedAttribute() });
    }
}