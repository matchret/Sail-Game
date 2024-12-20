using System;
using UnityEngine;

public class MenuAnimationController : MonoBehaviour
{
    public UIAnimator uiAnimator;
    public RectTransform playButtonRect;
    public RectTransform optionsButtonRect;
    public RectTransform quitButtonRect;

    

    void Start()
    {
        //Debug.Log("MenuController Start() appelé");
        // Le bouton arrive depuis la gauche sur une durée de 0.5s avec un offset de 500 pixels
       // StartCoroutine(uiAnimator.AnimateInFromLeft(playButtonRect,.5f, 500f));
       // StartCoroutine(uiAnimator.AnimateInFromLeft(optionsButtonRect,.5f, 500f));
        //StartCoroutine(uiAnimator.AnimateInFromLeft(quitButtonRect,.5f, 500f));
    }
    
    void OnEnable()
    {
        Debug.Log("MenuController Start() appelé");
        // Le bouton arrive depuis la gauche sur une durée de 0.5s avec un offset de 500 pixels
        StartCoroutine(uiAnimator.AnimateInFromLeft(playButtonRect,.5f, 500f));
        StartCoroutine(uiAnimator.AnimateInFromLeft(optionsButtonRect,.5f, 500f));
        StartCoroutine(uiAnimator.AnimateInFromLeft(quitButtonRect,.5f, 500f));
    }
    




    public void OnPlayButtonClicked()
    {
        // Quand on clique sur Play, on fait disparaitre le panel vers le haut
        //StartCoroutine(uiAnimator.AnimateOutUp(playButtonRect,.5f, 300f));
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