using UnityEngine;

public class BoatController : MonoBehaviour
{
    public GameLoopManager gameLoopManager; // Référence au GameLoopManager
    public int playerNumber; // 1 pour le joueur 1, 2 pour le joueur 2

    private int rotationSpeed=100;
    public float forceTarget = 50f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Debug.Log($"Joueur {playerNumber} a collecté une pièce !");
            gameLoopManager.CoinCollected(playerNumber, other.gameObject);
        }
    }

    public void RotateTowardsPoint(Vector3 targetPoint)
    {
        // Get direction to the point
        Vector3 direction = (targetPoint - transform.position);

        // Ignore the vertical component
        direction.y = 0;
        direction = direction.normalized;

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate toward the coin
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        calculateForceToTarget(targetPoint);
     
    }

    //Call with slider in GameLoop
    public bool MoveTowardsTarget(float currentForce)
    {
        Debug.Log($"Force target for Ai:{forceTarget}");
        if (forceTarget <= currentForce)
        {
            return true;
        }
        return false;

    }

    private void calculateForceToTarget(Vector3 targetPoint)
    {
        float maxDistance = 50f; // Distance maximale pour appliquer la force maximale
        float maxForce = 34.5f;  // Force maximale que le bateau peut appliquer
        float minForce = 15f;     // Force minimale pour garantir un mouvement minimum

        // Calculer la distance au point cible
        float distance = Vector3.Distance(transform.position, targetPoint);

        // Calculer la magnitude de la force en fonction de la distance
        float forceMagnitude = Mathf.Min((distance / maxDistance) * maxForce, maxForce);

        // S'assurer que la force est au moins égale à la force minimale
        forceMagnitude = Mathf.Max(forceMagnitude, minForce);

        Debug.Log($"Distance au point : {distance}, Force calculée : {forceMagnitude}");

        forceTarget = forceMagnitude; // Mettre à jour la force cible
    }
}