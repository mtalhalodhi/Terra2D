using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class SpaceColonizerNode : Node
    {
        [Output] public ValueMap output;

        [Input] public ValueMap mask = null;
        
        public Vector2 start;
        public int stepSize = 3;
        public int minDistance = 10;
        public int maxDistance = 35;
        public int leafCount = 30;
        public int maxSteps = 50;
        
        protected override void Init()
        {
            base.Init();
            name = "Space Colonizer";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    mask = GetInputValue<ValueMap>("mask", null);

                    Vector2Int coord = new Vector2Int();
                    coord.x = (int)(graph.size.x * start.x);
                    coord.y = (int)(graph.size.y * start.y);

                    output = new SpaceColonizer(graph.seed, graph.size, stepSize, maxDistance, minDistance, leafCount, maxSteps, mask != null ? mask : null)
                        .Generate(coord);
                }
                catch (System.Exception e)
                {
                    graph.isComputing = false;
                    throw e;
                }
            }

            return output;
        }
    }
}
