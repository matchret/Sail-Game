using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject welcomeScreen; // R�f�rence � l'�cran "Appuyez sur une touche"
    public GameObject Title;
    public GameObject mainMenu;      // R�f�rence au menu principal

    private bool hasKeyPressed = false;

    void Start()
    {
        
        welcomeScreen.SetActive(true);
        Title.SetActive(false);
        mainMenu.SetActive(false);
        AudioManager.Instance.PlayMenuMusic();
    }

    void Update()
    {
        // D�tecter si une touche est press�e
        if (!hasKeyPressed && Input.anyKeyDown)
        {
            Debug.Log("touche pesser");
            hasKeyPressed = true;
            ShowMainMenu();
        }
    }

    void ShowMainMenu()
    {
        // D�sactiver l'�cran de bienvenue et activer le menu principal
        welcomeScreen.SetActive(false);
        Title.SetActive(true);
        mainMenu.SetActive(true);
    }
    public void PlayGame()
    {
        AudioManager.Instance.StopMenuMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
