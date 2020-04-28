using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class CurveNode : Node
    {
        [Output] public ValueMap output;
        [Input] public ValueMap input;

        public AnimationCurve curve;

        protected override void Init()
        {
            base.Init();
            name = "Curve";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    var input = GetInputValue("input", this.input);
                    return input.ApplyCurve(curve);
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
