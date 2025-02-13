using UnityEngine;

public class GlobalAudioSystem : MonoBehaviour
{
    public static GlobalAudioSystem Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        GameObject soundObject = new GameObject("TempAudio");
        MetaXRAudioSource metaAudioSource = soundObject.AddComponent<MetaXRAudioSource>();
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.spatialBlend = 1.0f; // Ensures 3D sound positioning
        soundObject.transform.position = position;
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.Play();
        Destroy(soundObject, clip.length);
    }
}
