using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Terra2D
{
    [RequireComponent(typeof(Tilemap))]
    public class MapToTilemap : MonoBehaviour
    {
        private Tilemap tilemap;

        public Terra2DGraph graph;
        public string output;
        public List<IntTilePair> tiles;

        public void GenerateTiles()
        {
            if(tilemap == null)
            {
                tilemap = GetComponent<Tilemap>();
            }

            var map = graph.GetMapData(output);

            tilemap.ClearAllTiles();
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    TileBase tile = null;

                    for (int i = 0; i < tiles.Count; i++)
                    {
                        if (map[x, y] == tiles[i].index)
                        {
                            tile = tiles[i].tile;
                            break;
                        }
                    }
                    if (tile != null)
                    {
                        tilemap.SetTile(new Vector3Int(-map.Width / 2 + map.Width - 1 - x, -map.Height / 2 + map.Height - 1 - y, (int)transform.position.z), tile);
                    }
                }
            }
        }
    }
}
