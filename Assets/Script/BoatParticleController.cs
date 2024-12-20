using UnityEngine;

public class BoatParticleController : MonoBehaviour
{
    public Transform boatTransform;
    GameObject spawnedWind;
    GameObject spawnedWake;

    private void Update()
    {
        if (boatTransform != null && boatTransform.GetComponent<Rigidbody>().velocity.magnitude > 0.5)
        {
            // Correct spawn position behind the boat
            Vector3 boatBack = boatTransform.position - boatTransform.forward * 20f + Vector3.up * 5f;

            // Correct rotation: Look backward with the Z-axis
            Quaternion particleRotation = Quaternion.LookRotation(-boatTransform.forward, Vector3.up);

            // Spawn the wind particles
            spawnedWind = ParticlePoolManager.Instance.SpawnParticle("WindParticles", boatBack, particleRotation);

            // Correct position for the wake particles
            Vector3 wakePosition = boatTransform.position - boatTransform.forward * 1f + Vector3.down * 0.5f;

            // Spawn the wake particles (set rotation to identity for water trail)
            spawnedWake = ParticlePoolManager.Instance.SpawnParticle("BoatWakeParticles", wakePosition, Quaternion.identity);
        }
        else
        {
            if (spawnedWind != null) ParticlePoolManager.Instance.RemoveParticle(spawnedWind);
            if (spawnedWake != null) ParticlePoolManager.Instance.RemoveParticle(spawnedWake);
        }
    }
}