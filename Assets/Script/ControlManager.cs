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
        Pause
    }

    // Configuration par défaut des touches
    private Dictionary<Action, KeyCode> defaultKeyBindings = new Dictionary<Action, KeyCode>
    {
        { Action.MoveLeft, KeyCode.A },
        { Action.MoveRight, KeyCode.D },
        { Action.Dash, KeyCode.Space },
        { Action.Pause, KeyCode.P }
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
            if (action == Action.MoveLeft || action == Action.MoveRight)
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

    // Sauvegarder les bindings dans PlayerPrefs
    private void SaveBindings()
    {
        foreach (var binding in userKeyBindings)
        {
            PlayerPrefs.SetString(binding.Key.ToString(), binding.Value.ToString());
        }
        PlayerPrefs.Save();
    }

    // Charger les bindings depuis PlayerPrefs
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

