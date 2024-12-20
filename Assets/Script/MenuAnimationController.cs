using System;
using System.Collections;
using UnityEngine;

public class MenuAnimationController : MonoBehaviour
{
    public UIAnimator uiAnimator;
    public RectTransform playButtonRect;
    public RectTransform optionsButtonRect;
    public RectTransform quitButtonRect;
    public GameObject localMenu;
    public GameObject mainMenu;
    private Vector2 initialPlayButtonPosition;
    private Vector2 initialOptionsButtonPosition;
    private Vector2 initialQuitButtonPosition;

    

    void Awake()
    {
        // Enregistrer la position initiale des boutons dans Awake ou Start
        initialPlayButtonPosition = playButtonRect.anchoredPosition;
        initialOptionsButtonPosition = optionsButtonRect.anchoredPosition;
        initialQuitButtonPosition = quitButtonRect.anchoredPosition;
    }
    
    void OnEnable()
    {
        Debug.Log($"OnEnable appel");
     
        
        playButtonRect.anchoredPosition = initialPlayButtonPosition;
        optionsButtonRect.anchoredPosition = initialOptionsButtonPosition;
        quitButtonRect.anchoredPosition = initialQuitButtonPosition;
        
        // Le bouton arrive depuis la gauche sur une durée de 0.5s avec un offset de 500 pixels
        StartCoroutine(uiAnimator.AnimateInFromLeft(playButtonRect,.5f, 500f));
        StartCoroutine(uiAnimator.AnimateInFromLeft(optionsButtonRect,.5f, 500f));
        StartCoroutine(uiAnimator.AnimateInFromLeft(quitButtonRect,.5f, 500f));
    }
    
    public void GoToAnotherMenu()
    {
        StartCoroutine(HideMenuButtons());
    }

    IEnumerator HideMenuButtons()
    {
      
        StartCoroutine(uiAnimator.AnimateOutRight(playButtonRect, .5f, 1400f));
        StartCoroutine(uiAnimator.AnimateOutRight(optionsButtonRect, .5f, 1400f));
        yield return StartCoroutine(uiAnimator.AnimateOutRight(quitButtonRect, .5f, 1400f));

        mainMenu.SetActive(false);
        localMenu.SetActive(true);

    }




    public void OnButtonHover()
    {
        // Agrandir le bouton sur 0.3s quand on le survole
        StartCoroutine(uiAnimator.AnimateScale(playButtonRect,.1f, Vector3.one, new Vector3(1.5f,1.5f,1f)));
    }

    public void OnButtonHoverExit()
    {
        // Revenir à la taille normale
        StartCoroutine(uiAnimator.AnimateScale(playButtonRect,.1f, new Vector3(1.5f,1.5f,1f), Vector3.one));
    }
    
    public void OnButtonOptionHover()
    {
        // Agrandir le bouton sur 0.3s quand on le survole
        StartCoroutine(uiAnimator.AnimateScale(optionsButtonRect,.1f, Vector3.one, new Vector3(1.5f,1.5f,1f)));
    }

    public void OnButtonHoverOptionExit()
    {
        // Revenir à la taille normale
        StartCoroutine(uiAnimator.AnimateScale(optionsButtonRect,.1f, new Vector3(1.5f,1.5f,1f), Vector3.one));
    }
    
    public void OnButtonQuitHover()
    {
        // Agrandir le bouton sur 0.3s quand on le survole
        StartCoroutine(uiAnimator.AnimateScale(quitButtonRect,.1f, Vector3.one, new Vector3(1.5f,1.5f,1f)));
    }

    public void OnButtonHoverQuitExit()
    {
        // Revenir à la taille normale
        StartCoroutine(uiAnimator.AnimateScale(quitButtonRect,.1f, new Vector3(1.5f,1.5f,1f), Vector3.one));
    }
}