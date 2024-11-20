using System.Collections;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public GameObject player1Boat;
    public GameObject player2Boat;
    public Camera mainCamera;
    public float rotationSpeed = 100f;
    public float shotForce = 10f;
    public int maxRounds = 10;
    public float cameraDistance = 5f;
    public float cameraHeight = 2f;

    private int currentPlayer = 1; // 1 pour joueur 1, 2 pour joueur 2
    private int player1Score = 0;
    private int player2Score = 0;
    private int currentRound = 1;
    private bool isWaitingForAction = true; // Le joueur peut tourner ou avancer

    void Start()
    {
        Debug.Log("Le jeu commence !");
        SetupBoats();
        StartPlayerTurn();
    }

    void Update()
    {
        if (isWaitingForAction)
        {
            HandlePlayerRotation();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HandlePlayerShot();
            }
        }

        UpdateCameraPosition();
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

    void HandlePlayerShot()
    {
        Debug.Log($"Joueur {currentPlayer} avance !");
        GameObject activeBoat = currentPlayer == 1 ? player1Boat : player2Boat;

        // Appliquer une force dans la direction actuelle du bateau
        Rigidbody rb = activeBoat.GetComponent<Rigidbody>();
        rb.AddForce(activeBoat.transform.forward * shotForce, ForceMode.Impulse);

        // Fin du tour après un délai
        StartCoroutine(EndTurnAfterDelay(2f));
        isWaitingForAction = false;
    }

    IEnumerator EndTurnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Passer au joueur suivant
        currentPlayer = currentPlayer == 1 ? 2 : 1;

        // Fin de la manche ou continuer
        if (currentPlayer == 1)
        {
            currentRound++;
            if (currentRound > maxRounds)
            {
                EndGame();
                yield break;
            }
        }

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

    void EndGame()
    {
        Debug.Log("Partie terminée !");
        Debug.Log($"Score Joueur 1 : {player1Score}");
        Debug.Log($"Score Joueur 2 : {player2Score}");

        if (player1Score > player2Score)
            Debug.Log("Joueur 1 gagne !");
        else if (player2Score > player1Score)
            Debug.Log("Joueur 2 gagne !");
        else
            Debug.Log("Match nul !");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Debug.Log($"Joueur {currentPlayer} a collecté une pièce !");
            
            // Ajouter au score du joueur actif
            if (currentPlayer == 1) player1Score++;
            else player2Score++;

            // Détruire la pièce
            Destroy(other.gameObject);
        }
    }
}
