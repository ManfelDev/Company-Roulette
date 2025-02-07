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

    private void Start()
    {
        canIRemoveMag = true;
        xRSocketTagInteractor.socketActive = false;
        gunShot.SetHaveBulletInChamber(true);
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
            SpawnMag();
        }
    }

    void LeftHandFunction(InputAction.CallbackContext obj)
    {
        if (canIRemoveMag && handDetection != null && handDetection.IsLeftHand())
        {
            SpawnMag();
        }
    }

    public void SpawnMag()
    {
        magInGun.SetActive(false);
        Instantiate(emptyMagPrefab, magSpawnPoint.position, magSpawnPoint.rotation);
        canIRemoveMag = false;
        xRSocketTagInteractor.socketActive = true;
        gunShot.SetHaveBulletInChamber(false);
    }

    public void ReloadMag()
    {
        // Check if any interactable is currently selected in the socket
        if (xRSocketTagInteractor.interactablesSelected.Count > 0)
        {
            // Get the first interactable in the list
            var interactable = xRSocketTagInteractor.interactablesSelected[0];

            if (interactable != null)
            {
                // Destroy the GameObject in the socket
                Destroy(interactable.transform.gameObject);
            }
        }

        magInGun.SetActive(true);
        canIRemoveMag = true;
        xRSocketTagInteractor.socketActive = false;
        gunShot.SetHaveBulletInChamber(true);
    }
}