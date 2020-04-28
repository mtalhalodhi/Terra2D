using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class ConverterNode : Node
    {
        [Output] public MapData output;
        [Input] public ValueMap input;

        public int index;
        public float threshold;

        protected override void Init()
        {
            base.Init();
            name = "Converter";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    ValueMap input = GetInputValue("input", this.input);
                    output = CreateInstance<MapData>();
                    output.Initialize(input, index, threshold);
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