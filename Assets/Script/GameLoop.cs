using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoopManager : MonoBehaviour
{
    public GameObject player1Boat;
    public GameObject player2Boat;
    public Camera mainCamera;
    public Slider forceBar; // Référence à la barre de force
    public float rotationSpeed = 100f;
    public float maxForce = 50f; // Force maximale
    public float forceChargeSpeed = 50f; // Vitesse de chargement de la force
    public float cameraDistance = 5f;
    public float cameraHeight = 2f;
    public GameObject pauseMenu;
    public GameObject gameOverCanvas; // Référence au Canvas de fin de jeu
    public TextMeshProUGUI winnerText; // Texte pour afficher le gagnant

    private int currentPlayer = 1; // 1 pour joueur 1, 2 pour joueur 2
    private int player1Score = 0;
    private int player2Score = 0;
    private bool isWaitingForAction = true; // Le joueur peut tourner ou avancer
    private bool isGameOver = false; // Flag pour savoir si le jeu est terminé
    private bool isChargingForce = false; // Indique si la barre de force est en train de se charger
    private float currentForce = 0f; // Force actuelle
    private bool isPaused = false;
    private string player1Name = GameData.Player1Name;
    private string player2Name = GameData.Player2Name;
    private string player1Type = GameData.Player1Type;
    private string player2Type = GameData.Player2Type;

    void Start()
    {
        Debug.Log("Le jeu commence !");
        SetupBoats();
        StartPlayerTurn();
        forceBar.gameObject.SetActive(false); // Cacher la barre de force au début
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }

        if (isPaused || isGameOver) return;

        

        if (isWaitingForAction)
        {
            HandlePlayerRotation();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!isChargingForce) StartForceSelection(); // Début de la charge de force
                else ApplyForce(); // Appliquer la force
            }

            if (isChargingForce) UpdateForceBar(); // Mettre à jour la barre de force
        }

        UpdateCameraPosition();
    }
    
    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0; // Arrête le temps
            pauseMenu.SetActive(true); // Affiche le menu de pause
        }
        else
        {
            Time.timeScale = 1; // Relance le temps
            pauseMenu.SetActive(false); // Cache le menu de pause
        }

        Debug.Log($"Pause : {isPaused}");
    }
    
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // Relance le temps
        pauseMenu.SetActive(false); // Cache le menu de pause
        Debug.Log("Jeu repris !");
    }
    
    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Assurez-vous que le temps est réinitialisé
        SceneManager.LoadScene("MainMenu"); 
    }

    void SetupBoats()
    {
        Rigidbody rb1 = player1Boat.GetComponent<Rigidbody>();
        Rigidbody rb2 = player2Boat.GetComponent<Rigidbody>();

        if (rb1 != null)
        {
            rb1.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        if (rb2 != null)
        {
            rb2.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    void StartPlayerTurn()
    {
        if (CheckIfGameOver()) return;

        Debug.Log($"Tour du joueur {currentPlayer}");
        isWaitingForAction = true;
    }

    void HandlePlayerRotation()
    {
        GameObject activeBoat = currentPlayer == 1 ? player1Boat : player2Boat;

        // Récupérer l'input pour la rotation
        float rotationInput = Input.GetKey(KeyCode.A) ? -1 : (Input.GetKey(KeyCode.D) ? 1 : 0);

        // Appliquer la rotation
        activeBoat.transform.Rotate(Vector3.up * rotationInput * rotationSpeed * Time.deltaTime);
    }

    void StartForceSelection()
    {
        isChargingForce = true;
        currentForce = 0f;
        forceBar.value = 0;
        forceBar.gameObject.SetActive(true); // Afficher la barre de force
    }

    void UpdateForceBar()
    {
        currentForce += forceChargeSpeed * Time.deltaTime;

        // Boucler la force pour recommencer à 0 après le maximum
        if (currentForce > maxForce)
        {
            currentForce = 0;
        }

        forceBar.value = currentForce / maxForce; // Mettre à jour la valeur de la barre
        Debug.Log($"Force actuelle : {currentForce}, Slider value : {forceBar.value}");
    }

    void ApplyForce()
    {
        isChargingForce = false;
        forceBar.gameObject.SetActive(false); // Cacher la barre de force

        GameObject activeBoat = currentPlayer == 1 ? player1Boat : player2Boat;

        // Appliquer la force au bateau actif
        Rigidbody rb = activeBoat.GetComponent<Rigidbody>();
        rb.AddForce(activeBoat.transform.forward * currentForce, ForceMode.Impulse);

        Debug.Log($"Force appliquée : {currentForce}");

        // Fin du tour après un délai
        StartCoroutine(EndTurnAfterDelay(2f));
        isWaitingForAction = false;
    }

    IEnumerator EndTurnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Passer au joueur suivant
        currentPlayer = currentPlayer == 1 ? 2 : 1;

        if (CheckIfGameOver()) yield break;

        StartPlayerTurn();
    }

    void UpdateCameraPosition()
    {
        GameObject activeBoat = currentPlayer == 1 ? player1Boat : player2Boat;

        // Position derrière le bateau
        Vector3 desiredPosition = activeBoat.transform.position - activeBoat.transform.forward * cameraDistance + Vector3.up * cameraHeight;

        // Appliquer la position
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, Time.deltaTime * 5f);

        // Regarder dans la direction du bateau
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, Quaternion.LookRotation(activeBoat.transform.forward), Time.deltaTime * 5f);
    }

    bool CheckIfGameOver()
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");

        if (coins.Length == 0)
        {
            EndGame();
            return true;
        }

        return false;
    }

    void EndGame()
    {
        isGameOver = true;
        Time.timeScale = 0; // Arrêter le temps de jeu

        Debug.Log("Partie terminée !");
        Debug.Log($"Score Joueur 1 : {player1Score}");
        Debug.Log($"Score Joueur 2 : {player2Score}");

        // Afficher le gagnant
        if (player1Score > player2Score)
        {
            winnerText.text = "Joueur 1 gagne!";
            Debug.Log("Joueur 1 gagne !");
        }
        else if (player2Score > player1Score)
        {
            winnerText.text = "Joueur 2 gagne!";
            Debug.Log("Joueur 2 gagne !");
        }
        else
        {
            winnerText.text = "Match nul!";
            Debug.Log("Match nul !");
        }

        // Activer l'écran de fin de jeu
        gameOverCanvas.SetActive(true);
        
        StartCoroutine(ReturnToMainMenuAfterDelay(4f));
    }
    
    IEnumerator ReturnToMainMenuAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // Utiliser WaitForSecondsRealtime pour ignorer Time.timeScale
        Time.timeScale = 1; // Réinitialiser le temps avant de charger une nouvelle scène
        GoToMainMenu(); // Remplacez "MainMenu" par le nom exact de votre scène de menu principal
    }


    public void CoinCollected(int playerNumber, GameObject coin)
    {
        // Mise à jour des scores
        if (playerNumber == 1)
        {
            player1Score++;
        }
        else if (playerNumber == 2)
        {
            player2Score++;
        }

        // Détruire la pièce collectée
        Destroy(coin);

        Debug.Log($"Scores - Joueur 1 : {player1Score}, Joueur 2 : {player2Score}");

        // Vérifiez si toutes les pièces ont été collectées
        if (CheckIfGameOver())
        {
            EndGame();
        }
    }
    
    public int GetPlayerScore(int player)
    {
        if (player == 1)
            return player1Score;
        else if (player == 2)
            return player2Score;

        return 0;
    }
}


