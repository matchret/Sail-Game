using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;


public class WaterGridGenerator : MonoBehaviour
{
    public GameObject parentObject; // Empty object containing the meshes
    public int heightOfGrid = 10; // Height of the grid above water
    public int gridResolutionX = 100; // Number of cells along X-axis
    public int gridResolutionZ = 100; // Number of cells along Z-axis

    public List<Vector3> openNodes = new List<Vector3>(); // List of open nodes for movement

    void Start()
    {
        if (parentObject == null)
        {
            Debug.LogError("Parent object is not assigned!");
            return;
        }

        // Get the combined bounds of all meshes
        Bounds combinedBounds = CalculateCombinedBounds(parentObject);

        if (combinedBounds.size != Vector3.zero)
        {
            // Calculate the farthest points in 2D (horizontal plane)
            Vector3 min = combinedBounds.min;
            Vector3 max = combinedBounds.max;

            Vector3 bottomLeft = new Vector3(min.x, max.y + heightOfGrid, min.z);
            Vector3 bottomRight = new Vector3(max.x, max.y + heightOfGrid, min.z);
            Vector3 topLeft = new Vector3(min.x, max.y + heightOfGrid, max.z);

            Debug.Log($"Bottom Left: {bottomLeft}");
            Debug.Log($"Bottom Right: {bottomRight}");
            Debug.Log($"Top Left: {topLeft}");

            // Generate the grid and find open nodes
            GenerateGrid(bottomLeft, bottomRight, topLeft);
        }
        else
        {
            Debug.LogError("No valid meshes found to calculate bounds!");
        }

        ConnectNodes();


    }

    void Update()
    {
        // Draw an upward line for each open node
        foreach (Vector3 node in openNodes)
        {
            Debug.DrawLine(node, node + Vector3.up * 5f, Color.blue); // Draw a 5-unit upward line
        }
    }

    Bounds CalculateCombinedBounds(GameObject obj)
    {
        Bounds bounds = new Bounds();
        MeshRenderer[] meshRenderers = obj.GetComponentsInChildren<MeshRenderer>();

        if (meshRenderers.Length == 0)
        {
            Debug.LogWarning("No MeshRenderers found in the hierarchy!");
            return bounds;
        }

        // Initialize bounds
        bool boundsInitialized = false;

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            if (!boundsInitialized)
            {
                bounds = meshRenderer.bounds;
                boundsInitialized = true;
            }
            else
            {
                bounds.Encapsulate(meshRenderer.bounds);
            }
        }

        return bounds;
    }

    void GenerateGrid(Vector3 bottomLeft, Vector3 bottomRight, Vector3 topLeft)
    {
        // Calculate the grid spacing
        Vector3 horizontalStep = (bottomRight - bottomLeft) / gridResolutionX;
        Vector3 verticalStep = (topLeft - bottomLeft) / gridResolutionZ;

        // Iterate through the grid
        for (int i = 0; i < gridResolutionZ; i++)
        {
            for (int j = 0; j < gridResolutionX; j++)
            {
                // Calculate the four corners of the grid cell
                Vector3 cellBottomLeft = bottomLeft + horizontalStep * j + verticalStep * i;
                Vector3 cellBottomRight = cellBottomLeft + horizontalStep;
                Vector3 cellTopLeft = cellBottomLeft + verticalStep;
                Vector3 cellTopRight = cellBottomLeft + horizontalStep + verticalStep;

                // Check if all four points are non-obstructed
                if (!IsPointObstructed(cellBottomLeft) &&
                    !IsPointObstructed(cellBottomRight) &&
                    !IsPointObstructed(cellTopLeft) &&
                    !IsPointObstructed(cellTopRight))
                {
                    // Add the center of the square to the list of open nodes
                    Vector3 cellCenter = (cellBottomLeft + cellTopRight) / 2f;
                    openNodes.Add(cellCenter);

                    // Draw the valid cell in green
                    DrawCell(cellBottomLeft, cellBottomRight, cellTopLeft, cellTopRight, Color.green);
                }
                else
                {
                    // Draw the obstructed cell in red
                    DrawCell(cellBottomLeft, cellBottomRight, cellTopLeft, cellTopRight, Color.red);
                }
            }
        }

        Debug.Log($"Total open nodes: {openNodes.Count}");
    }

    bool IsPointObstructed(Vector3 point)
    {
        // Cast a ray from above to detect obstructions
        Vector3 rayOrigin = point + Vector3.up * 100f; // Start the ray well above the point
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 150f))
        {
            // Check if the hit point is not on the water surface
            if (!hit.collider.CompareTag("Water")) // Assuming water has the "Water" tag
            {
                return true; // Obstructed
            }
        }

        return false; // Not obstructed
    }

    void DrawCell(Vector3 bottomLeft, Vector3 bottomRight, Vector3 topLeft, Vector3 topRight, Color color)
    {
        Debug.DrawLine(bottomLeft, bottomRight, color, 10f); // Bottom edge
        Debug.DrawLine(bottomRight, topRight, color, 10f); // Right edge
        Debug.DrawLine(topRight, topLeft, color, 10f); // Top edge
        Debug.DrawLine(topLeft, bottomLeft, color, 10f); // Left edge
    }

    public List<PathNode> graphNodes = new List<PathNode>();

    void ConnectNodes(float connectionRadius = 5f)
    {
        // Create PathNodes for each open node
        foreach (Vector3 openNodePosition in openNodes)
        {
            graphNodes.Add(new PathNode(openNodePosition));
        }

        // Connect nodes within the connection radius
        foreach (PathNode node in graphNodes)
        {
            foreach (PathNode potentialNeighbor in graphNodes)
            {
                if (node != potentialNeighbor &&
                    Vector3.Distance(node.position, potentialNeighbor.position) <= connectionRadius)
                {
                    node.neighbors.Add(potentialNeighbor);
                }
            }
        }

        Debug.Log($"Graph created with {graphNodes.Count} nodes.");
    }
}
