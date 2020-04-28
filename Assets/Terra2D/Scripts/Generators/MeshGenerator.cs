using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Terra2D
{
    public class MeshGenerator
    {
        #region Variables

        private SquareGrid grid;
        private List<Vector3> vertices;
        private List<int> triangles;

        Dictionary<int, List<Triangle>> triangleDictionary = new Dictionary<int, List<Triangle>>();
        List<List<int>> outlines = new List<List<int>>();
        HashSet<int> checkedVertices = new HashSet<int>();

        #endregion

        #region Main Methods

        public Mesh GenerateMesh(MapData map, int index, float size, float textureTileAmount)
        {
            var thickMap = ScriptableObject.CreateInstance<MapData>();
            thickMap.Initialize(map.Width + 2, map.Height + 2);

            for (int x = 0; x < thickMap.Width; x++)
            {
                for (int y = 0; y < thickMap.Height; y++)
                {
                    if (x > 0 && x < thickMap.Width - 1 && y > 0 && y < thickMap.Height - 1)
                    {
                        thickMap[x, y] = map[x - 1, y - 1];
                    }
                    else
                    {
                        thickMap[x, y] = int.MinValue;
                    }
                }
            }

            triangleDictionary.Clear();
            outlines.Clear();
            checkedVertices.Clear();

            vertices = new List<Vector3>();
            triangles = new List<int>();

            triangleDictionary = new Dictionary<int, List<Triangle>>();

            grid = new SquareGrid(thickMap, index, size);

            for (int x = 0; x < grid.squares.GetLength(0); x++)
            {
                for (int y = 0; y < grid.squares.GetLength(1); y++)
                {
                    TriangulateSquare(grid.squares[x, y]);
                }
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();

            Vector2[] uvs = new Vector2[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                float percentX = Mathf.InverseLerp(-map.Width / 2 * size, map.Width / 2 * size, vertices[i].x) * textureTileAmount;
                float percentY = Mathf.InverseLerp(-map.Height / 2 * size, map.Height / 2 * size, vertices[i].y) * textureTileAmount;

                uvs[i] = new Vector2(percentX, percentY);
            }

            mesh.uv = uvs;

            return mesh;
        }

        public Mesh GenerateWallMesh(float thickness)
        {
            CalculateMeshOutlines();

            List<Vector3> wallVertices = new List<Vector3>();
            List<int> wallTriangles = new List<int>();

            Mesh wallMesh = new Mesh();

            foreach (var outline in outlines)
            {
                for (int i = 0; i < outline.Count - 1; i++)
                {
                    int startIndex = wallVertices.Count;
                    wallVertices.Add(vertices[outline[i]]); // left
                    wallVertices.Add(vertices[outline[i + 1]]); // right
                    wallVertices.Add(vertices[outline[i]] - Vector3.back * thickness); // bottom left
                    wallVertices.Add(vertices[outline[i + 1]] - Vector3.back * thickness); // bottom right

                    wallTriangles.Add(startIndex + 0);
                    wallTriangles.Add(startIndex + 2);
                    wallTriangles.Add(startIndex + 3);

                    wallTriangles.Add(startIndex + 3);
                    wallTriangles.Add(startIndex + 1);
                    wallTriangles.Add(startIndex + 0);
                }
            }

            wallMesh.vertices = wallVertices.ToArray();
            wallMesh.triangles = wallTriangles.ToArray();
            wallMesh.RecalculateNormals();

            return wallMesh;
        }

        public List<EdgeCollider2D> Generate2DColliders(GameObject gameObject)
        {
            CalculateMeshOutlines();

            EdgeCollider2D[] currentColliders = gameObject.GetComponents<EdgeCollider2D>();
            for (int i = 0; i < currentColliders.Length; i++)
            {
#if UNITY_EDITOR
                GameObject.DestroyImmediate(currentColliders[i]);
#else
                GameObject.Destroy(currentColliders[i]);
#endif
            }

            List<EdgeCollider2D> colliders = new List<EdgeCollider2D>();

            foreach (var outline in outlines)
            {
                EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
                Vector2[] edgePoints = new Vector2[outline.Count];
                for (int i = 0; i < outline.Count; i++)
                {
                    edgePoints[i] = vertices[outline[i]];
                }
                edgeCollider.points = edgePoints;
                colliders.Add(edgeCollider);
            }

            return colliders;
        }

        #endregion

        #region Utility Methods

        void TriangulateSquare(Square square)
        {
            switch (square.configuration)
            {
                case 0:
                    break;

                case 1:
                    MeshFromPoints(square.centreLeft, square.centreBottom, square.bottomLeft);
                    break;
                case 2:
                    MeshFromPoints(square.bottomRight, square.centreBottom, square.centreRight);
                    break;
                case 4:
                    MeshFromPoints(square.topRight, square.centreRight, square.centreTop);
                    break;
                case 8:
                    MeshFromPoints(square.topLeft, square.centreTop, square.centreLeft);
                    break;

                case 3:
                    MeshFromPoints(square.centreRight, square.bottomRight, square.bottomLeft, square.centreLeft);
                    break;
                case 6:
                    MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.centreBottom);
                    break;
                case 9:
                    MeshFromPoints(square.topLeft, square.centreTop, square.centreBottom, square.bottomLeft);
                    break;
                case 12:
                    MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreLeft);
                    break;
                case 5:
                    MeshFromPoints(square.centreTop, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft, square.centreLeft);
                    break;
                case 10:
                    MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.centreBottom, square.centreLeft);
                    break;

                case 7:
                    MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft);
                    break;
                case 11:
                    MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft);
                    break;
                case 13:
                    MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft);
                    break;
                case 14:
                    MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centreBottom, square.centreLeft);
                    break;

                case 15:
                    MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
                    checkedVertices.Add(square.topLeft.index);
                    checkedVertices.Add(square.topRight.index);
                    checkedVertices.Add(square.bottomRight.index);
                    checkedVertices.Add(square.bottomLeft.index);
                    break;
            }
        }

        private void MeshFromPoints(params Node[] points)
        {
            AssignVertices(points);

            if (points.Length >= 3)
            {
                CreateTriangle(points[0], points[1], points[2]);
            }
            if (points.Length >= 4)
            {
                CreateTriangle(points[0], points[2], points[3]);
            }
            if (points.Length >= 5)
            {
                CreateTriangle(points[0], points[3], points[4]);
            }
            if (points.Length >= 6)
            {
                CreateTriangle(points[0], points[4], points[5]);
            }
        }

        private void AssignVertices(Node[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].index == -1)
                {
                    points[i].index = vertices.Count;
                    vertices.Add(points[i].position);
                }
            }
        }

        private void CreateTriangle(Node a, Node b, Node c)
        {
            triangles.Add(a.index);
            triangles.Add(b.index);
            triangles.Add(c.index);

            Triangle triangle = new Triangle(a.index, b.index, c.index);

            AddTriangleToDictionary(triangle.indexA, triangle);
            AddTriangleToDictionary(triangle.indexB, triangle);
            AddTriangleToDictionary(triangle.indexC, triangle);
        }

        void AddTriangleToDictionary(int index, Triangle triangle)
        {
            if (triangleDictionary.ContainsKey(index))
            {
                triangleDictionary[index].Add(triangle);
            }
            else
            {
                List<Triangle> triangleList = new List<Triangle>();
                triangleList.Add(triangle);
                triangleDictionary.Add(index, triangleList);
            }
        }

        void CalculateMeshOutlines()
        {
            for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
            {
                if (!checkedVertices.Contains(vertexIndex))
                {
                    int newOutlineVertex = GetConnectedOutlineVertex(vertexIndex);
                    if (newOutlineVertex != -1)
                    {
                        checkedVertices.Add(vertexIndex);

                        List<int> newOutline = new List<int>();
                        newOutline.Add(vertexIndex);
                        outlines.Add(newOutline);
                        FollowOutline(newOutlineVertex, outlines.Count - 1);
                        outlines[outlines.Count - 1].Add(vertexIndex);
                    }
                }
            }
        }

        void FollowOutline(int vertexIndex, int outlineIndex)
        {
            outlines[outlineIndex].Add(vertexIndex);
            checkedVertices.Add(vertexIndex);
            int nextVertexIndex = GetConnectedOutlineVertex(vertexIndex);

            if (nextVertexIndex != -1)
            {
                FollowOutline(nextVertexIndex, outlineIndex);
            }
        }

        int GetConnectedOutlineVertex(int vertexIndex)
        {
            List<Triangle> trianglesContainingVertex = triangleDictionary[vertexIndex];

            for (int i = 0; i < trianglesContainingVertex.Count; i++)
            {
                Triangle triangle = trianglesContainingVertex[i];
                for (int j = 0; j < 3; j++)
                {
                    int vertexB = triangle[j];

                    if (vertexB != vertexIndex && !checkedVertices.Contains(vertexB))
                    {
                        if (IsOutlineEdge(vertexIndex, vertexB))
                        {
                            return vertexB;
                        }
                    }
                }
            }

            return -1;
        }

        bool IsOutlineEdge(int vertexA, int vertexB)
        {
            List<Triangle> trianglesContainingVertexA = triangleDictionary[vertexA];
            int sharedTriangleCount = 0;

            for (int i = 0; i < trianglesContainingVertexA.Count; i++)
            {
                if (trianglesContainingVertexA[i].Contains(vertexB))
                {
                    sharedTriangleCount++;
                    if (sharedTriangleCount > 1)
                    {
                        break;
                    }
                }
            }

            return sharedTriangleCount == 1;
        }

        #endregion

        #region Sub Classes

        struct Triangle
        {
            private int[] vertices;

            public int indexA;
            public int indexB;
            public int indexC;

            public int this[int index]
            {
                get
                {
                    return vertices[index];
                }
            }

            public Triangle(int a, int b, int c)
            {
                indexA = a;
                indexB = b;
                indexC = c;

                vertices = new int[3];
                vertices[0] = a;
                vertices[1] = b;
                vertices[2] = c;
            }

            public bool Contains(int index)
            {
                return index == indexA || index == indexB || index == indexC;
            }
        }

        public class SquareGrid
        {
            public Square[,] squares;

            public SquareGrid(MapData map, int index, float size)
            {
                ControlNode[,] controlNodes = new ControlNode[map.Width, map.Height];

                for (int x = 0; x < map.Width; x++)
                {
                    for (int y = 0; y < map.Height; y++)
                    {
                        Vector3 pos = new Vector3(-map.Width * size / 2 + x * size + size / 2, -map.Height * size / 2 + y * size + size / 2);
                        controlNodes[x, y] = new ControlNode(pos, map[x, y] == index, size);
                    }
                }

                squares = new Square[map.Width - 1, map.Height - 1];

                for (int x = 0; x < map.Width - 1; x++)
                {
                    for (int y = 0; y < map.Height - 1; y++)
                    {
                        squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                    }
                }
            }
        }

        public class Square
        {
            public ControlNode topLeft, topRight, bottomRight, bottomLeft;
            public Node centreTop, centreRight, centreBottom, centreLeft;
            public int configuration;

            public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft)
            {
                this.topLeft = topLeft;
                this.topRight = topRight;
                this.bottomRight = bottomRight;
                this.bottomLeft = bottomLeft;

                centreTop = topLeft.right;
                centreRight = bottomRight.above;
                centreBottom = bottomLeft.right;
                centreLeft = bottomLeft.above;

                if (topLeft.active)
                {
                    configuration += 8;
                }
                if (topRight.active)
                {
                    configuration += 4;
                }
                if (bottomRight.active)
                {
                    configuration += 2;
                }
                if (bottomLeft.active)
                {
                    configuration += 1;
                }
            }
        }

        public class Node
        {
            public Vector3 position;
            public int index;

            public Node(Vector3 position)
            {
                this.position = position;
                index = -1;
            }
        }

        public class ControlNode : Node
        {
            public bool active;
            public Node above, right;

            public ControlNode(Vector3 position, bool active, float size) : base(position)
            {
                this.active = active;

                above = new Node(position + Vector3.up * size / 2f);
                right = new Node(position + Vector3.right * size / 2f);
            }
        }

        #endregion
    }
}
