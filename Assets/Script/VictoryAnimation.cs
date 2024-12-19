using System.Collections;
using UnityEngine;


public class BoatVictoryAnimation : MonoBehaviour
{
    public Transform boatVisual;       // Référence à l'enfant visuel
    public float jumpHeight = 5f;      // Hauteur du saut
    public float jumpDuration = 1f;    // Durée du saut
    public float rotationSpeed = 720f; // Vitesse de rotation en degrés par seconde

    private bool isAnimating = false;
    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    void Start()
    {
        // Assigner boatVisual si non défini
        if (boatVisual == null)
        {
            boatVisual = transform.Find("BoatVisual");
            if (boatVisual == null)
            {
                Debug.LogError("BoatVisual not found! Assign it in the inspector or verify hierarchy.");
            }
        }

        if (boatVisual != null)
        {
            initialLocalPosition = boatVisual.localPosition;
            initialLocalRotation = boatVisual.localRotation;
        }
    }

    public void PlayVictoryAnimation()
    {
        if (!isAnimating && boatVisual != null)
        {
            StartCoroutine(VictoryJumpAndSpin());
        }
    }

    private IEnumerator VictoryJumpAndSpin()
    {
        isAnimating = true;

        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;

            // Mouvement vertical: parabole basée sur sin(t * π)
            float height = Mathf.Sin(t * Mathf.PI) * jumpHeight;

            // Mettre à jour la position locale du visuel (pas la position globale)
            boatVisual.localPosition = initialLocalPosition + new Vector3(0, height, 0);

            // Rotation locale
            boatVisual.localRotation = initialLocalRotation * Quaternion.Euler(0, rotationSpeed * (elapsedTime / jumpDuration), 0);

            elapsedTime += Time.unscaledDeltaTime; // Utiliser unscaledDeltaTime pour ignorer Time.timeScale
            yield return null;
        }

        // Retour à l'état initial après le saut
        boatVisual.localPosition = initialLocalPosition;
        boatVisual.localRotation = initialLocalRotation;
        isAnimating = false;
    }
}
