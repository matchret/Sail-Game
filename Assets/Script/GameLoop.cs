using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoopManager : MonoBehaviour
{
    public GameObject player1Boat;
    public GameObject player2Boat;
    public GameObject targetBoat;
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

    GameObject spawnedWind;
    GameObject spawnedWake;

    //simulation axis
    private float axisValue =0f;
    private float deadZone = 0.1f;
    private float gravity = 2f;
    private float maxValue = 1f;

    public GameObject controlCustomizationMenu;

    void Start()
    {

        if (AudioManager.Instance==null) {
            Debug.Log("AudioManager not found, creating one...");

            // Dynamically create a GameObject for AudioManager
            GameObject audioManagerObj = new GameObject("AudioManager");
            audioManagerObj.AddComponent<AudioManager>();
        }
        Debug.Log("Le jeu commence !");
        SetupBoats();
        StartPlayerTurn();
        AudioManager.Instance.PlayEnvironmentSounds();
        AudioManager.Instance.PlayGameMusic();
        ControlManager.Instance.IsAPlayerMouse();


        forceBar.gameObject.SetActive(false); // Cacher la barre de force au début
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        UpdateCameraPosition();
        if (isGameOver) return;

        GameObject activeBoat = currentPlayer == 1 ? player1Boat : player2Boat;
        // Update boat particles
        UpdateBoatParticles(activeBoat);

        if (ControlManager.Instance.IsActionPressed(ControlManager.Action.Pause) || ControlManager.Instance.IsActionPressed(ControlManager.Action.PauseP2))
        {
            TogglePause();
        }

        if (isWaitingForAction)
        {
            HandlePlayerRotation();

            //Ai Apply force
            bool isAIPlayer = (currentPlayer == 1 && GameData.Player1Type == "Ai") ||
                      (currentPlayer == 2 && GameData.Player2Type == "Ai");
            if (isAIPlayer) 
            {
                if (!isChargingForce) StartForceSelection();
                AIScript aiScript = activeBoat.GetComponent<AIScript>();

                if (aiScript != null)
                {
                    aiScript.PerformTurn();

                   if (activeBoat.GetComponent<BoatController>().MoveTowardsTarget(currentForce))
                    {
                        ApplyForce();
                    }

                  
                }
                
            }
            //If Human Player
            else {
                if ((ControlManager.Instance.IsActionPressed(ControlManager.Action.Dash) && currentPlayer == 1) || (ControlManager.Instance.IsActionPressed(ControlManager.Action.DashP2) && currentPlayer == 2))
                {
                    if (!isChargingForce) StartForceSelection();
                    else ApplyForce();
                }
            }

            if (isChargingForce) UpdateForceBar();
        }

        UpdateCameraPosition();
    }
    
    
    
    void TogglePause()
    {
        isPaused = !isPaused;
        AudioManager.Instance.StopEnvironmentSounds();

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
        AudioManager.Instance.PlayEnvironmentSounds();
        Debug.Log("Jeu repris !");
    }
    
    public void GoToMainMenu()
    {
        Time.timeScale = 1; 
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
        if (GameData.Player1Type == "Ai")
        {
            player1Boat.AddComponent<AIScript>();
        }

        if (rb2 != null)
        {
            rb2.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        if (GameData.Player2Type == "Ai")
        {
            player2Boat.AddComponent<AIScript>();
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


        bool isCurrentPlayerAI = (currentPlayer == 1 && GameData.Player1Type == "Ai") ||
                                 (currentPlayer == 2 && GameData.Player2Type == "Ai");

        if (isCurrentPlayerAI)
        {

        }
        else {
            //float rotationInput = 0;
            //if ((ControlManager.Instance.IsActionPressed(ControlManager.Action.MoveLeft) && currentPlayer == 1) || (ControlManager.Instance.IsActionPressed(ControlManager.Action.MoveLeftP2) && currentPlayer == 2))
            //    rotationInput = -1;
            //else if ((ControlManager.Instance.IsActionPressed(ControlManager.Action.MoveRight) && currentPlayer == 1) || (ControlManager.Instance.IsActionPressed(ControlManager.Action.MoveRightP2) && currentPlayer == 2))
            //    rotationInput = 1;

            float rotationInput;
            if (currentPlayer == 1)
            {
                rotationInput =
                    SimulateAxis(ControlManager.Instance.GetKeyBinding(ControlManager.Action.MoveLeft),
                        ControlManager.Instance.GetKeyBinding(ControlManager.Action.MoveRight));
                // Appliquer la rotation
            }
            else
            {
                rotationInput = SimulateAxis(
                    ControlManager.Instance.GetKeyBinding(ControlManager.Action.MoveLeftP2),
                    ControlManager.Instance.GetKeyBinding(ControlManager.Action.MoveRightP2));
                // Appliquer la rotation
            }
            activeBoat.transform.Rotate(Vector3.up * rotationInput * rotationSpeed * Time.deltaTime);
        }
    }
    
    float SimulateAxis(KeyCode leftKey, KeyCode rightKey)
    {
        float targetValue = 0f;

        if (Input.GetKey(leftKey))
        {
            targetValue -= maxValue; // Déplacement vers la gauche (valeur négative)
        }

        if (Input.GetKey(rightKey))
        {
            targetValue += maxValue; // Déplacement vers la droite (valeur positive)
        }
        
        if (Mathf.Abs(targetValue) < deadZone && Mathf.Abs(axisValue) < deadZone)
        {
            return 0f;
        }
        
        if (targetValue == 0)
        {
            // Retour vers zéro avec la gravité
            axisValue = Mathf.MoveTowards(axisValue, 0, gravity * Time.deltaTime);
        }
        else
        {
            if (currentPlayer == 1)
            {
                // Approcher la cible avec la sensibilité
                axisValue = Mathf.MoveTowards(axisValue, targetValue,
                    ControlManager.Instance.sensitivityP1 * Time.deltaTime);
            }
            else
            {
                axisValue = Mathf.MoveTowards(axisValue, targetValue,
                    ControlManager.Instance.sensitivityP2 * Time.deltaTime);  
            }
        }
        

        return Mathf.Clamp(axisValue, -1, 1); // Limiter l'axe entre -1 et 1
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

        Vector3 positionBoat = activeBoat.transform.position;


        AudioManager.Instance.PlayWindPushSound(positionBoat + Vector3.up * 8f);

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
        GameObject activeBoat;
        if (targetBoat != null)
        {
            // Si on a spécifié un bateau cible, l'utiliser
            activeBoat = targetBoat;
        }
        else
        {
            // Sinon, comportement normal
            activeBoat = currentPlayer == 1 ? player1Boat : player2Boat;
        }

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

        Debug.Log("Partie terminée !");
        Debug.Log($"Score Joueur 1 : {player1Score}");
        Debug.Log($"Score Joueur 2 : {player2Score}");
        
        targetBoat = null;
        Transform winnerVisual = null;

        if (player1Score > player2Score)
        {
            Debug.Log("Joueur 1 gagne !");
            targetBoat = player1Boat;
            UpdateCameraPosition();
            winnerVisual = player1Boat.transform.Find("BoatVisual");
        }
        else if (player2Score > player1Score)
        {
            Debug.Log("Joueur 2 gagne !");
            targetBoat = player2Boat;
            UpdateCameraPosition();
            winnerVisual = player2Boat.transform.Find("BoatVisual2");
        }
        else
        {
            Debug.Log("Match nul !");
            winnerText.text = "Match nul!";
            gameOverCanvas.SetActive(true);
            StartCoroutine(ReturnToMainMenuAfterDelay(4f));
            return;
        }
        winnerText.text = player1Score > player2Score ? "Joueur 1 gagne!" : "Joueur 2 gagne!";
        gameOverCanvas.SetActive(true);

        // Mettre à jour la caméra pour se centrer sur le vainqueur
        // La caméra se repositionnera pendant les prochains frames
        // On ne lance pas l'animation tout de suite
        StartCoroutine(WaitBeforeVictoryAnimation(winnerVisual));
    }
    
    IEnumerator WaitBeforeVictoryAnimation(Transform winnerVisual)
    {
        // Attendre un peu pour permettre à la caméra d'effectuer plusieurs Update()
        // et donc de se rapprocher du targetBoat
        yield return new WaitForSeconds(0.5f); // Ajustez le délai en fonction de la vitesse de Lerp

        if (winnerVisual != null)
        {
            Debug.Log("Lancement de l'animation de victoire.");
            BoatVictoryAnimation victoryAnim = winnerVisual.GetComponent<BoatVictoryAnimation>();
            if (victoryAnim != null)
            {
                victoryAnim.PlayVictoryAnimation();
            }
        }

        StartCoroutine(ReturnToMainMenuAfterDelay(4f));
        AudioManager.Instance.StopEnvironmentSounds();
    }
    
    IEnumerator ReturnToMainMenuAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); 
        Time.timeScale = 1; 
        GoToMainMenu(); 
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

    private void UpdateBoatParticles(GameObject activeBoat)
    {
        if (activeBoat == null) return;

        Rigidbody boatRb = activeBoat.GetComponent<Rigidbody>();

        if (boatRb != null && boatRb.velocity.magnitude > 0.5)
        {
            // Correct spawn position behind the boat
            Vector3 boatBack = activeBoat.transform.position - activeBoat.transform.forward * 20f + Vector3.up * 5f;

            // Correct rotation: Look backward with the Z-axis
            Quaternion particleRotation = Quaternion.LookRotation(-activeBoat.transform.forward, Vector3.up);

            // Spawn the wind particles
            spawnedWind = ParticlePoolManager.Instance.SpawnParticle("WindParticles", boatBack, particleRotation);

            // Correct position for the wake particles
            Vector3 wakePosition = activeBoat.transform.position - activeBoat.transform.forward * 1f + Vector3.down * 0.5f;

            // Spawn the wake particles (set rotation to identity for water trail)
            spawnedWake = ParticlePoolManager.Instance.SpawnParticle("BoatWakeParticles", wakePosition, Quaternion.identity);
        }
        else
        {
            if (spawnedWind != null) ParticlePoolManager.Instance.RemoveParticle(spawnedWind);
            if (spawnedWake != null) ParticlePoolManager.Instance.RemoveParticle(spawnedWake);
        }
    }
}


