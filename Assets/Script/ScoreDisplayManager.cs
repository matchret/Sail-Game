using UnityEngine;
using TMPro; // Nécessaire pour TextMeshPro

public class ScoreDisplayManager : MonoBehaviour
{
    public TextMeshProUGUI player1ScoreText; // Zone de texte pour le score du joueur 1
    public TextMeshProUGUI player2ScoreText; // Zone de texte pour le score du joueur 2

    public GameObject player1Boat; // Bateau du joueur 1
    public GameObject player2Boat; // Bateau du joueur 2

    private BoatController player1Controller;
    private BoatController player2Controller;

    void Start()
    {
        // Obtenir les contrôleurs des bateaux
        player1Controller = player1Boat.GetComponent<BoatController>();
        player2Controller = player2Boat.GetComponent<BoatController>();

        // Initialiser les scores
        UpdateScoreDisplay();
    }

    void Update()
    {
        // Mettre à jour les scores à chaque frame
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        // Mettre à jour les zones de texte avec les scores
        player1ScoreText.text = $"Joueur 1 : {player1Controller.playerScore}";
        player2ScoreText.text = $"Joueur 2 : {player2Controller.playerScore}";
    }
}