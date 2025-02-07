using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private GameObject lights;

    public void TurnOnLights()
    {
        lights.SetActive(true);
    }

    public void TurnOffLights()
    {
        lights.SetActive(false);
    }
}
