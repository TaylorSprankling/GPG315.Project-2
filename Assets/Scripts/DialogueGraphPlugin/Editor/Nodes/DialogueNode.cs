using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueGraphPlugin
{
    [Serializable]
    
    public class DialogueNode : Node
    {
        public const string LanguagesOptionID = "Languages";
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            base.OnDefinePorts(context);
            
            INodeOption option = GetNodeOptionByName(LanguagesOptionID);
            option.TryGetValue(out int portCount);
            
            context.AddInputPort("Input").WithConnectorUI(PortConnectorUI.Arrowhead).Build();
            context.AddOutputPort("Output").WithConnectorUI(PortConnectorUI.Arrowhead).Build();
            
            context.AddInputPort<Sprite>("Speaker Portrait").Build();
            context.AddInputPort<string>("Speaker Name").Build();
            context.AddInputPort<string>("Dialogue Text").Build();
            for (int i = 1; i < portCount; i++)
            {
                context.AddInputPort<string>($"L{i}: Speaker Name").Build();
                context.AddInputPort<string>($"L{i}: Dialogue Text").Build();
            }
        }
        
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            base.OnDefineOptions(context);
            context.AddNodeOption(LanguagesOptionID, defaultValue: 1, attributes: new Attribute[] { new DelayedAttribute() });
        }
    }
}