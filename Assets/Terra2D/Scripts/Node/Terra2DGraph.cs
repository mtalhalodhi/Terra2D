using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    [CreateAssetMenu(fileName ="Graph", menuName ="Terra2D/Graph")]
    public class Terra2DGraph : NodeGraph
    {
        public string seed;
        public Vector2Int size;

        public bool isComputing { get; set; }

        public override Node AddNode(Type type)
        {
            return base.AddNode(type);
        }

        public MapData GetMapData(string id)
        {
            MapDataOutputNode node = (MapDataOutputNode)nodes.FindAll(n => n is MapDataOutputNode).Find(n => ((MapDataOutputNode)n).id == id);

            isComputing = true;
            MapData map = node.GetInputValue("mapData", node.mapData);
            isComputing = false;

            return map;
        }

        public ValueMap GetValueMap(string id)
        {
            ValueMapOutputNode node = (ValueMapOutputNode)nodes.FindAll(n => n is ValueMapOutputNode).Find(n => ((ValueMapOutputNode)n).id == id);

            isComputing = true;
            ValueMap map = node.GetInputValue("valueMap", node.valueMap);
            isComputing = false;

            return map;
        }
    }
}
