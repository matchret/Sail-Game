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
        Debug.Log($"Initial Play Pos: {initialPlayButtonPosition}");
        Debug.Log($"Initial Options Pos: {initialOptionsButtonPosition}");
        Debug.Log($"Initial Quit Pos: {initialQuitButtonPosition}");

        
        playButtonRect.anchoredPosition = initialPlayButtonPosition;
        optionsButtonRect.anchoredPosition = initialOptionsButtonPosition;
        quitButtonRect.anchoredPosition = initialQuitButtonPosition;
        
        // Le bouton arrive depuis la gauche sur une durée de 0.5s avec un offset de 500 pixels
        StartCoroutine(uiAnimator.AnimateInFromLeft(playButtonRect,.5f, 500f));
        StartCoroutine(uiAnimator.AnimateInFromLeft(optionsButtonRect,.5f, 500f));
        StartCoroutine(uiAnimator.AnimateInFromLeft(quitButtonRect,.5f, 500f));
    }

    private void OnDisable()
    {
        //StartCoroutine(uiAnimator.AnimateOutRight(playButtonRect,.5f, 500f));
        //StartCoroutine(uiAnimator.AnimateOutRight(optionsButtonRect,.5f, 500f));
        //StartCoroutine(uiAnimator.AnimateOutRight(quitButtonRect,.5f, 500f));
    }


    public void OnPlayButtonClicked()
    {
        // Quand on clique sur Play, on fait disparaitre le panel vers le haut
        //StartCoroutine(uiAnimator.AnimateOutUp(playButtonRect,.5f, 300f));
    }

    public void GoToAnotherMenu()
    {
        Debug.Log("Appel à AnimateOutRight");
        // Faire partir les boutons vers la droite avant de désactiver le menu
        StartCoroutine(HideMenuButtons());
    }

    IEnumerator HideMenuButtons()
    {
        // Par exemple, les faire partir sur 0.5s, 500 pixels vers la droite
        StartCoroutine(uiAnimator.AnimateOutRight(playButtonRect, 0.5f, 2000f));
        StartCoroutine(uiAnimator.AnimateOutRight(optionsButtonRect, 0.5f, 2000f));
        yield return StartCoroutine(uiAnimator.AnimateOutRight(quitButtonRect, 0.5f, 2000f));
        yield return new WaitForSecondsRealtime(0.1f); 

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