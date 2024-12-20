using UnityEngine;
using System.Collections;

public class UIAnimator : MonoBehaviour
{
    public AudioSource sfxSource; 
    public ParticleSystem vfx; 

    // Animation d'arrivée depuis la gauche
    public IEnumerator AnimateInFromLeft(RectTransform rect, float duration, float startOffset)
    {
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

        // Jouer un SFX
        if (sfxSource != null)
        {
            sfxSource.Play();
        }

        // Jouer un VFX (particules)
        if (vfx != null)
        {
            vfx.Play();
        }
    }

    // Animation de disparition vers le haut
    public IEnumerator AnimateOutUp(RectTransform rect, float duration, float endOffset)
    {
        Vector2 initialPos = rect.anchoredPosition;
        Vector2 endPos = initialPos + new Vector2(0, endOffset);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed/duration);
            float easedT = EasingFunctions.EaseInOutQuad(t);

            rect.anchoredPosition = Vector2.Lerp(initialPos, endPos, easedT);
            yield return null;
        }
        rect.anchoredPosition = endPos;
    }

    // Animation de scale (par exemple au survol)
    public IEnumerator AnimateScale(RectTransform rect, float duration, Vector3 startScale, Vector3 endScale)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed/duration);
            float easedT = EasingFunctions.EaseInOutQuad(t); // EaseInOutQuad pour un scale doux

            rect.localScale = Vector3.Lerp(startScale, endScale, easedT);
            yield return null;
        }
        rect.localScale = endScale;
    }
}
