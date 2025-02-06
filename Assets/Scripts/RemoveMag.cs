using UnityEngine;
using UnityEngine.InputSystem;

public class RemoveMag : MonoBehaviour
{
    [SerializeField] private GameObject mag;
    [SerializeField] private Transform magSpawnPoint;

    [SerializeField] private GameObject magInGun;

    [SerializeField] InputActionReference[] Actions;

    private bool canIRemoveMag = true;

    void OnEnable()
    {
        foreach (var action in Actions)
        {
            action.action.Enable();
            action.action.performed += ExampleFunction;
        }
    }

    void OnDisable()
    {
        foreach (var action in Actions)
        {
            action.action.Disable();
            action.action.performed -= ExampleFunction;
        }
    }

    void ExampleFunction(InputAction.CallbackContext obj)
    {
        SpawnMag();
    }

    void Update()
    {
        if (canIRemoveMag)
        {
            if (Actions[0].action.triggered || Actions[1].action.triggered)
            {
                SpawnMag();
                canIRemoveMag = false;
            }
        }
    }

    public void SpawnMag()
    {
        magInGun.SetActive(false);

        Instantiate(mag, magSpawnPoint.position, magSpawnPoint.rotation);
    }
}
