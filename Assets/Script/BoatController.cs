using UnityEngine;

public class BoatController : MonoBehaviour
{
    public GameLoopManager gameLoopManager; // Référence au GameLoopManager
    public int playerNumber; // 1 pour le joueur 1, 2 pour le joueur 2

    private int rotationSpeed=100;
    public float forceTarget = 0;

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
    }

    public bool MoveTowardsTarget(float currentForce)
    {
        Debug.Log(currentForce);
        if (forceTarget < currentForce)
        {
            return true;
        }
        return false;

    }
}