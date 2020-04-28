using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class TransformValueMapNode : Node
    {
        [Output] public ValueMap output;

        [Input] public ValueMap input;

        public Vector2Int translate;
        public Vector2Int scale = new Vector2Int(1, 1);

        public bool resize;

        protected override void Init()
        {
            base.Init();
            name = "Transform";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    var input = GetInputValue("input", this.input);

                    input.Scale(scale, resize);
                    input.Translate(translate, resize);

                    return input;
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