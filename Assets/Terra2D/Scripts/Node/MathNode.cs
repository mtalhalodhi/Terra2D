using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class MathNode : Node
    {

        [Output] public ValueMap output;

        [Input] public ValueMap a;
        [Input] public ValueMap b;

        public MathOperation method;

        public bool normalize;

        protected override void Init()
        {
            base.Init();
            name = "Math";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    ValueMap a = GetInputValue("a", this.a);
                    ValueMap b = GetInputValue("b", this.b);

                    if (method == MathOperation.Add)
                    {
                        var output = a.Add(b);
                        if (normalize) output = output.MapToRange(0, 1);
                        return output;
                    }
                    else if (method == MathOperation.Multiply)
                    {
                        var output = a.Multiply(b);
                        if (normalize) output = output.MapToRange(0, 1);
                        return output;
                    }
                    else if (method == MathOperation.Subtract)
                    {
                        var output = a.Subtract(b);
                        if (normalize) output = output.MapToRange(0, 1);
                        return output;
                    }
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