using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Terra2D
{

    [CustomEditor(typeof(MapToTexture))]
    public class MapToTextureEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Display Map"))
            {
                ((MapToTexture)target).DisplayMap();
            }

            if (GUILayout.Button("Display Values"))
            {
                ((MapToTexture)target).DisplayValueMap();
            }
        }

    }
}