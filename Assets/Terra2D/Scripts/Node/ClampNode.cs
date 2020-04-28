using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class ClampNode : Node
    {
        [Output] public ValueMap output;
        [Input] public ValueMap input;

        public float min;
        public float max;

        protected override void Init()
        {
            base.Init();
            name = "Clamp";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    var input = GetInputValue("input", this.input);
                    return input.Clamp(min, max);
                }
                catch (System.Exception e)
                {
                    graph.isComputing = false;
                    throw e;
                }
            }
            return null;
        }
    }
}
