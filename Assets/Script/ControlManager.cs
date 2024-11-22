using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public static ControlManager Instance;

    public float sensitivityDefault = 2.5f;
    public float sensitivityP1 = 2.5f;
    public float sensitivityP2 = 2.5f;

    // Définition des actions
    public enum Action
    {
        MoveLeft,
        MoveRight,
        Dash,
        Pause,
        MoveLeftP2,
        MoveRightP2,
        DashP2,
        PauseP2
    }

    // Configuration par défaut des touches
    private Dictionary<Action, KeyCode> defaultKeyBindings = new Dictionary<Action, KeyCode>
    {
        { Action.MoveLeft, KeyCode.A },
        { Action.MoveRight, KeyCode.D },
        { Action.Dash, KeyCode.Space },
        { Action.Pause, KeyCode.P },
        
        { Action.MoveLeftP2, KeyCode.LeftArrow },
        { Action.MoveRightP2, KeyCode.RightArrow },
        { Action.DashP2, KeyCode.Return}, // Enter
        { Action.PauseP2, KeyCode.Escape }
    };

    // Bindings personnalisés par l'utilisateur
    private Dictionary<Action, KeyCode> userKeyBindings = new Dictionary<Action, KeyCode>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadBindings();
        }
        else
        {
            //Destroy(gameObject);
        }
    }
    
    
    public void ResetState()
    {
        userKeyBindings = new Dictionary<Action, KeyCode>(defaultKeyBindings); // Réinitialise les bindings
        SaveBindings(); // Sauvegarde les valeurs par défaut
    }

    // Vérifier si une action est déclenchée
    public bool IsActionPressed(Action action)
    {
        if (userKeyBindings.TryGetValue(action, out KeyCode key))
        {
            if (action == Action.MoveLeft || action == Action.MoveRight || action == Action.MoveLeftP2 || action == Action.MoveRightP2)
            {
                return Input.GetKey(key);
            }
            else
            {
                return Input.GetKeyDown(key);
            }
          
        }
        return false;
    }

    // Réassigner une touche
    public void SetKeyBinding(Action action, KeyCode newKey)
    {
        userKeyBindings[action] = newKey;
        SaveBindings();
    }

    public void IsAPlayerMouse()
    {
        if (GameData.Player1Mouse == true)
        {
            LoadBindings();
            SetKeyBinding(Action.MoveLeft, KeyCode.Mouse0);
            SetKeyBinding(Action.MoveRight, KeyCode.Mouse1);
            SetKeyBinding(Action.Dash, KeyCode.Mouse2);
        }
        else if (GameData.Player2Mouse == true)
        {
            LoadBindings();
            SetKeyBinding(Action.MoveLeftP2, KeyCode.Mouse0);
            SetKeyBinding(Action.MoveRightP2, KeyCode.Mouse1);
            SetKeyBinding(Action.DashP2, KeyCode.Mouse2);
        }
        else
        {
            LoadBindings();
        }
    }

    // Récupérer la touche actuelle assignée à une action
    public KeyCode GetKeyBinding(Action action)
    {
        if (userKeyBindings.TryGetValue(action, out KeyCode key))
        {
            return key;
        }
        return KeyCode.None;
    }
    
    public bool IsKeyAlreadyAssigned(KeyCode key)
    {
        foreach (var binding in userKeyBindings.Values)
        {
            if (binding == key)
            {
                return true; // La touche est déjà assignée
            }
        }
        return false; // La touche est disponible
    }
    
    public void ResetToDefaultBindings()
    {
        
        userKeyBindings = new Dictionary<Action, KeyCode>(defaultKeyBindings);
        sensitivityP1 = sensitivityDefault;
        sensitivityP2 = sensitivityDefault;
        SaveBindings(); // Sauvegarder les valeurs par défaut
    }

    // Sauvegarder les bindings dans PlayerPrefs
    public void SaveBindings()
    {
        foreach (var binding in userKeyBindings)
        {

            if (binding.Value == KeyCode.Mouse0 || binding.Value == KeyCode.Mouse1 || binding.Value == KeyCode.Mouse2 || binding.Value == KeyCode.Mouse3)
            {
              //PlayerPrefs.SetString(binding.Key.ToString(), defaultKeyBindings.GetValueOrDefault(binding.Key).ToString());  
            }
            else
            {
                PlayerPrefs.SetString(binding.Key.ToString(), binding.Value.ToString());  
            }
            
        }
        PlayerPrefs.SetString("sensP1", sensitivityP1.ToString());
        PlayerPrefs.SetString("sensP2", sensitivityP2.ToString());
        PlayerPrefs.Save();
    }
    
    public void LoadBindings()
    {
        foreach (var action in defaultKeyBindings.Keys)
        {
            string savedKey = PlayerPrefs.GetString(action.ToString(), defaultKeyBindings[action].ToString());
            if (System.Enum.TryParse(savedKey, out KeyCode key))
            {
                userKeyBindings[action] = key;
            }
            
            sensitivityP1 = float.Parse(PlayerPrefs.GetString("sensP1", sensitivityDefault.ToString()));
            sensitivityP2 = float.Parse(PlayerPrefs.GetString("sensP2", sensitivityDefault.ToString()));
            Debug.Log($"Sensibilité P1 : {sensitivityP1}, Sensibilité P2 : {sensitivityP2}");
        }
    }
}

