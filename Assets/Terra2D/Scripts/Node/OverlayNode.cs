using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class OverlayNode : Node
    {
        [Output] public MapData output;

        [Input] public MapData a;
        [Input] public MapData b;

        protected override void Init()
        {
            base.Init();
            name = "Overlay";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    MapData a = GetInputValue("a", this.a);
                    MapData b = GetInputValue("b", this.b);

                    return a.OverlayMap(b);
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