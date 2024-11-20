using UnityEngine;

public class BoatController : MonoBehaviour
{
    public int playerScore = 0; // Score du joueur

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Debug.Log("Pièce collectée !");
            playerScore++; // Augmenter le score
            Destroy(other.gameObject); // Détruire la pièce
        }
    }
}