using UnityEngine;
using System.Collections;

public class SpotlightConeCheck : MonoBehaviour
{
    [Header("Spotlight Settings")]
    public Light spotLight; // Assign your Spotlight in Inspector
    public float checkRadius = 5f; // Defines the max detection range

    [Header("Material Settings")]
    public Renderer targetRenderer; // The Renderer of the object
    public Material opaqueMaterial; // The opaque material
    public Material transparentMaterial; // The transparent material
    public float minAlpha = 0.5f; // Minimum alpha when detected
    public float maxAlpha = 1.0f; // Maximum alpha when not detected
    public float alphaDecreaseSpeed = 0.5f; // Speed of alpha decrease
    public float alphaIncreaseSpeed = 0.5f; // Speed of alpha increase

    private bool isDetected = false;
    private bool isDetectionActive = false;
    private bool isUsingTransparentMaterial = false;
    private Coroutine alphaCoroutine;

    private void FixedUpdate()
    {
        if (!isDetectionActive || spotLight == null || targetRenderer == null) return;

        float spotAngle = spotLight.spotAngle * 0.5f; // Convert full angle to half for calculations
        float spotRange = spotLight.range;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, spotRange); // Detect objects in range
        isDetected = false;

        foreach (Collider col in hitColliders)
        {
            Vector3 directionToTarget = (col.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            // Check if within the cone angle and range
            if (angleToTarget < spotAngle && col.CompareTag("Pistol"))
            {
                isDetected = true;
                break;
            }
        }

        HandleMaterialSwitch();
        AdjustAlpha();
    }

    private void HandleMaterialSwitch()
    {
        if (isDetected && !isUsingTransparentMaterial)
        {
            targetRenderer.material = transparentMaterial;
            isUsingTransparentMaterial = true;
        }
        else if (!isDetected && isUsingTransparentMaterial)
        {
            targetRenderer.material = opaqueMaterial;
            isUsingTransparentMaterial = false;
        }
    }

    private void AdjustAlpha()
    {
        if (targetRenderer != null)
        {
            float targetAlpha = isDetected ? minAlpha : maxAlpha;
            float speed = isDetected ? alphaDecreaseSpeed : alphaIncreaseSpeed;

            // Stop any ongoing alpha transition before starting a new one
            if (alphaCoroutine != null)
            {
                StopCoroutine(alphaCoroutine);
            }

            alphaCoroutine = StartCoroutine(LerpAlpha(targetAlpha, speed));
        }
    }

    private IEnumerator LerpAlpha(float targetAlpha, float speed)
    {
        Material currentMaterial = targetRenderer.material;
        Color color = currentMaterial.color;

        while (!Mathf.Approximately(color.a, targetAlpha))
        {
            color.a = Mathf.MoveTowards(color.a, targetAlpha, speed * Time.deltaTime);
            currentMaterial.color = color;
            yield return null; // Wait for the next frame
        }
    }

    // Method to enable detection
    public void EnableDetection()
    {
        isDetectionActive = true;
        Debug.Log("Spotlight Detection ENABLED");
    }

    // Method to disable detection
    public void DisableDetection()
    {
        isDetectionActive = false;
        isDetected = false;
        HandleMaterialSwitch();
        AdjustAlpha();
        Debug.Log("Spotlight Detection DISABLED");
    }
}