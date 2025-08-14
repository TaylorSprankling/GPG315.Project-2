using System;
using Unity.GraphToolkit.Editor;

namespace DialogueGraphPlugin
{
    [Serializable]
    public class EndNode : Node
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            base.OnDefinePorts(context);
            context.AddInputPort("Input").WithConnectorUI(PortConnectorUI.Arrowhead).Build();
        }
    }
}