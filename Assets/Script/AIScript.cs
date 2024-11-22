using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    // Références aux composants nécessaires
    private BoatController boatController;          // Contrôleur du bateau
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
        // Récupérer les références nécessaires
        boatController = GetComponent<BoatController>();
        gridGenerator = FindObjectOfType<WaterGridGenerator>();
        gameLoopManager = FindObjectOfType<GameLoopManager>();

        if (boatController == null || gridGenerator == null || gameLoopManager == null)
        {
            Debug.LogError("AIScript : Composants requis manquants !");
            enabled = false;  // Désactiver le script si des références sont manquantes
            return;
        }

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
        else
        {
            // Si aucun chemin n'est disponible, essayer de trouver la prochaine pièce
            FindNextCoin();
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
}