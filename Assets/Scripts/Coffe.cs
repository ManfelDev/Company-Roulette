using UnityEngine;

public class Coffe : MonoBehaviour
{
    private Transform playerHead;
    public float drinkDistance = 0.2f;

    private void Start()
    {
        playerHead = Camera.main?.transform;

        if (playerHead == null)
        {
            Debug.LogError("Player head (XR Camera) not found! Assign manually.");
        }
    }

    private void Update()
    {
        if (playerHead == null) return; // Avoid null reference errors

        // Calculate distance between coffee and player's head
        float distance = Vector3.Distance(transform.position, playerHead.position);
        Debug.Log($"Distance to coffee: {distance:F2}m");

        // Check if coffee is close enough to drink
        if (distance < drinkDistance)
        {
            DrinkCoffee();
        }
    }

    private void DrinkCoffee()
    {
        Debug.Log("Coffee consumed!");
        Destroy(gameObject); // Simulate drinking by removing the cup
    }
}