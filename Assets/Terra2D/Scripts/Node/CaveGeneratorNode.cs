using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class CaveGeneratorNode : Node
    {
        [Output] public ValueMap output;

        [Input] public ValueMap input;

        public float threshold;
        public int iterations = 3;
        public int searchRadius = 1;
        public int birthRule = 2;
        public int deathRule = 3;

        protected override void Init()
        {
            base.Init();
            name = "Caves";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    var input = GetInputValue("input", this.input);

                    if (input == null)
                    {
                        input = CreateInstance<ValueMap>();
                        input.Initialize(graph.size);
                    }

                    output = new CaveGenerator(input, threshold, iterations, searchRadius, birthRule, deathRule).Generate();
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