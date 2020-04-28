using System.Collections;
using System.Collections.Generic;
using Terra2D;
using UnityEngine;

namespace Terra2D
{
    public class MapToMesh : MonoBehaviour
    {
        private MeshGenerator meshGenerator = new MeshGenerator();  

        public MeshFilter mesh;
        public MeshFilter wallMesh;

        public Terra2DGraph graph;
        public string output;
        public int index;
        public float size;
        public float wallThickness;
        public float textureTilePercentage;

        public void GenerateMesh()
        {
            gameObject.isStatic = true;

            var map = graph.GetMapData(output);

            mesh.mesh = meshGenerator.GenerateMesh(map, index, size, textureTilePercentage);

            if (wallMesh != null)
            {
                wallMesh.gameObject.isStatic = true;
                if (wallThickness != 0) wallMesh.mesh = meshGenerator.GenerateWallMesh(wallThickness);

            }

            meshGenerator.Generate2DColliders(gameObject);
        }
    }
}