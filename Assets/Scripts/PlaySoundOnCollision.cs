using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip impactSound;
    [SerializeField] private float minVelocity = 0.2f; // Minimum velocity to trigger sound
    [SerializeField] private float volumeScale = 1.0f; // Volume scaling factor
    [SerializeField] private float minPitch = 0.9f; // Minimum pitch variation
    [SerializeField] private float maxPitch = 1.1f; // Maximum pitch variation

    private void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce >= minVelocity)
        {
            // Set a random pitch within the defined range
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(impactSound, Mathf.Clamp01(impactForce * volumeScale));
        }
    }
}