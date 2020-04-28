using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class ImageNode : Node
    {
        [Output] public ValueMap output;

        public Texture2D image;

        protected override void Init()
        {
            base.Init();
            name = "Image";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    output = ValueMap.CreateInstance(image);
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