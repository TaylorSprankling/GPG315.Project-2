using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

[Serializable]
public class DialogueNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        base.OnDefinePorts(context);
        context.AddInputPort("Input").WithConnectorUI(PortConnectorUI.Arrowhead).Build();
        context.AddOutputPort("Output").WithConnectorUI(PortConnectorUI.Arrowhead).Build();
        
        context.AddInputPort<Sprite>("Speaker Portrait").Build();
        context.AddInputPort<string>("Speaker Name").Build();
        context.AddInputPort<string>("Dialogue Text").Build();
    }
}