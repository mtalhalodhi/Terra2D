using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terra2D
{
    /// <summary>
    /// The base map class for ValueMap and MapData
    /// </summary>
    /// <typeparam name="T">Type of values stored in the map</typeparam>
    [Serializable]
    public abstract class BaseMap<T> : ScriptableObject where T : IComparable, IEquatable<T>
    {
        #region Variables

        protected T[,] values;

        #endregion

        #region Indexer

        public T this[int x, int y]
        {
            get
            {
                return values[x, y];
            }
            set
            {
                values[x, y] = value;
            }
        }

        public T this[Vector2Int cell]
        {
            get
            {
                return this[cell.x, cell.y];
            }
            set
            {
                this[cell.x, cell.y] = value;
            }
        }

        #endregion

        #region Properties

        public int Width
        {
            get { return values.GetLength(0); }
        }

        public int Height
        {
            get { return values.GetLength(1); }
        }

        #endregion

        #region Transform

        /// <summary>
        /// Scale the map
        /// </summary>
        /// <param name="scale">Scale factor for width and height</param>
        /// <param name="resize">If resize is set to 'true', the maps width and height are changed as well</param>
        public void Scale(Vector2Int scale, bool resize)
        {
            T[,] newValues = new T[resize ? Width * scale.x : Width, resize ? Height * scale.y : Height];

            for (int x = 0; x < newValues.GetLength(0); x += scale.x)
            {
                for (int y = 0; y < newValues.GetLength(1); y += scale.y)
                {
                    for (int i = 0; i < scale.x; i++)
                    {
                        for (int j = 0; j < scale.y; j++)
                        {
                            if (IsInsideBounds(newValues, x + i, y + j)) newValues[x + i, y + j] = values[x / scale.x, y / scale.y];
                        }
                    }
                }
            }

            values = newValues;
        }

        /// <summary>
        /// Translate(move) the map
        /// </summary>
        /// <param name="offset">Offset amount</param>
        /// <param name="resize">If resize is set to 'true', the maps width and height are changed as well</param>
        public void Translate(Vector2Int offset, bool resize = true)
        {
            var size = resize ? new Vector2Int(Width + offset.x, Height + offset.y) : new Vector2Int(Width, Height);

            if (size.x < 0) size.x = 0;
            if (size.y < 0) size.y = 0;

            T[,] newValues = new T[size.x, size.y];

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    int ox = x + offset.x;
                    int oy = y + offset.y;

                    if (IsInsideBounds(newValues, ox, oy) && IsInsideBounds(x, y))
                    {
                        newValues[ox, oy] = values[x, y];
                    }
                }
            }

            values = newValues;
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Returns the amount of occurences of a value in a block
        /// </summary>
        /// <param name="start">The start point of the block</param>
        /// <param name="end">The end point of the block</param>
        /// <param name="value">The value to check for</param>
        public int CountItemsInRegion(Vector2Int start, Vector2Int end, T value)
        {
            int count = 0;
            for (int x = start.x; x <= end.x; x++)
            {
                for (int y = start.y; y <= end.y; y++)
                {
                    if (IsInsideBounds(x, y))
                    {
                        if (this[x, y].Equals(value))
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Draws a line between two points
        /// </summary>
        /// <param name="start">Start point of the line</param>
        /// <param name="end">End point of the line</param>
        /// <param name="value">The value to draw the line with</param>
        public void ConnectCells(Vector2 start, Vector2 end, T value)
        {
            Vector2 d = end - start;

            float step = Math.Abs(d.x) >= Math.Abs(d.y) ? Math.Abs(d.x) : Math.Abs(d.y);

            d /= step;

            float x = start.x;
            float y = start.y;

            for (int i = 0; i <= step; i++)
            {
                if (IsInsideBounds((int)x, (int)y)) this[(int)x, (int)y] = value;
                x += d.x;
                y += d.y;
            }
        }

        /// <summary>
        /// Returns all surrounding cells
        /// </summary>
        /// <param name="cell">The centre cell</param>
        /// <param name="spacing">How far to look when searching</param>
        /// <param name="includeDiagonals">Include cells diagonal to this one</param>
        public List<Vector2Int> GetNeighbours(Vector2Int cell, int spacing = 1, bool includeDiagonals = false)
        {
            List<Vector2Int> neighbours = new List<Vector2Int>();

            if (includeDiagonals)
            {
                for (int x = cell.x - spacing; x <= cell.x + spacing; x++)
                {
                    for (int y = cell.y - spacing; y <= cell.y + spacing; y++)
                    {
                        if (x != cell.x || y != cell.y)
                        {
                            neighbours.Add(new Vector2Int(x, y));
                        }
                    }
                }
            }
            else
            {
                for (int x = cell.x - spacing; x <= cell.x + spacing; x++)
                {
                    if (x != cell.x)
                    {
                        neighbours.Add(new Vector2Int(x, cell.y));
                    }
                }
                for (int y = cell.y - spacing; y <= cell.y + spacing; y++)
                {
                    if (y != cell.y)
                    {
                        neighbours.Add(new Vector2Int(cell.x, y));
                    }
                }
            }

            return neighbours;
        }

        /// <summary>
        /// Returns cells that are a certain distance away from this cell
        /// </summary>
        /// <param name="cell">The centre cell</param>
        /// <param name="spacing">How far to look when searching</param>
        /// <param name="includeDiagonals">Include cells diagonal to this one</param>
        public List<Vector2Int> GetCellsAtEdge(Vector2Int cell, int spacing = 1, bool includeDiagonals = false)
        {
            List<Vector2Int> neighbours = new List<Vector2Int>();

            if (IsInsideBounds(cell.x - spacing, cell.y))
            {
                neighbours.Add(new Vector2Int(cell.x - spacing, cell.y));
            }
            if (IsInsideBounds(cell.x + spacing, cell.y))
            {
                neighbours.Add(new Vector2Int(cell.x + spacing, cell.y));
            }
            if (IsInsideBounds(cell.x, cell.y - spacing))
            {
                neighbours.Add(new Vector2Int(cell.x, cell.y - spacing));
            }
            if (IsInsideBounds(cell.x, cell.y + spacing))
            {
                neighbours.Add(new Vector2Int(cell.x, cell.y + spacing));
            }
            if (includeDiagonals)
            {
                if (IsInsideBounds(cell.x - spacing, cell.y - spacing))
                {
                    neighbours.Add(new Vector2Int(cell.x - spacing, cell.y - spacing));
                }
                if (IsInsideBounds(cell.x + spacing, cell.y + spacing))
                {
                    neighbours.Add(new Vector2Int(cell.x + spacing, cell.y + spacing));
                }
                if (IsInsideBounds(cell.x - spacing, cell.y + spacing))
                {
                    neighbours.Add(new Vector2Int(cell.x - spacing, cell.y + spacing));
                }
                if (IsInsideBounds(cell.x + spacing, cell.y - spacing))
                {
                    neighbours.Add(new Vector2Int(cell.x + spacing, cell.y - spacing));
                }
            }

            return neighbours;
        }

        /// <summary>
        /// Check if a point is inside the bounds of the map
        /// </summary>
        public bool IsInsideBounds(int x, int y)
        {
            return (x < Width && x >= 0) && (y < Height && y >= 0);
        }

        /// <summary>
        /// Check if a point is inside the bounds of the map using a vector
        /// </summary>
        public bool IsInsideBounds(Vector2Int cell)
        {
            return IsInsideBounds(cell.x, cell.y);
        }

        /// <summary>
        /// Check if a point is inside the bounds of an array
        /// </summary>
        public static bool IsInsideBounds(Array array, int x, int y)
        {
            return (x < array.GetLength(0) && x >= 0) && (y < array.GetLength(1) && y >= 0);
        }

        /// <summary>
        /// Copy values from another map
        /// </summary>
        public void CopyValues(BaseMap<T> map)
        {
            values = map.values;
        }

        #endregion
    }
}
