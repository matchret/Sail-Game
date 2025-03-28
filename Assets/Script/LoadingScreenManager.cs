using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class LoadingScreenManager : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Image LoadingBarFill;

    public TMP_InputField player1NameInput; // Input Field for Player 1 Name
    public TMP_InputField player2NameInput; // Input Field for Player 2 Name

    public TMP_Dropdown player1TypeDropdown; // Dropdown for Player 1 Type
    public TMP_Dropdown player2TypeDropdown; // Dropdown for Player 2 Type
    
    public TMP_Dropdown MouseDropdown;

    public void LoadScene(int sceneId)
    {
        SetPlayerData();

        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        LoadingScreen.SetActive(true);

        while (operation != null)
        {
            float progressValue = Mathf.Clamp01(operation.progress/ 0.9f);

            LoadingBarFill.fillAmount = progressValue;
           
            yield return null;
        }
    }

    private void SetPlayerData()
    {
        string Player1Name;
        string Player2Name;
        string Player1Type;
        string Player2Type;
        bool Player1Mouse;
        bool Player2Mouse;

       
        Player1Name = player1NameInput.text;
        Player2Name = player2NameInput.text;

        // Read selected options from Dropdowns
        Player1Type = player1TypeDropdown.options[player1TypeDropdown.value].text;
        Player2Type = player2TypeDropdown.options[player2TypeDropdown.value].text;

        if (MouseDropdown.value == 0)
        {
            Debug.Log($"dropdown 0");
            Player1Mouse = false;
            Player2Mouse = true;
        }
        else if(MouseDropdown.value == 1)
        {
            Debug.Log($"dropdown 1");
            Player1Mouse = true;
            Player2Mouse = false;
        }
        else
        {
            Debug.Log($"dropdown 2");
            Player1Mouse = false;
            Player2Mouse = false;
        }


        // Optional: Pass these values to a global/static GameData class
        GameData.Player1Name = Player1Name;
        GameData.Player2Name = Player2Name;
        GameData.Player1Type = Player1Type;
        GameData.Player2Type = Player2Type;
        GameData.Player1Mouse = Player1Mouse;
        GameData.Player2Mouse = Player2Mouse;
    }
}
