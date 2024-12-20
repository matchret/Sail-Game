using UnityEngine;

public class BoatParticleController : MonoBehaviour
{
    public Transform boatTransform;

    private void Update()
    {
        // Si le bateau se d�place, g�n�rez des particules
        if (boatTransform != null && boatTransform.GetComponent<Rigidbody>().velocity.magnitude > 1)
        {
            Vector3 boatBack = boatTransform.position - boatTransform.forward * 2f;

            // Effets de vent
            ParticlePoolManager.Instance.SpawnParticle("WindParticles", boatBack, boatTransform.rotation);

            // Sillage du bateau
            Vector3 wakePosition = boatTransform.position - boatTransform.forward * 1f + Vector3.down * 0.5f;
            ParticlePoolManager.Instance.SpawnParticle("BoatWakeParticles", wakePosition, Quaternion.identity);
        }
    }
}
