using UnityEngine;

public class GunShot : MonoBehaviour
{
    [SerializeField] private bool haveBulletInChamber;
    [SerializeField] private AudioSource gunshotSound;
    [SerializeField] private AudioSource noBulletSound;
    [SerializeField] private GunMag gunMag; // Reference to GunMag to update firing status

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Shot()
    {
        if (haveBulletInChamber)
        {
            animator.Play("GunShot", -1, 0f);
            gunshotSound.Play();
            haveBulletInChamber = false;

            // Inform GunMag that the gun has been fired
            if (gunMag != null)
            {
                gunMag.MarkGunAsFired();
            }
        }
        else
        {
            noBulletSound.Play();
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
}