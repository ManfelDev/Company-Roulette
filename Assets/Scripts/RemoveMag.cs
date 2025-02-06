using UnityEngine;
using UnityEngine.InputSystem;

public class RemoveMag : MonoBehaviour
{
    [SerializeField] private GameObject mag;
    [SerializeField] private Transform magSpawnPoint;
    [SerializeField] private GameObject magInGun;
    [SerializeField] InputActionReference RightHand;
    [SerializeField] InputActionReference LeftHand;
    [SerializeField] private HandDetection handDetection;

    private bool canIRemoveMag;

    private void Start()
    {
        canIRemoveMag = true;
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
        Instantiate(mag, magSpawnPoint.position, magSpawnPoint.rotation);
        canIRemoveMag = false;
    }
}