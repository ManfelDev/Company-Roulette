using UnityEngine;
using UnityEngine.InputSystem;

public class GunMag : MonoBehaviour
{
    [SerializeField] private GameObject emptyMagPrefab;
    [SerializeField] private Transform magSpawnPoint;
    [SerializeField] private GameObject magInGun;
    [SerializeField] InputActionReference RightHand;
    [SerializeField] InputActionReference LeftHand;
    [SerializeField] private HandDetection handDetection;
    [SerializeField] private XRSocketTagInteractor xRSocketTagInteractor;
    [SerializeField] private GunShot gunShot;

    private Animator animator;
    private bool canIRemoveMag;
    private bool hasFiredWithCurrentMag;

    private void Start()
    {
        canIRemoveMag = false;
        xRSocketTagInteractor.socketActive = true;
        gunShot.SetHaveBulletInChamber(false);
        hasFiredWithCurrentMag = true;
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        RightHand.action.Enable();
        RightHand.action.performed += RightHandFunction;

        LeftHand.action.Enable();
        LeftHand.action.performed += LeftHandFunction;
    }

    void OnDisable()
    {
        RightHand.action.Disable();
        RightHand.action.performed -= RightHandFunction;

        LeftHand.action.Disable();
        LeftHand.action.performed -= LeftHandFunction;
    }

    void RightHandFunction(InputAction.CallbackContext obj)
    {
        if (canIRemoveMag && handDetection != null && handDetection.IsRightHand())
        {
            UnloadGun();
        }
    }

    void LeftHandFunction(InputAction.CallbackContext obj)
    {
        if (canIRemoveMag && handDetection != null && handDetection.IsLeftHand())
        {
            UnloadGun();
        }
    }

    public void UnloadGun()
    {
        // Prevent unloading if the gun has not been fired at least once
        if (!hasFiredWithCurrentMag)
        {
            Debug.Log("Cannot unload the magazine without firing at least once.");
            return;
        }

        magInGun.SetActive(false);
        Instantiate(emptyMagPrefab, magSpawnPoint.position, magSpawnPoint.rotation);
        canIRemoveMag = false;
        xRSocketTagInteractor.socketActive = true;
        gunShot.SetHaveBulletInChamber(false);
        hasFiredWithCurrentMag = false; // Reset for the next magazine
        gunShot.DeactivateBangFlag();
    }

    public void ReloadMag()
    {
        if (xRSocketTagInteractor.interactablesSelected.Count > 0)
        {
            var interactable = xRSocketTagInteractor.interactablesSelected[0];

            if (interactable != null)
            {
                Magazine magazine = interactable.transform.GetComponent<Magazine>();

                if (magazine != null)
                {
                    gunShot.SetHaveBulletInChamber(magazine.HasBullet);

                    if (!magazine.HasBullet)
                    {
                        interactable.transform.gameObject.tag = "UsedMagazine";
                    }
                }

                Destroy(interactable.transform.gameObject);
            }
        }

        animator.Play("GunReload", -1, 0f);
        magInGun.SetActive(true);
        canIRemoveMag = true;
        xRSocketTagInteractor.socketActive = false;
        hasFiredWithCurrentMag = false;

        // Reset the GunShot animation when reloading
        gunShot.ResetGunShotAnimation();
    }

    // Called from GunShot when the gun is fired
    public void MarkGunAsFired()
    {
        hasFiredWithCurrentMag = true;
    }
}