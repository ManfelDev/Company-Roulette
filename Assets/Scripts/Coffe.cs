using UnityEngine;

public class Coffe : MonoBehaviour
{
    [SerializeField] private AudioSource drinkSound;
    
    private bool hasDrunk = false;
    private Transform playerHead;
    public float drinkDistance = 0.2f;

    private GameManager manager;

    private void Start()
    {
        playerHead = Camera.main?.transform;

        manager = FindAnyObjectByType<GameManager>();

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

        // Check if coffee is close enough to drink
        if (distance < drinkDistance)
        {
            DrinkCoffee();
        }
    }

    private void DrinkCoffee()
    {
        if (hasDrunk) return; // Prevents multiple drinks

        Debug.Log("Coffee consumed!");
        drinkSound.Play();
        manager.HealPlayer(1);
        hasDrunk = true;
    }
}