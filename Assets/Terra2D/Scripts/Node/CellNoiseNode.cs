using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Terra2D
{
    public class CellNoiseNode : Node
    {

        public struct State
        {
            public FastNoise.CellularDistanceFunction distanceFunction;
            public FastNoise.CellularReturnType returnType;
            public float jitter;
            public float frequency;
            public Vector2Int offset;
            public bool normalize;

            public string seed;
            public Vector2 size;

            public static bool operator ==(State a, State b)
            {
                return a.seed == b.seed && a.size == b.size && a.distanceFunction == b.distanceFunction && a.returnType == b.returnType && a.jitter == b.jitter && a.frequency == b.frequency && a.normalize == b.normalize && a.offset == b.offset;
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

        public FastNoise.CellularDistanceFunction distanceFunction;
        public FastNoise.CellularReturnType returnType;
        public float jitter = 0.45f;
        public float frequency = 0.02f;

        public Vector2Int offset;

        public bool normalize;

        protected override void Init()
        {
            base.Init();
            name = "Cell Noise";
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
                        noise = NoiseGenerator.CellNoise(graph.seed.GetHashCode(), graph.size, offset, distanceFunction, returnType, jitter, frequency);
                        if (normalize) noise = noise.MapToRange(0, 1);

                        previousState = currentState;
                    }
                    return noise;
                }
                catch (Exception e)
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
                distanceFunction = distanceFunction,
                frequency = frequency,
                jitter = jitter,
                normalize = normalize,
                returnType = returnType,
                offset = offset,
                seed = graph.seed,
                size = graph.size
            };
        }
    }
}