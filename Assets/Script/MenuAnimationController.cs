using UnityEngine;

public class MenuAnimationController : MonoBehaviour
{
    public UIAnimator uiAnimator;
    public RectTransform playButtonRect;
    public RectTransform optionsButtonRect;

    

    void Start()
    {
        // Le bouton arrive depuis la gauche sur une durée de 0.5s avec un offset de 500 pixels
        StartCoroutine(uiAnimator.AnimateInFromLeft(playButtonRect,.5f, 500f));
    }

    public void OnPlayButtonClicked()
    {
        // Quand on clique sur Play, on fait disparaitre le panel vers le haut
        //StartCoroutine(uiAnimator.AnimateOutUp(0.5f, 300f));
    }

    public void OnButtonHover()
    {
        // Agrandir le bouton sur 0.3s quand on le survole
       // StartCoroutine(uiAnimator.AnimateScale(0.3f, Vector3.one, new Vector3(1.2f,1.2f,1f)));
    }

    public void OnButtonHoverExit()
    {
        // Revenir à la taille normale
        //StartCoroutine(playButtonAnimator.AnimateScale(0.3f, new Vector3(1.2f,1.2f,1f), Vector3.one));
    }
}