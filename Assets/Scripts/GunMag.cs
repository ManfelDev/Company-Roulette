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

    private bool canIRemoveMag;
    private bool hasFiredWithCurrentMag; // Track if the gun has been fired at least once

    private void Start()
    {
        canIRemoveMag = true;
        xRSocketTagInteractor.socketActive = false;
        gunShot.SetHaveBulletInChamber(true);
        hasFiredWithCurrentMag = false; // Reset fire status for the first mag
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
    }

    public void ReloadMag()
    {
        // Check if any interactable is currently selected in the socket
        if (xRSocketTagInteractor.interactablesSelected.Count > 0)
        {
            var interactable = xRSocketTagInteractor.interactablesSelected[0];

            if (interactable != null)
            {
                Destroy(interactable.transform.gameObject);
            }
        }

        magInGun.SetActive(true);
        canIRemoveMag = true;
        xRSocketTagInteractor.socketActive = false;
        gunShot.SetHaveBulletInChamber(true);
        hasFiredWithCurrentMag = false; // Reset because this is a fresh magazine
    }

    // Called from GunShot when the gun is fired
    public void MarkGunAsFired()
    {
        hasFiredWithCurrentMag = true;
    }
}