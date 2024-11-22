using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PathNode
{
    public Vector3 position; // Node position
    public List<PathNode> neighbors = new List<PathNode>(); // Adjacent nodes
    public float gCost = Mathf.Infinity; // Cost from start to this node
    public float hCost = 0; // Heuristic cost to target node
    public float fCost => gCost + hCost; // Total cost
    public PathNode parent; // Parent node for path reconstruction

    public PathNode(Vector3 pos)
    {
        position = pos;
    }
}