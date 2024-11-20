using UnityEngine;

public class BoatController : MonoBehaviour
{
    public GameLoopManager gameLoopManager; // Référence au GameLoopManager
    public int playerNumber; // 1 pour le joueur 1, 2 pour le joueur 2

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Debug.Log($"Joueur {playerNumber} a collecté une pièce !");
            gameLoopManager.CoinCollected(playerNumber, other.gameObject);
        }
    }
}