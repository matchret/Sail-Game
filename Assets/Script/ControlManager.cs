using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public static ControlManager Instance;

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
        userKeyBindings = new Dictionary<Action, KeyCode>(defaultKeyBindings); // Copier les valeurs par défaut
        SaveBindings(); // Sauvegarder les valeurs par défaut
    }

    // Sauvegarder les bindings dans PlayerPrefs
    private void SaveBindings()
    {
        foreach (var binding in userKeyBindings)
        {
            PlayerPrefs.SetString(binding.Key.ToString(), binding.Value.ToString());
        }
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
        }
    }
}

