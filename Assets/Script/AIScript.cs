using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    // R�f�rences aux composants n�cessaires
    private BoatController boatController;          // Contr�leur du bateau
    private WaterGridGenerator gridGenerator;       // G�n�rateur de grille d'eau
    private GameLoopManager gameLoopManager;        // Gestionnaire de la boucle de jeu

    // Variables pour le chemin
    private List<Vector3> currentPath = new List<Vector3>();    // Chemin actuel
    private int currentWaypointIndex = 0;                        // Index du point de cheminement actuel

    // Gestion des pi�ces
    private GameObject currentTargetCoin;           // Pi�ce cible actuelle
    private List<GameObject> remainingCoins = new List<GameObject>();   // Liste des pi�ces restantes

    void Start()
    {
        // R�cup�rer les r�f�rences n�cessaires
        boatController = GetComponent<BoatController>();
        gridGenerator = FindObjectOfType<WaterGridGenerator>();
        gameLoopManager = FindObjectOfType<GameLoopManager>();

        if (boatController == null || gridGenerator == null || gameLoopManager == null)
        {
            Debug.LogError("AIScript : Composants requis manquants !");
            enabled = false;  // D�sactiver le script si des r�f�rences sont manquantes
            return;
        }

        // Initialiser la liste des pi�ces restantes
        remainingCoins = new List<GameObject>(GameObject.FindGameObjectsWithTag("Coin"));

        // Trouver la premi�re pi�ce
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
            // Si aucun chemin n'est disponible, essayer de trouver la prochaine pi�ce
            FindNextCoin();
        }
    }

    // Trouver la pi�ce la plus proche et calculer le chemin vers celle-ci
    void FindNextCoin()
    {
        // Mettre � jour la liste des pi�ces restantes
        remainingCoins = new List<GameObject>(GameObject.FindGameObjectsWithTag("Coin"));

        if (remainingCoins.Count == 0)
        {
            Debug.Log("AIScript : Toutes les pi�ces ont �t� collect�es !");
            currentPath = null;
            return;
        }

        // Trouver la pi�ce la plus proche
        currentTargetCoin = FindClosestCoin();

        if (currentTargetCoin != null)
        {
            Debug.Log($"AIScript : Pi�ce cible trouv�e � {currentTargetCoin.transform.position}");

            // Trouver le chemin le plus court vers la pi�ce
            currentPath = gridGenerator.FindPath(transform.position, currentTargetCoin.transform.position);

            if (currentPath != null && currentPath.Count > 0)
            {
                currentWaypointIndex = 0;
                Debug.Log($"AIScript : Chemin vers la pi�ce trouv� avec {currentPath.Count} points.");
            }
            else
            {
                Debug.LogWarning("AIScript : Aucun chemin trouv� vers la pi�ce !");
                // Si aucun chemin n'est trouv�, retirer la pi�ce de la liste et essayer avec une autre
                remainingCoins.Remove(currentTargetCoin);
                currentTargetCoin = null;
                currentPath = null;
                FindNextCoin();
            }
        }
    }

    // Trouver la pi�ce la plus proche
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

    // Dessiner le chemin pour le d�bogage
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