using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Terra2D
{
    [CustomEditor(typeof(MapToMesh))]
    public class MapToMeshEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate Mesh"))
            {
                ((MapToMesh)target).GenerateMesh();
            }
        }
    }
}
