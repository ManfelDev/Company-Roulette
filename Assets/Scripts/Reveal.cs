// Reveal.cs - Updates the shader with light position, direction, and angle
using UnityEngine;

[ExecuteInEditMode]
public class Reveal : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private Light spotLight;

    private void Update()
    {
        if (mat == null || spotLight == null)
            return;

        mat.SetVector("_LightPosition", spotLight.transform.position);
        mat.SetVector("_LightDirection", -spotLight.transform.forward);
        mat.SetFloat("_LightAngle", spotLight.spotAngle);
    }
}