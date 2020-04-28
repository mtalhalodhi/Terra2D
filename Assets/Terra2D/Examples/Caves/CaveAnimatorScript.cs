using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terra2D;

public class CaveAnimatorScript : MonoBehaviour
{
    public Terra2DGraph graph;
    public SelectNode selectNode;
    public MapToMesh converter;

    public float speed = 0.01f;

    void Start()
    {
        selectNode = graph.nodes.Find(n => n.name == "Select") as SelectNode;
        selectNode.value = 1;
    }
    
    void Update()
    {
        if (selectNode.value > 0) selectNode.value -= speed;
        converter.GenerateMesh();
    }
}
