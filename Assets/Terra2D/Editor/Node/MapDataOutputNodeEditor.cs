using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Terra2D
{
    [CustomNodeEditor(typeof(MapDataOutputNode))]
    public class MapDataOutputNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            var node = (MapDataOutputNode)target;

            if (!string.IsNullOrEmpty(node.id) && node.id != node.name)
            {
                node.name = node.id;
            }

            if (GUILayout.Button("Save"))
            {
                node.Save();
            }
        }
    }
}
