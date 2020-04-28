using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terra2D
{

    public class MapToTexture : MonoBehaviour
    {

        public Terra2DGraph graph;

        public string mapDataId = "Output";
        public string valueMapId = "Output";
        
        public Renderer targetRenderer;
        public IntColorPair[] colorAtlas;

        public void DisplayMap()
        {
            targetRenderer.sharedMaterial.mainTexture = MapDataToTexture2D(graph.GetMapData(mapDataId), colorAtlas);
        }

        public void DisplayValueMap()
        {
            targetRenderer.sharedMaterial.mainTexture = ValueMapToTexture2D(graph.GetValueMap(valueMapId));
        }

        public static Texture2D MapDataToTexture2D(MapData map, IntColorPair[] atlas)
        {
            Texture2D texture = new Texture2D(map.Width, map.Height);
            Color[] colors = new Color[map.Width * map.Height];

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    Color color = Color.black;

                    foreach (var ic in atlas)
                    {
                        if (ic.index == map[x, y])
                        {
                            color = ic.color;
                            break;
                        }
                    }

                    colors[x + y * map.Width] = color;
                }
            }

            texture.SetPixels(colors);
            texture.Apply();

            return texture;
        }

        public static Texture2D ValueMapToTexture2D(ValueMap map)
        {
            map.MapToRange(0, 1);

            Texture2D texture = new Texture2D(map.Width, map.Height);
            Color[] colors = new Color[map.Width * map.Height];

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    colors[x + y * map.Width] = new Color(map[x, y], map[x, y], map[x, y]);
                }
            }

            texture.SetPixels(colors);
            texture.Apply();

            return texture;
        }
    }
}