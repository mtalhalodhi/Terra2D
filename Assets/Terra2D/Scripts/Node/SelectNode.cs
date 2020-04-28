using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class SelectNode : Node
    {
        [Output] public ValueMap output;
        [Input] public ValueMap input;

        public SelectOperation method;
        public float value;

        protected override void Init()
        {
            base.Init();
            name = "Select";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    ValueMap input = GetInputValue("input", this.input);

                    return input.Select(method, value);
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