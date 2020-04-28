using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class GradientNode : Node
    {

        [Output] public ValueMap output;
        public Gradient gradient;
        public bool vertical;

        protected override void Init()
        {
            base.Init();
            name = "Gradient";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    output = ValueMap.CreateInstance(graph.size.x, graph.size.y, gradient, vertical);
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