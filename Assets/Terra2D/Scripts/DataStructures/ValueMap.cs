using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terra2D
{
    /// <summary>
    /// Class used to store float values for operations
    /// </summary>
    [Serializable]
    public class ValueMap : BaseMap<float>
    {
        #region Initializers

        /// <summary>
        /// Initialize map properties
        /// </summary>
        public void Initialize(int width, int height)
        {
            values = new float[width, height];
        }

        /// <summary>
        /// Initialize map properties
        /// </summary>
        public void Initialize(Vector2Int size)
        {
            Initialize(size.x, size.y);
        }

        /// <summary>
        /// Initialize map from a gradient
        /// </summary>
        public void Initialize(int width, int height, Gradient gradient, bool vertical = false)
        {
            Initialize(width, height);

            values = new float[width, height];

            if (vertical)
            {
                for (int y = 0; y < height; y++)
                {
                    float time = y / (float)height;
                    for (int x = 0; x < width; x++)
                    {
                        values[x, y] = gradient.Evaluate(time).grayscale;
                    }
                }
            }
            else
            {
                for (int x = 0; x < width; x++)
                {
                    float time = x / (float)width;
                    for (int y = 0; y < height; y++)
                    {
                        values[x, y] = gradient.Evaluate(time).grayscale;
                    }
                }
            }
        }

        /// <summary>
        /// Initialize map from a Texture2D (converted to grayscale)
        /// </summary>
        public void Initialize(Texture2D image)
        {
            Initialize(image.width, image.height);

            var colors = image.GetPixels();

            for (int x = 0; x < image.width; x++)
            {
                for (int y = 0; y < image.height; y++)
                {
                    values[x, y] = colors[(image.width - x - 1) + (image.height - y - 1) * image.width].grayscale;
                }
            }
        }

        #endregion

        #region Static Initializers

        /// <summary>
        /// Creates a ValueMap instance
        /// </summary>
        public static ValueMap CreateInstance(int width, int height)
        {
            ValueMap map = CreateInstance<ValueMap>();
            map.Initialize(width, height);
            return map;
        }

        /// <summary>
        /// Creates a ValueMap instance
        /// </summary>
        public static ValueMap CreateInstance(Vector2Int size)
        {
            ValueMap map = CreateInstance<ValueMap>();
            map.Initialize(size.x, size.y);
            return map;
        }

        /// <summary>
        /// Creates a ValueMap instance from a gradient
        /// </summary>
        public static ValueMap CreateInstance(int width, int height, Gradient gradient, bool vertical = false)
        {
            ValueMap output = CreateInstance<ValueMap>();
            output.Initialize(width, height, gradient, vertical);
            return output;
        }

        /// <summary>
        /// Creates a ValueMap instance from a Texture2D (converted to grayscale)
        /// </summary>
        public static ValueMap CreateInstance(Texture2D image)
        {
            ValueMap output = CreateInstance<ValueMap>();
            output.Initialize(image);
            return output;
        }

        #endregion

        #region Math

        /// <summary>
        /// Adds two Value Maps
        /// </summary>
        public ValueMap Add(ValueMap map)
        {
            ValueMap output = Clone();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (map.IsInsideBounds(x,y)) output.values[x, y] = values[x, y] + map.values[x, y];
                }
            }

            return output;
        }

        /// <summary>
        /// Multiplies two Value Maps
        /// </summary>
        public ValueMap Multiply(ValueMap map)
        {
            ValueMap output = Clone();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (map.IsInsideBounds(x, y)) output.values[x, y] = values[x, y] * map.values[x, y];
                }
            }

            return output;
        }

        /// <summary>
        /// Subtracts two Value Maps
        /// </summary>
        public ValueMap Subtract(ValueMap map)
        {
            var output = Clone();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (map.IsInsideBounds(x, y)) output.values[x, y] = values[x, y] - map.values[x, y];
                }
            }

            return output;
        }

        #endregion
        
        #region Operations
        
        /// <summary>
        /// Applies am AnimationCurve on all values
        /// </summary>
        public ValueMap ApplyCurve(AnimationCurve curve)
        {
            ValueMap output = CreateInstance<ValueMap>();
            output.Initialize(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    output[x,y] = curve.Evaluate(this[x, y]);
                }
            }

            return output;
        }

        /// <summary>
        /// Clamps values between a range
        /// </summary>
        public ValueMap Clamp(float min, float max)
        {
            ValueMap output = CreateInstance<ValueMap>();
            output.Initialize(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    output[x, y] = (this[x,y] < min) ? min : ((this[x,y] > max) ? max : this[x,y]);
                }
            }

            return output;
        }

        /// <summary>
        /// Detects and returns edges in a map
        /// </summary>
        public ValueMap DetectEdges(float threshold)
        {
            ValueMap firstPass = CreateInstance<ValueMap>();
            firstPass.Initialize(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    firstPass[x, y] = 0;

                    for (int i = x - 1; i <= x + 1; i++)
                    {
                        for (int j = y - 1; j <= y + 1; j++)
                        {
                            if (IsInsideBounds(i, j))
                            {
                                if (Mathf.Abs(this[x, y] - this[i, j]) > threshold)
                                {
                                    firstPass[x, y] = 1;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            ValueMap secondPass = CreateInstance<ValueMap>();
            secondPass.Initialize(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (firstPass.CountItemsInRegion(new Vector2Int(x - 1, y - 1), new Vector2Int(x + 1, y + 1), 1) > 8)
                    {
                        secondPass[x, y] = 0;
                    }
                    else
                    {
                        secondPass[x, y] = firstPass[x,y];
                    }
                }
            }

            return secondPass;
        }

        /// <summary>
        /// Adds thickness (Dithering) to all values greater then the threshold
        /// </summary>
        public ValueMap Dither(int amount, float threshold)
        {
            var output = Clone();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (values[x, y] > threshold)
                    {
                        for (int i = x - amount; i <= x + amount; i++)
                        {
                            for (int j = y - amount; j <= y + amount; j++)
                            {
                                if (IsInsideBounds(i, j))
                                {
                                    output[i, j] = 1;
                                }
                            }
                        }
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Inverts all values
        /// </summary>
        /// <returns></returns>
        public ValueMap Invert()
        {
            float cmin = float.MaxValue;
            float cmax = float.MinValue;

            ValueMap output = CreateInstance<ValueMap>();
            output.Initialize(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (values[x, y] < cmin) cmin = values[x, y];
                    else if (values[x, y] > cmax) cmax = values[x, y];
                }
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    output[x, y] = Mathf.Lerp(cmax, cmin, Mathf.InverseLerp(cmin, cmax, values[x, y]));
                }
            }

            return output;
        }

        /// <summary>
        /// Maps all values to a range
        /// </summary>
        public ValueMap MapToRange(float min, float max)
        {
            float cmin = float.MaxValue;
            float cmax = float.MinValue;

            ValueMap output = CreateInstance<ValueMap>();
            output.Initialize(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (values[x, y] < cmin) cmin = values[x, y];
                    else if (values[x, y] > cmax) cmax = values[x, y];
                }
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    output[x, y] = Mathf.Lerp(min, max, Mathf.InverseLerp(cmin, cmax, values[x, y]));
                }
            }

            return output;
        }

        /// <summary>
        /// Uses another ValueMap to mask this one, values less then or equal to zero are treated as masked
        /// </summary>
        public ValueMap ApplyMask(ValueMap mask, float threshold)
        {
            ValueMap output = Clone();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (mask.IsInsideBounds(x, y))
                    {
                        if (mask[x,y] <= threshold)
                        {
                            output[x, y] = 0;
                        }
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Selects certain values from a map, and sets them to 1, and the rest to zero. Usefull for conversions
        /// </summary>
        public ValueMap Select(SelectOperation operation, float value)
        {
            ValueMap output = CreateInstance<ValueMap>();
            output.Initialize(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    switch (operation)
                    {
                        case SelectOperation.EqualTo:
                            output[x, y] = values[x, y] == value ? 1 : 0;
                            break;
                        case SelectOperation.GreaterThan:
                            output[x, y] = values[x, y] > value ? 1 : 0;
                            break;
                        case SelectOperation.LessThan:
                            output[x, y] = values[x, y] < value ? 1 : 0;
                            break;
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
        public ValueMap Clone()
        {
            ValueMap output = CreateInstance<ValueMap>();
            output.Initialize(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    output[x, y] = values[x, y];
                }
            }

            return output;
        }

        #endregion 
    }
}
