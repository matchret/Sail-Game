using UnityEngine;
using UnityEngine.UI;
using System.Collections; 

public class ControlCustomizationMenu : MonoBehaviour
{
    public GameObject customizationPanel; // Le menu de personnalisation
    public Button returnButton;
    public Button resetButton;
    public Transform controlListContainer; // Conteneur pour les contrôles
    public GameObject controlItemPrefab; // Préfabriqué pour un élément de la liste des contrôles
    public GameObject pauseMenu; // Référence au menu de pause
    public GameObject controlCustomizationMenu; // Référence à ce menu

    void Start()
    {
        returnButton.onClick.AddListener(CloseMenu);
        resetButton.onClick.AddListener(ResetToDefault);

        // Générer la liste des contrôles
        foreach (var action in System.Enum.GetValues(typeof(ControlManager.Action)))
        {
            var item = Instantiate(controlItemPrefab, controlListContainer);
            var actionName = item.transform.Find("ActionName").GetComponent<Text>();
            var keyButton = item.transform.Find("KeyButton").GetComponent<Button>();
            actionName.text = action.ToString();
            keyButton.GetComponentInChildren<Text>().text = ControlManager.Instance.GetKeyBinding((ControlManager.Action)action).ToString();

            keyButton.onClick.AddListener(() => StartRebinding((ControlManager.Action)action, keyButton));
        }
    }
    
    public void BackToPauseMenu()
    {
        controlCustomizationMenu.SetActive(false); // Désactiver ce menu
        pauseMenu.SetActive(true); // Réactiver le menu de pause
    }

    void StartRebinding(ControlManager.Action action, Button keyButton)
    {
        StartCoroutine(WaitForKeyPress(action, keyButton));
    }

    IEnumerator WaitForKeyPress(ControlManager.Action action, Button keyButton)
    {
        keyButton.GetComponentInChildren<Text>().text = "Press any key...";
        while (!Input.anyKeyDown) yield return null;

        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                ControlManager.Instance.SetKeyBinding(action, key);
                keyButton.GetComponentInChildren<Text>().text = key.ToString();
                break;
            }
        }
    }

    void ResetToDefault()
    {
        ControlManager.Instance.LoadBindings(); // Recharge les bindings par défaut
    }

    void CloseMenu()
    {
        customizationPanel.SetActive(false);
        Time.timeScale = 1; // Reprendre le jeu
    }
}
