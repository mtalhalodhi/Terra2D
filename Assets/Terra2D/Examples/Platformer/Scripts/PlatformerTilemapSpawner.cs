using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

class PlatformerTilemapSpawner : MonoBehaviour
{

    public Terra2D.MapData map;

    public Tilemap tilemap;

    public TileBase background = null;
    public TileBase platforms = null;
    public TileBase decos = null;

    public void GenerateTiles()
    {
        tilemap.ClearAllTiles();

        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                var pos = new Vector3Int(x, y, 0);

                if (map[x, y] == 1)
                {
                    tilemap.SetTile(pos, platforms);
                }
                else
                {
                    if (map.IsInsideBounds(x, y - 1) && map[x, y - 1] == 1)
                    {
                        tilemap.SetTile(pos, decos);
                    }
                    else
                    {
                        tilemap.SetTile(pos, background);
                    }
                }
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PlatformerTilemapSpawner))]
class PlatformerTilemapSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate")) ((PlatformerTilemapSpawner)target).GenerateTiles();
    }
}
#endif
