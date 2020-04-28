using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terra2D
{
    /// <summary>
    /// Generate maze using the 'Growing Tree Algorithm'
    /// </summary>
    public class MazeGenerator
    {
        #region Variables

        private ValueMap maze;
        private System.Random random;

        private string seed;
        private Vector2Int size;
        private Vector2Int start;
        private int wallSpacing = 2;
        private bool includeDiagonals;
        private float waviness;
        private ValueMap mask;

        #endregion

        #region Constructor
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="seed">The seed for randomness</param>
        /// <param name="size">Width and Height of the maze</param>
        /// <param name="start">Starting point for the maze</param>
        /// <param name="wallSpacing">Spacing between passages</param>
        /// <param name="includeDiagonals">Allow diagonal passages</param>
        /// <param name="waviness">Higher values yield long passages with fewer dead ends, and vice versa</param>
        /// <param name="mask">Generate maze inside a mask, make sure the start point is inisde this mask</param>
        public MazeGenerator(string seed, Vector2Int size, Vector2Int start, int wallSpacing, bool includeDiagonals, float waviness = 0.5f, ValueMap mask = null)
        {
            this.seed = seed;
            this.size = size;
            this.start = start;
            this.wallSpacing = wallSpacing;
            this.includeDiagonals = includeDiagonals;
            this.waviness = waviness;
            this.mask = mask;
        }

        #endregion

        #region Main Methods

        public ValueMap Generate()
        {
            // initialize
            maze = ValueMap.CreateInstance(size.x, size.y);
            random = new System.Random(seed.GetHashCode());
            List<Vector2Int> activeCells = new List<Vector2Int>();

            // add the start cell to the active list
            activeCells.Add(start);
            maze[start.x, start.y] = 1;

            // while active list is not empty
            while (activeCells.Count > 0)
            {
                // select cell from active list
                // randomly selected based on waviness
                // higher waviness produces long winding passages with fewer dead ends, and vice versa
                int cellIndex = random.NextDouble() > waviness ? random.Next(activeCells.Count) : activeCells.Count - 1;
                var cell = activeCells[cellIndex];

                // get untraversed neighbours of selected cell
                var neighbours = GetNeighbours(cell.x, cell.y);
                
                if (neighbours.Count > 0)
                {
                    // randomly select a neighbour and connect it to the selected cell
                    var neighbour = neighbours[random.Next(neighbours.Count)];
                    maze.ConnectCells(cell, neighbour, 1);

                    // add that neighbour to the active list
                    activeCells.Add(neighbour);
                }
                else
                {
                    // if no traversable neighbours, remove cell from active list
                    activeCells.Remove(cell);
                }
            }

            return maze;
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Get all neighbours that havent been traversed
        /// </summary>
        private List<Vector2Int> GetNeighbours(int x, int y)
        {
            // get all neighbour cells
            List<Vector2Int> neighbours = maze.GetCellsAtEdge(new Vector2Int(x, y), wallSpacing, includeDiagonals);

            // remove all cells that have been traversed
            neighbours = neighbours.FindAll(n => maze[n] == 0);

            // apply mask (if there is one)
            if (mask != null)
            {
                List<Vector2Int> maskedNeighbours = new List<Vector2Int>();

                for (int i = neighbours.Count - 1; i >= 0; i--)
                {
                    if (mask.IsInsideBounds(neighbours[i]))
                    {
                        if (mask[neighbours[i]] > 0)
                        {
                            maskedNeighbours.Add(neighbours[i]);
                        }
                    }
                }

                return maskedNeighbours;
            }
            else
            {
                return neighbours;
            }
        }

        #endregion
    }
}
