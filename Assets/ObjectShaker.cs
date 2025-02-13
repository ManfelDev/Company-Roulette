using UnityEngine;
using System.Collections;

public class ObjectShaker : MonoBehaviour
{
    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;


    private IEnumerator Start()
    {

        yield return new WaitForSeconds(3f);
        //StartShake(0.75f, 0.03f);
    }

    public void StartShake(float duration, float intensity)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(Shake(duration, intensity));
    }

    private IEnumerator Shake(float duration, float intensity)
    {
        originalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
