using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class GraphNode : Node
    {
        [Output] public MapData mapData;
        [Output] public ValueMap valueMap;

        public Terra2DGraph inputGraph;

        public string mapDataId;
        public string valueMapId;

        protected override void Init()
        {
            base.Init();
            name = "Graph";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    var input = GetInputValue("inputGraph", inputGraph);
                    
                    if (port.ValueType == typeof(MapData))
                    {
                        return input.GetMapData(mapDataId);
                    }
                    if (port.ValueType == typeof(ValueMap))
                    {
                        return input.GetValueMap(valueMapId);
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