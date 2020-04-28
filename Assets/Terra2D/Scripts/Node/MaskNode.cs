using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class MaskNode : Node
    {
        [Output] public ValueMap output;

        [Input] public ValueMap input;
        [Input] public ValueMap mask;

        float threshold = 0;

        protected override void Init()
        {
            base.Init();
            name = "Overlay";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    ValueMap input = GetInputValue("input", this.input);
                    ValueMap mask = GetInputValue("mask", this.mask);

                    return input.ApplyMask(mask, threshold);
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