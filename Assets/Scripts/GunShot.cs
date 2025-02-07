using UnityEngine;

public class GunShot : MonoBehaviour
{
    [SerializeField] private bool haveBulletInChamber;
    [SerializeField] private AudioSource gunshotSound;
    [SerializeField] private AudioSource noBulletSound;
    [SerializeField] private GunMag gunMag;
    [SerializeField] private GameObject bangFlag;

    private Animator animator;
    private bool hasPlayedGunShotAnimation; // Track if animation has been played

    private void Start()
    {
        animator = GetComponent<Animator>();
        hasPlayedGunShotAnimation = true; // Reset animation flag
    }

    public void Shot()
    {
        if (haveBulletInChamber)
        {
            gunshotSound.Play();
            haveBulletInChamber = false;
            ActivateBangFlag();
        }
        else
        {
            noBulletSound.Play();
        }

        // Play the animation only on the first shot
        if (!hasPlayedGunShotAnimation)
        {
            animator.Play("GunShot", -1, 0f);
            hasPlayedGunShotAnimation = true; // Mark that the animation has been played
        }

        gunMag.MarkGunAsFired();
    }

    public void SetHaveBulletInChamber(bool value)
    {
        haveBulletInChamber = value;
    }

    public bool HasBulletInChamber()
    {
        return haveBulletInChamber;
    }

    // Reset animation flag when reloading
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