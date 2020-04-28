using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terra2D
{
    /// <summary>
    /// Used to generate caves using 'Cellular Automata'
    /// </summary>
    public class CaveGenerator
    {
        #region Variables

        private ValueMap input;
        private ValueMap map;
        private ValueMap buffer;

        private Vector2Int size;

        private float threshold;

        private int iterations;
        private int searchRadius;
        private int birthRule;
        private int deathRule;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new Cave Generator
        /// </summary>
        /// <param name="input">The input map, usually white noise</param>
        /// <param name="threshold">Input values below this threshold will be treated as zero</param>
        /// <param name="iterations">How many times to iterate the automata, higher values give smoother caves, but are slow</param>
        /// <param name="searchRadius">Neighbour search radius</param>
        /// <param name="birthRule"></param>
        /// <param name="deathRule"></param>
        public CaveGenerator(ValueMap input, float threshold, int iterations = 3, int searchRadius = 1, int birthRule = 2, int deathRule = 3)
        {
            this.threshold = threshold;
            this.iterations = iterations;
            this.searchRadius = searchRadius;
            this.birthRule = birthRule;
            this.deathRule = deathRule;
            this.input = input;

            size = new Vector2Int(input.Width, input.Height);

            map = ValueMap.CreateInstance(size);
            buffer = ValueMap.CreateInstance(size);
        }

        #endregion

        #region Main Methoods
        
        public ValueMap Generate()
        {
            map = input.Clone();

            for (int i = 0; i < iterations; i++)
            {
                buffer.Initialize(size);

                for (int x = 0; x < size.x; x++)
                {
                    for (int y = 0; y < size.y; y++)
                    {
                        // Count alive neighbours
                        int neighbours = map.GetNeighbours(new Vector2Int(x, y), searchRadius, true).FindAll(n => map[n] > threshold).Count;

                        // cell is alive
                        if (neighbours > birthRule)
                        {
                            buffer[x, y] = 1;
                        }
                        // cell is dead
                        else if (neighbours < deathRule)
                        {
                            buffer[x, y] = 0;
                        }
                    }
                }

                map = buffer.Clone();
            }

            return map;
        }

        #endregion
    }
}
