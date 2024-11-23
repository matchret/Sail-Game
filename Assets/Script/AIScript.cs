using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    // Références aux composants nécessaires
    private WaterGridGenerator gridGenerator;       // Générateur de grille d'eau
    private GameLoopManager gameLoopManager;        // Gestionnaire de la boucle de jeu

    // Variables pour le chemin
    private List<Vector3> currentPath = new List<Vector3>();    // Chemin actuel
    private int currentWaypointIndex = 0;                        // Index du point de cheminement actuel

    // Gestion des pièces
    private GameObject currentTargetCoin;           // Pièce cible actuelle
    private List<GameObject> remainingCoins = new List<GameObject>();   // Liste des pièces restantes
    

    void Start()
    {
        gridGenerator = FindObjectOfType<WaterGridGenerator>();
        gameLoopManager = FindObjectOfType<GameLoopManager>();

        // Initialiser la liste des pièces restantes
        remainingCoins = new List<GameObject>(GameObject.FindGameObjectsWithTag("Coin"));

        // Trouver la première pièce
        FindNextCoin();
    }

    void Update()
    {
        // Si un chemin est disponible, le dessiner
        if (currentPath != null && currentPath.Count > 0)
        {
            DrawPath();
        }
    }

    // Trouver la pièce la plus proche et calculer le chemin vers celle-ci
    void FindNextCoin()
    {
        // Mettre à jour la liste des pièces restantes
        remainingCoins = new List<GameObject>(GameObject.FindGameObjectsWithTag("Coin"));

        if (remainingCoins.Count == 0)
        {
            Debug.Log("AIScript : Toutes les pièces ont été collectées !");
            currentPath = null;
            return;
        }

        // Trouver la pièce la plus proche
        currentTargetCoin = FindClosestCoin();

        if (currentTargetCoin != null)
        {
            Debug.Log($"AIScript : Pièce cible trouvée à {currentTargetCoin.transform.position}");

            // Trouver le chemin le plus court vers la pièce
            currentPath = gridGenerator.FindPath(transform.position, currentTargetCoin.transform.position);

            if (currentPath != null && currentPath.Count > 0)
            {
                currentWaypointIndex = 0;
                Debug.Log($"AIScript : Chemin vers la pièce trouvé avec {currentPath.Count} points.");
            }
            else
            {
                Debug.LogWarning("AIScript : Aucun chemin trouvé vers la pièce !");
                // Si aucun chemin n'est trouvé, retirer la pièce de la liste et essayer avec une autre
                remainingCoins.Remove(currentTargetCoin);
                currentTargetCoin = null;
                currentPath = null;
                FindNextCoin();
            }
        }
    }

    // Trouver la pièce la plus proche
    GameObject FindClosestCoin()
    {
        GameObject closestCoin = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject coin in remainingCoins)
        {
            float distance = Vector3.Distance(transform.position, coin.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCoin = coin;
            }
        }

        return closestCoin;
    }

    // Dessiner le chemin pour le débogage
    void DrawPath()
    {
        if (currentPath != null && currentPath.Count > 1)
        {
            // Dessiner le chemin en utilisant Debug.DrawLine
            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                Debug.DrawLine(currentPath[i], currentPath[i + 1], Color.magenta);
            }
        }
    }

    public void PerformTurn()
    {
        FindNextCoin();
        Vector3 targetnode;

       if (50f > Vector3.Distance(currentTargetCoin.transform.position, transform.position)){
            targetnode = currentTargetCoin.transform.position;
        }
        else {
            targetnode = ChoseTargetPathNodeNotEncombred(); //use ray to see if anything in front
        }


        Debug.Log($"Target Node in the Path :{targetnode}");
        GetComponent<BoatController>().RotateTowardsPoint(targetnode);


        
    }

    private Vector3 ChoseTargetPathNodeNotEncombred()
    {
        float maxDistance = 50f;
        if (currentPath == null || currentPath.Count == 0)
        {
            Debug.Log("Chemin non disponible.");
            return transform.position;
        }

        Vector3 startPosition = transform.position;
        Vector3 targetNode = startPosition;

        // Parcourir le chemin du premier au dernier nœud
        for (int i = 0; i < currentPath.Count; i++)
        {
            Vector3 node = currentPath[i];

            // Calculer la distance en 2D (ignorer la coordonnée y)
            float distanceToNode = Vector2.Distance(new Vector2(startPosition.x, startPosition.z), new Vector2(node.x, node.z));

            Debug.Log($"Vérification du nœud {i} à la position {node}, distance 2D : {distanceToNode}");

            if (distanceToNode <= maxDistance)
            {
                if (IsPathClear(startPosition, node))
                {
                   // Debug.Log($"Nœud {i} est accessible.");
                    targetNode = node;
                }
            }
            else
            {
               // Debug.Log($"Nœud {i} est hors de portée.");
                break;
            }
        }

       // Debug.Log($"Nœud cible choisi : {targetNode}");
        return targetNode;
    }

    private bool IsPathClear(Vector3 start, Vector3 end)
    {
        start = start + transform.forward * 2f;
        start.y = 2;

        Vector3 direction = end - start ;
        direction.y = 2;
        
        float distance = direction.magnitude;
        direction.Normalize();

        // Dessiner le raycast en rouge
        Debug.DrawRay(start, direction * distance, Color.red, 0.1f);

        LayerMask obstacleLayerMask = LayerMask.GetMask("Default");

        if (Physics.Raycast(start, direction, distance, obstacleLayerMask))
        {
            return false;
        }

        return true;
    }
}