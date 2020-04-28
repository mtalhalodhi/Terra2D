using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class MapDataOutputNode : Node
    {
        [Input] public MapData mapData;

        public string id;

        public string assetName;

        protected override void Init()
        {
            base.Init();
            name = "Output";
        }

        public void Save()
        {
            var graph = (Terra2DGraph)this.graph;
            try
            {
                graph.isComputing = true;
                MapData asset = GetInputValue<MapData>("mapData", mapData);
                graph.isComputing = false;

                AssetDatabase.CreateAsset(asset, "Assets/" + assetName + ".asset");
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;
            }
            catch(Exception e)
            {
                graph.isComputing = false;
                throw e;
            }
        }
    }
}