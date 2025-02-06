using UnityEngine;
using Unity.XR;

public class GunShot : MonoBehaviour
{
    [SerializeField] private bool haveBulletInChamber = true;

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
            // PLay gun animation "GunShot"
            animator.Play("GunShot",  -1, 0f);
            
            // Play gunshot sound
            gunshotSound.Play();
        }

        else
        {       
            // Play no bullet sound
            noBulletSound.Play();
        }
    }
}
