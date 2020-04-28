using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class DitherNode : Node
    {
        [Output] public ValueMap output;

        [Input] public ValueMap input;

        [Range(0, 256)]
        public int amount;

        public float threshold;

        protected override void Init()
        {
            base.Init();
            name = "Dither";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    ValueMap input = GetInputValue("input", this.input);
                    output = input.Dither(amount, threshold);
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