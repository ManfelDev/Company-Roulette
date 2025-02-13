using UnityEngine;
using System.Collections;

public class SpotlightConeCheck : MonoBehaviour
{
    [Header("Spotlight Settings")]
    public Light spotLight; // Assign your Spotlight in Inspector
    public float checkRadius = 5f; // Defines the max detection range

    [Header("Material Settings")]
    public SkinnedMeshRenderer skinnedMeshRenderer; // Assign the Skinned Mesh Renderer
    public Material transparentMaterial; // Transparent version of the material

    private Material originalMaterial3; // Store the original Material 3
    private Material originalMaterial4; // Store the original Material 4

    private bool isDetected = false;
    private bool isDetectionActive = false;
    private bool isMaterialTransparent = false; // Track if Materials 3 & 4 are transparent

    private void Start()
    {
        if (skinnedMeshRenderer == null)
        {
            Debug.LogError("SkinnedMeshRenderer is not assigned in the Inspector!");
            return;
        }

        if (skinnedMeshRenderer.materials.Length < 5)
        {
            Debug.LogError("SkinnedMeshRenderer does not have at least 5 materials at runtime! It has " + skinnedMeshRenderer.materials.Length);
            return;
        }

        // Save the original Material 3 & 4
        originalMaterial3 = skinnedMeshRenderer.materials[3];
        originalMaterial4 = skinnedMeshRenderer.materials[4];

        if (originalMaterial3 == null || originalMaterial4 == null)
        {
            Debug.LogError("Original Materials (Element 3 or 4) are null! Check the SkinnedMeshRenderer material list.");
        }
    }

    private void FixedUpdate()
    {
        if (!isDetectionActive || spotLight == null || skinnedMeshRenderer == null || originalMaterial3 == null || originalMaterial4 == null) return;

        float spotAngle = spotLight.spotAngle * 0.5f; // Convert full angle to half for calculations
        float spotRange = spotLight.range;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, spotRange); // Detect objects in range
        isDetected = false;

        foreach (Collider col in hitColliders)
        {
            Vector3 directionToTarget = (col.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            // Check if within the cone angle and in range
            if (angleToTarget < spotAngle && col.CompareTag("Pistol"))
            {
                isDetected = true;
                break;
            }
        }

        // Change Material 3 & 4 only when necessary
        if (isDetected && !isMaterialTransparent)
        {
            SetMaterialsTransparent();
        }
        else if (!isDetected && isMaterialTransparent)
        {
            SetMaterialsOpaque();
        }

        AdjustAlpha();
    }

    private void AdjustAlpha()
    {
        if (skinnedMeshRenderer != null && skinnedMeshRenderer.materials.Length >= 5)
        {
            Material thirdMaterial = skinnedMeshRenderer.materials[3];
            Material fourthMaterial = skinnedMeshRenderer.materials[4];

            if (thirdMaterial != null && fourthMaterial != null)
            {
                float targetAlpha = isDetected ? 0.5f : 1.0f;
                Color color3 = thirdMaterial.color;
                Color color4 = fourthMaterial.color;
                color3.a = targetAlpha;
                color4.a = targetAlpha;
                thirdMaterial.color = color3;
                fourthMaterial.color = color4;
            }
        }
    }

    // Enable detection and allow material swap
    public void EnableDetection()
    {
        isDetectionActive = true;
        Debug.Log("Spotlight Detection ENABLED");
    }

    // Disable detection and reset materials
    public void DisableDetection()
    {
        isDetectionActive = false;
        isDetected = false;
        SetMaterialsOpaque(); // Reset materials when detection is disabled
        Debug.Log("Spotlight Detection DISABLED");
    }

    private void SetMaterialsTransparent()
    {
        if (skinnedMeshRenderer != null && skinnedMeshRenderer.materials.Length >= 5 && transparentMaterial != null)
        {
            Material[] materials = skinnedMeshRenderer.materials;
            materials[3] = transparentMaterial; // Replace Material 3
            materials[4] = transparentMaterial; // Replace Material 4
            skinnedMeshRenderer.materials = materials;

            isMaterialTransparent = true;
        }
    }

    private void SetMaterialsOpaque()
    {
        if (skinnedMeshRenderer != null && skinnedMeshRenderer.materials.Length >= 5 && originalMaterial3 != null && originalMaterial4 != null)
        {
            Material[] materials = skinnedMeshRenderer.materials;
            materials[3] = originalMaterial3; // Restore Material 3
            materials[4] = originalMaterial4; // Restore Material 4
            skinnedMeshRenderer.materials = materials;

            isMaterialTransparent = false;
        }
    }
}