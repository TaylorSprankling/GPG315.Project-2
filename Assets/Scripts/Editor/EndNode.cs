using System;
using Unity.GraphToolkit.Editor;

[Serializable]
public class EndNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        base.OnDefinePorts(context);
        context.AddInputPort("Input").WithConnectorUI(PortConnectorUI.Arrowhead).Build();
    }
}