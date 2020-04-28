using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class FractalNoiseNode : Node
    {

        public struct State
        {
            public FractalNoiseType noiseType;
            public FastNoise.FractalType fractalType;
            public int depth;
            public float frequency;
            public Vector2Int offset;
            public bool normalize;

            public string seed;
            public Vector2 size;

            public static bool operator ==(State a, State b)
            {
                return a.seed == b.seed && a.size == b.size && a.depth == b.depth && a.fractalType == b.fractalType && a.frequency == b.frequency && a.noiseType == b.noiseType && a.normalize == b.normalize && a.offset == b.offset;
            }

            public static bool operator !=(State a, State b)
            {
                return !(a == b);
            }

            public override bool Equals(object obj)
            {
                return this == (State)obj;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        private State currentState;
        private State previousState;

        [Output] public ValueMap noise;

        public FractalNoiseType noiseType;
        public FastNoise.FractalType fractalType;
        public int depth = 1;
        public float frequency = 0.02f;

        public Vector2Int offset;

        public bool normalize;

        protected override void Init()
        {
            base.Init();
            name = "Fractal Noise";
        }

        public override object GetValue(NodePort port)
        {
            var graph = (Terra2DGraph)this.graph;
            if (graph.isComputing)
            {
                try
                {
                    currentState = GetCurrentState();
                    if (currentState != previousState)
                    {
                        noise = NoiseGenerator.FractalNoise(graph.seed.GetHashCode(), graph.size, offset, noiseType, fractalType, depth, frequency);
                        if (normalize) noise = noise.MapToRange(0, 1);

                        previousState = currentState;
                    }
                    return noise;
                }
                catch (System.Exception e)
                {
                    graph.isComputing = false;
                    throw e;
                }
            }
            return null;
        }

        public State GetCurrentState()
        {
            var graph = (Terra2DGraph)this.graph;
            return new State()
            {
                depth = depth,
                fractalType = fractalType,
                frequency = frequency,
                noiseType = noiseType,
                normalize = normalize,
                offset = offset,
                seed = graph.seed,
                size = graph.size
            };
        }
    }
}