using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Terra2D
{
    [CustomNodeEditor(typeof(ValueMapOutputNode))]
    public class ValueMapOutputNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            var node = (ValueMapOutputNode)target;

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
