using UnityEngine;
using Unity.XR;

public class GunShot : MonoBehaviour
{
    [SerializeField] private bool haveBulletInChamber;
    [SerializeField] private AudioSource gunshotSound;
    [SerializeField] private AudioSource noBulletSound;

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