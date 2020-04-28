using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class DetectEdgeNode : Node
    {

        [Output] public ValueMap output;

        [Input] public ValueMap input;

        public float threshold = .035f;

        protected override void Init()
        {
            base.Init();
            name = "Edge Detect";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    ValueMap input = GetInputValue("input", this.input);
                    output = input.DetectEdges(threshold);
                    return output;
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