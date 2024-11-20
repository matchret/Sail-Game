using UnityEngine;
using TMPro;

public class ScoreDisplayManager : MonoBehaviour
{
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;

    public GameLoopManager gameLoopManager; // Référence au GameLoopManager

    void Update()
    {
        // Récupérer les scores depuis le GameLoopManager
        player1ScoreText.text = $"Joueur 1 : {gameLoopManager.GetPlayerScore(1)}";
        player2ScoreText.text = $"Joueur 2 : {gameLoopManager.GetPlayerScore(2)}";
    }
}