using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ControlCustomizationMenu : MonoBehaviour
{
    public GameObject customizationPanel; // Le menu de personnalisation
    public GameObject pauseMenu; // Référence au menu de pause

    // Boutons pour chaque action
    public Button moveLeftButton;
    public Button moveRightButton;
    public Button dashButton;
    public Button pauseButton;

    // Boutons pour navigation
    public Button returnButton; // Bouton pour revenir au menu pause
    public Button resetButton; // Bouton pour réinitialiser les contrôles

    // Textes des boutons pour afficher les touches actuelles
    public TextMeshProUGUI moveLeftText;
    public TextMeshProUGUI moveRightText;
    public TextMeshProUGUI dashText;
    public TextMeshProUGUI pauseText;
    
    public Button moveLeftButtonP2;
    public Button moveRightButtonP2;
    public Button dashButtonP2;
    public Button pauseButtonP2;

    public TextMeshProUGUI moveLeftTextP2;
    public TextMeshProUGUI moveRightTextP2;
    public TextMeshProUGUI dashTextP2;
    public TextMeshProUGUI pauseTextP2;

    private ControlManager.Action currentAction; // L'action en cours de modification
    private Button currentButton; // Le bouton en attente d'une touche

    void Start()
    {
        // Assignez les actions des boutons
        moveLeftButton.onClick.AddListener(() => StartRebinding(ControlManager.Action.MoveLeft, moveLeftText));
        moveRightButton.onClick.AddListener(() => StartRebinding(ControlManager.Action.MoveRight, moveRightText));
        dashButton.onClick.AddListener(() => StartRebinding(ControlManager.Action.Dash, dashText));
        pauseButton.onClick.AddListener(() => StartRebinding(ControlManager.Action.Pause, pauseText));
        
        moveLeftButtonP2.onClick.AddListener(() => StartRebinding(ControlManager.Action.MoveLeftP2, moveLeftTextP2));
        moveRightButtonP2.onClick.AddListener(() => StartRebinding(ControlManager.Action.MoveRightP2, moveRightTextP2));
        dashButtonP2.onClick.AddListener(() => StartRebinding(ControlManager.Action.DashP2, dashTextP2));
        pauseButtonP2.onClick.AddListener(() => StartRebinding(ControlManager.Action.PauseP2, pauseTextP2));


        // Boutons pour navigation
        returnButton.onClick.AddListener(BackToPauseMenu);
        resetButton.onClick.AddListener(ResetToDefault);

        // Initialiser les textes des boutons
        UpdateButtonLabels();
    }

    public void StartRebinding(ControlManager.Action action, TextMeshProUGUI buttonText)
    {
        currentAction = action;
        currentButton = null; // Pas nécessaire ici, car nous utilisons `buttonText` directement
        buttonText.text = "Press any key..."; // Affiche "Press any key..." pendant la réassignation
        StartCoroutine(WaitForKeyPress(buttonText));
    }

    IEnumerator WaitForKeyPress(TextMeshProUGUI buttonText)
    {
        while (!Input.anyKeyDown) // Attendre qu'une touche soit pressée
        {
            yield return null;
        }

        KeyCode pressedKey = KeyCode.None;
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                pressedKey = key;
                break;
            }
        }

        if (pressedKey != KeyCode.None)
        {
            // Vérifiez si la touche est déjà assignée
            if (ControlManager.Instance.IsKeyAlreadyAssigned(pressedKey))
            {
                buttonText.text = "Already Taken"; // Affiche "Already Taken" si la touche est déjà utilisée
                yield return new WaitForSeconds(1f);
                UpdateButtonLabels(); // Réinitialise l'affichage des touches
            }
            else
            {
                // Assigner la nouvelle touche
                ControlManager.Instance.SetKeyBinding(currentAction, pressedKey);
                buttonText.text = pressedKey.ToString(); // Met à jour le texte avec la nouvelle touche
            }
        }
    }

    public void UpdateButtonLabels()
    {
        moveLeftText.text = ControlManager.Instance.GetKeyBinding(ControlManager.Action.MoveLeft).ToString();
        moveRightText.text = ControlManager.Instance.GetKeyBinding(ControlManager.Action.MoveRight).ToString();
        dashText.text = ControlManager.Instance.GetKeyBinding(ControlManager.Action.Dash).ToString();
        pauseText.text = ControlManager.Instance.GetKeyBinding(ControlManager.Action.Pause).ToString();
        
        moveLeftTextP2.text = ControlManager.Instance.GetKeyBinding(ControlManager.Action.MoveLeftP2).ToString();
        moveRightTextP2.text = ControlManager.Instance.GetKeyBinding(ControlManager.Action.MoveRightP2).ToString();
        dashTextP2.text = ControlManager.Instance.GetKeyBinding(ControlManager.Action.DashP2).ToString();
        pauseTextP2.text = ControlManager.Instance.GetKeyBinding(ControlManager.Action.PauseP2).ToString();
    }

    public void BackToPauseMenu()
    {
        customizationPanel.SetActive(false); // Désactiver ce menu
        pauseMenu.SetActive(true); // Réactiver le menu de pause
    }

    public void ResetToDefault()
    {
        ControlManager.Instance.ResetToDefaultBindings(); // Réinitialiser les bindings aux valeurs par défaut
        UpdateButtonLabels(); // Met à jour tous les textes des boutons
    }
}

