using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class MazeNode : Node
    {
        [Output] public ValueMap output;

        [Input] public ValueMap mask = null;
        
        public Vector2 start;
        public int wallSpacing = 2;
        public float waviness = 0.5f;
        public bool diagonals = false;

        protected override void Init()
        {
            base.Init();
            name = "Maze";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    mask = GetInputValue<ValueMap>("mask", null);

                    Vector2Int coord = new Vector2Int((int)(graph.size.x * start.x), (int)(graph.size.y * start.y));

                    output = new MazeGenerator(graph.seed, graph.size, coord, wallSpacing, diagonals, waviness, mask).Generate();
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
