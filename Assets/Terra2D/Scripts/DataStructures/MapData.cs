using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terra2D
{
    /// <summary>
    /// Class used to store final map data, with integer indexes for values
    /// </summary>
    [Serializable]
    public class MapData : BaseMap<int>
    {
        #region Initializers

        /// <summary>
        /// Initialize map properties
        /// </summary>
        public void Initialize(int width, int height)
        {
            values = new int[width, height];
        }

        /// <summary>
        /// Initialize map properties
        /// </summary>
        public void Initialize(Vector2Int size)
        {
            Initialize(size.x, size.y);
        }
        
        /// <summary>
        /// Initialize map from a ValueMap
        /// </summary>
        /// <param name="index">The index value of the converted ValueMap</param>
        /// <param name="threshold">Minimum threshold to convert from</param>
        public void Initialize(ValueMap map, int index, float threshold)
        {
            Initialize(map.Width, map.Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (map[x, y] > threshold)
                    {
                        this[x, y] = index;
                    }
                    else
                    {
                        this[x, y] = 0;
                    }
                }
            }
        }

        #endregion

        #region Static Initializers

        /// <summary>
        /// Creates a MapData instance
        /// </summary>
        public static MapData CreateInstance(int width, int height)
        {
            MapData map = CreateInstance<MapData>();
            map.Initialize(width, height);
            return map;
        }

        /// <summary>
        /// Creates a MapData instance
        /// </summary>
        public static MapData CreateInstance(Vector2Int size)
        {
            MapData map = CreateInstance<MapData>();
            map.Initialize(size.x, size.y);
            return map;
        }

        /// <summary>
        /// Creates a MapData instance
        /// </summary>
        /// <param name="index">The index value of the converted ValueMap</param>
        /// <param name="threshold">Minimum threshold to convert from</param>
        public static MapData CreateInstance(ValueMap map, int index, float threshold)
        {
            MapData output = CreateInstance<MapData>();
            output.Initialize(map, index, threshold);
            return output;
        }

        #endregion

        #region Operations

        /// <summary>
        /// Overlays a Map on this one and returns a new map.
        /// All values less then or equal to zero are treated as transparent
        /// </summary>
        public MapData OverlayMap(MapData map)
        {
            MapData output = Clone();

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    if (map[x, y] > 0)
                    {
                        if (IsInsideBounds(x, y))
                        {
                            output[x, y] = map[x, y];
                        }
                    }
                }
            }

            return output;
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Create a clone of this map
        /// </summary>
        public MapData Clone()
        {
            MapData output = CreateInstance<MapData>();
            output.Initialize(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    output[x, y] = this[x, y];
                }
            }

            return output;
        }

        #endregion
    }
}
