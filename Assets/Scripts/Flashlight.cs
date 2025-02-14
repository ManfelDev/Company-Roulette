using UnityEngine;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticsUtility;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private GameObject lights;
    [SerializeField] private AudioSource flashlightSwitchSound;

    private GameManager gameManager;
    private HandDetection handDetection;
    private GunShot gunShot;

    private void Start()
    {
        handDetection = GetComponent<HandDetection>();
        gunShot = FindAnyObjectByType<GunShot>();

        gameManager = FindAnyObjectByType<GameManager>();
    }

    public void TurnOnLights()
    {
        lights.SetActive(true);
        SendHapticFeedback(0.4f, 0.1f, 0.4f);
        
        if (gunShot.HasBulletInChamber())
        {
            gunShot.SetBulletInChamberActive(true);
        }

        gameManager.flashlightUsed = true;

        flashlightSwitchSound.Play();
    }

    public void TurnOffLights()
    {
        lights.SetActive(false);
        SendHapticFeedback(0.3f, 0.05f, 0.3f);

        if (gunShot.HasBulletInChamber())
        {
            gunShot.SetBulletInChamberActive(false);
        }

        flashlightSwitchSound.Play();
    }

    private void SendHapticFeedback(float hapticAmplitude, float hapticDuration, float hapticFrequency)
    {
        if (handDetection.IsRightHand())
        {
            SendHapticImpulse(hapticAmplitude, hapticDuration, Controller.Right, hapticFrequency);
        }
        else if (handDetection.IsLeftHand())
        {
            SendHapticImpulse(hapticAmplitude, hapticDuration, Controller.Left, hapticFrequency);
        }
    }
}