using UnityEngine;

public class BoatFloat : MonoBehaviour
{
    public float floatAmplitude = 0.5f; // Amplitude du mouvement vertical
    public float floatFrequency = 1f;  // Fréquence du mouvement vertical

    public float tiltAmplitude = 5f;   // Amplitude de l’inclinaison (en degrés)
    public float tiltFrequency = 1f;  // Fréquence de l’inclinaison

    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    void Start()
    {
        // Enregistrer la position et la rotation locales initiales
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
    }

    void Update()
    {
        // Mouvement vertical sinusoidal relatif à la position initiale
        float verticalOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Inclinaison du bateau relative à la rotation initiale
        float tiltOffset = Mathf.Sin(Time.time * tiltFrequency) * tiltAmplitude;

        // Appliquer les transformations locales sans accumulation
        transform.localPosition = initialLocalPosition + new Vector3(0, verticalOffset, 0);
        transform.localRotation = initialLocalRotation * Quaternion.Euler(tiltOffset, 0, -tiltOffset * 0.5f);
    }
}