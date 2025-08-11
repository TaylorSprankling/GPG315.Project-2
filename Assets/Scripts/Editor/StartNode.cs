using System;
using Unity.GraphToolkit.Editor;

[Serializable]
public class StartNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        base.OnDefinePorts(context);
        context.AddOutputPort("Output").WithConnectorUI(PortConnectorUI.Arrowhead).Build();
    }
}