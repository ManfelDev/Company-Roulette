using UnityEngine;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticsUtility;
using UnityEngine.Events;

public class GunShot : MonoBehaviour
{
    [SerializeField] private bool haveBulletInChamber;
    [SerializeField] private AudioSource gunshotSound;
    [SerializeField] private AudioSource noBulletSound;
    [SerializeField] private GunMag gunMag;
    [SerializeField] private GameObject bangFlag;

    [SerializeField] private UnityEvent doOnShoot;

    private Animator animator;
    private bool hasPlayedGunShotAnimation;
    private HandDetection handDetection;

    private void Start()
    {
        animator = GetComponent<Animator>();
        hasPlayedGunShotAnimation = true; // Reset animation flag
        handDetection = GetComponent<HandDetection>();
    }

    public void Shot()
    {
        if (haveBulletInChamber)
        {
            gunshotSound.Play();
            haveBulletInChamber = false;
            ActivateBangFlag();
            SendHapticFeedback(1f, 0.05f, 1f);
        }
        else
        {
            noBulletSound.Play();
            SendHapticFeedback(0.3f, 0.05f, 0.3f);
        }

        doOnShoot.Invoke();

        // Play the animation only on the first shot
        if (!hasPlayedGunShotAnimation)
        {
            animator.Play("GunShot", -1, 0f);
            hasPlayedGunShotAnimation = true; // Mark that the animation has been played
        }

        gunMag.MarkGunAsFired();
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

    public void SetHaveBulletInChamber(bool value)
    {
        haveBulletInChamber = value;
    }

    public bool HasBulletInChamber()
    {
        return haveBulletInChamber;
    }

    public void ResetGunShotAnimation()
    {
        hasPlayedGunShotAnimation = false;
    }

    public void ActivateBangFlag()
    {
        bangFlag.SetActive(true);
    }

    public void DeactivateBangFlag()
    {
        bangFlag.SetActive(false);
    }
}