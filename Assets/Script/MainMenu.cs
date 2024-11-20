using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject welcomeScreen; // Référence à l'écran "Appuyez sur une touche"
    public GameObject Title;
    public GameObject mainMenu;      // Référence au menu principal

    private bool hasKeyPressed = false;

    void Start()
    {
        // Assurez-vous que seul l'écran de bienvenue est actif au début
        welcomeScreen.SetActive(true);
        Title.SetActive(false);
        mainMenu.SetActive(false);
    }

    void Update()
    {
        // Détecter si une touche est pressée
        if (!hasKeyPressed && Input.anyKeyDown)
        {
            Debug.Log("touche pesser");
            hasKeyPressed = true;
            ShowMainMenu();
        }
    }

    void ShowMainMenu()
    {
        // Désactiver l'écran de bienvenue et activer le menu principal
        welcomeScreen.SetActive(false);
        Title.SetActive(true);
        mainMenu.SetActive(true);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
