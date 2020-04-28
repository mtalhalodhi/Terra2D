using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class WhiteNoiseNode : Node
    {
        [Output] public ValueMap noise;

        public float threshold;

        protected override void Init()
        {
            base.Init();
            name = "White Noise";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    noise = NoiseGenerator.WhiteNoise(graph.seed.GetHashCode(), graph.size, threshold);
                    return noise;
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