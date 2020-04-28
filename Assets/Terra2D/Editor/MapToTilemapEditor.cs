using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Terra2D
{
    [CustomEditor(typeof(MapToTilemap))]
    public class MapToTilemapEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate Tiles"))
            {
                ((MapToTilemap)target).GenerateTiles();
            }
        }
    }
}
