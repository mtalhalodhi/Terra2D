using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Terra2D
{
    public class SpaceColonizer
    {
        #region Variables
        System.Random rand;
        int seed;

        private Vector2Int size;

        ValueMap tree;
        ValueMap mask;

        int stepSize;
        int maxDistance;
        int minDistance;
        int leafCount;
        int maxSteps;
        #endregion

        #region Constructor
        public SpaceColonizer(string seed, Vector2Int size, int stepSize, int maxDistance, int minDistance, int leafCount, int maxSteps, ValueMap mask = null)
        {
            this.seed = seed.GetHashCode();
            this.size = size;
            this.stepSize = stepSize;
            this.maxDistance = maxDistance;
            this.minDistance = minDistance;
            this.leafCount = leafCount;
            this.maxSteps = maxSteps;
            this.mask = mask;
        }
        #endregion

        #region Main Methods
        public ValueMap Generate(Vector2Int start)
        {
            tree = ScriptableObject.CreateInstance<ValueMap>();
            tree.Initialize(size.x, size.y);
            
            var leaves = SpawnLeaves();

            List<Branch> branches = new List<Branch>();
            branches.Add(new Branch(start, null));

            int step = 0;
            while (true)
            {
                step++;
                if (step > maxSteps) break;

                List<Vector2> leavesToRemove = new List<Vector2>();

                foreach (var leaf in leaves)
                {
                    Branch closest = null;
                    float closestBranchDistance = float.MaxValue;
                    for(int i = 0; i < branches.Count; i++)
                    {
                        var direction = leaf - branches[i].position;
                        var distance = direction.magnitude;

                        if (distance < maxDistance)
                        {
                            if (distance < closestBranchDistance)
                            {
                                closest = branches[i];
                                closestBranchDistance = distance;
                            }
                        }
                        if (distance < minDistance)
                        {
                            leavesToRemove.Add(leaf);
                        }
                    }

                    if (closest != null)
                    {
                        var direction = leaf - closest.position;

                        closest.direction += direction.normalized;
                        closest.growCount++;
                    }
                }

                foreach(var leaf in leavesToRemove)
                {
                    leaves.Remove(leaf);
                }

                List<Branch> newBranches = new List<Branch>();
                foreach(var branch in branches)
                {
                    if (branch.growCount > 0 && branch.growCount != branch.prevGrowCount)
                    {
                        Vector2 newDirection = branch.direction;

                        newDirection /= branch.growCount;
                        newDirection = newDirection.normalized;

                        newBranches.Add(new Branch(branch.position + (newDirection * stepSize), branch));
;
                        branch.prevGrowCount = branch.growCount; 
                        branch.growCount = 0;
                        branch.ResetDirection();
                    }
                }
                branches.AddRange(newBranches);

                if (newBranches.Count == 0)
                {
                    break;
                }
            }

            foreach (var branch in branches)
            {
                if (branch.parent != null)
                {
                    tree.ConnectCells(branch.parent.position, branch.position, 1);
                }
            }

            return tree;
        }
        #endregion

        #region Utility Methods

        private List<Vector2> SpawnLeaves()
        {
            System.Random rand = new System.Random(seed);

            List<Vector2> leaves = new List<Vector2>();

            for(int i = 0; i < leafCount; i++)
            {
                Vector2Int leaf = new Vector2Int(rand.Next(tree.Width), rand.Next(tree.Height));
                bool spawnLeaf = true;
                if (mask != null)
                {
                    if (mask[leaf.x, leaf.y] <= 0) spawnLeaf = false;
                }
                if (spawnLeaf)
                {
                    leaves.Add(leaf);
                }
            }

            return leaves;
        }
        #endregion

        #region Sub Classes

        class Branch
        {
            public Vector2 position;
            public Vector2 direction = Vector2.up;
            public Branch parent;
            public int growCount = 0;

            public int prevGrowCount = -1;

            public Branch(Vector2 position, Branch parent)
            {
                this.position = position;
                this.parent = parent;

                ResetDirection();
            }

            public void ResetDirection()
            {
                if (parent != null)
                {
                    direction = (position - parent.position).normalized;
                }
                else
                {
                    direction = Vector2.up;
                }
            }
        }

        #endregion
    }
}
