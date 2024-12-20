using UnityEngine;
using System.Collections;

public class UIAnimator : MonoBehaviour
{

    public bool isAnimating = false;
    // Animation d'arrivée depuis la gauche
    public IEnumerator AnimateInFromLeft(RectTransform rect, float duration, float startOffset)
    {
        Debug.Log("animateleft");
        Vector2 initialPos = rect.anchoredPosition;
        Vector2 startPos = initialPos + new Vector2(-startOffset, 0);
        rect.anchoredPosition = startPos;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed/duration);
            // Utilisons EaseOutBounce en exemple
            float easedT = EasingFunctions.EaseOutBounce(t);

            rect.anchoredPosition = Vector2.Lerp(startPos, initialPos, easedT);
            yield return null;
        }
        rect.anchoredPosition = initialPos;
        
    }
    

    // Animation de scale (par exemple au survol)
    public IEnumerator AnimateScale(RectTransform rect, float duration, Vector3 startScale, Vector3 endScale)
    {
        if (!isAnimating)
        {
            Debug.Log("animatescale");
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float easedT = EasingFunctions.EaseInOutQuad(t); // EaseInOutQuad pour un scale doux

                rect.localScale = Vector3.Lerp(startScale, endScale, easedT);
                yield return null;
            }

            rect.localScale = endScale;
        }
    }
    
    public IEnumerator AnimateOutRight(RectTransform rect, float duration, float endOffset)
    {
        isAnimating = true;
        Debug.Log("animateright");
        Vector2 initialPos = rect.anchoredPosition;
        Vector2 endPos = initialPos + new Vector2(endOffset, 0); // endOffset pixel vers la droite

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed/duration);
            // Utiliser un easing, par exemple EaseInOutQuad
            float easedT = EasingFunctions.EaseInOutQuad(t);

            rect.anchoredPosition = Vector2.Lerp(initialPos, endPos, easedT);
            yield return null;
        }
        rect.anchoredPosition = endPos;
        isAnimating = false;
    }
}
