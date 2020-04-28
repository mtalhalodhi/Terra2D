using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terra2D
{
    public static class NoiseGenerator
    {

        public static ValueMap CellNoise(int seed, Vector2Int size, Vector2 offset, FastNoise.CellularDistanceFunction distanceFunction, FastNoise.CellularReturnType returnType, float jitter = 0.45f, float frequency = 0.02f)
        {
            var noise = new FastNoise(seed);

            noise.SetCellularDistanceFunction(distanceFunction);
            noise.SetCellularReturnType(returnType);
            noise.SetCellularJitter(jitter);
            noise.SetFrequency(frequency);

            ValueMap output = ValueMap.CreateInstance(size);

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    output[x, y] = noise.GetCellular(offset.x + x, offset.y + y);
                }
            }

            return output;
        }

        public static ValueMap FractalNoise(int seed, Vector2Int size, Vector2 offset, FractalNoiseType noiseType, FastNoise.FractalType fractalType, int depth, float frequency = 0.02f)
        {
            var noise = new FastNoise(seed);

            noise.SetFractalType(fractalType);
            noise.SetFractalOctaves(depth);
            noise.SetFrequency(frequency);

            ValueMap output = ScriptableObject.CreateInstance<ValueMap>();
            output.Initialize(size.x, size.y);

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    float value = 0;
                    if (noiseType == FractalNoiseType.Cubic)
                    {
                        value = noise.GetCubicFractal(offset.x + x, offset.y + y);
                    }
                    else if (noiseType == FractalNoiseType.Perlin)
                    {
                        value = noise.GetPerlinFractal(offset.x + x, offset.y + y);
                    }
                    else if (noiseType == FractalNoiseType.Value)
                    {
                        value = noise.GetValueFractal(offset.x + x, offset.y + y);
                    }

                    output[x, y] = value;
                }
            }

            return output;
        }

        public static ValueMap WhiteNoise(int seed, Vector2Int size, float threshold)
        {
            System.Random rand = new System.Random(seed);

            var noise = new FastNoise(seed);

            ValueMap output = ScriptableObject.CreateInstance<ValueMap>();
            output.Initialize(size.x, size.y);

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {

                    output[x, y] = rand.NextDouble() > threshold ? noise.GetWhiteNoise(x, y) : 0;
                }
            }

            return output;
        }
    }
}