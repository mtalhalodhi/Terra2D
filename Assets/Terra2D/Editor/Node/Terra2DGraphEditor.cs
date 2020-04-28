using System;
using UnityEngine;
using UnityEditor;
using XNodeEditor;

namespace Terra2D
{

    [CustomNodeGraphEditor(typeof(Terra2DGraph), "Terra2DGraph.Settings")]
    public class Terra2DGraphEditor : NodeGraphEditor
    {
        public override string GetNodeMenuName(Type type)
        {
            if (type == typeof(MapDataOutputNode)) return "Output/Map Data";
            if (type == typeof(ValueMapOutputNode)) return "Output/Value Map";

            if (type == typeof(GradientNode)) return "Input/Gradient";
            if (type == typeof(GraphNode)) return "Input/Graph";
            if (type == typeof(ImageNode)) return "Input/Image";
            if (type == typeof(MapDataInputNode)) return "Input/Map Data";
            if (type == typeof(ValueMapInputNode)) return "Input/Value Map";

            if (type == typeof(ConverterNode)) return "Convert/ValueMap to MapData";
            
            if (type == typeof(TransformValueMapNode)) return "Transform/Value Map";
            if (type == typeof(TransformMapDataNode)) return "Transform/Map Data";

            if (type == typeof(WhiteNoiseNode)) return "Generators/White Noise";
            if (type == typeof(CellNoiseNode)) return "Generators/Cell Noise";
            if (type == typeof(FractalNoiseNode)) return "Generators/Fractal Noise";
            if (type == typeof(MazeNode)) return "Generators/Maze";
            if (type == typeof(CaveGeneratorNode)) return "Generators/Caves";
            if (type == typeof(SpaceColonizerNode)) return "Generators/Space Colonizer";

            if (type == typeof(ClampNode)) return "Value Map/Clamp";
            if (type == typeof(CurveNode)) return "Value Map/Curve";
            if (type == typeof(DetectEdgeNode)) return "Value Map/Detect Edges";
            if (type == typeof(DitherNode)) return "Value Map/Dither";
            if (type == typeof(InvertNode)) return "Value Map/Invert";
            if (type == typeof(MapToRangeNode)) return "Value Map/Map to Range";
            if (type == typeof(MathNode)) return "Value Map/Math";
            if (type == typeof(MaskNode)) return "Value Map/Mask";
            if (type == typeof(SelectNode)) return "Value Map/Select";

            if (type == typeof(OverlayNode)) return "Map Data/Overlay";

            return base.GetNodeMenuName(type);
        }

        public override Color GetTypeColor(Type type)
        {
            if (type == typeof(ValueMap)) return new Color(0, 122.0f / 255.0f, 70.0f / 255);
            if (type == typeof(MapData)) return new Color(91.0f / 255.0f, 200.0f / 255.0f, 60.0f / 255.0f);

            return base.GetTypeColor(type);
        }

        public override NodeEditorPreferences.Settings GetDefaultPreferences()
        {
            return new NodeEditorPreferences.Settings()
            {
                autoSave = true,
                gridSnap = true,
                highlightColor = Color.white,
                noodleType = NodeEditorPreferences.NoodleType.Curve
            };
        }

        public override void OnGUI()
        {
            base.OnGUI();
        }
    }
}
