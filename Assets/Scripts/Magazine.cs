using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField] private bool hasBullet;
    [SerializeField] private float bulletChance = 0.5f;

    public bool HasBullet => hasBullet;

    private void Start()
    {
        // Randomly decide if this magazine has a bullet (50/50 chance)
        hasBullet = Random.value > bulletChance;
    }

    public void UseMagazine()
    {
        hasBullet = false; // The bullet is used
        gameObject.tag = "UsedMagazine"; // Change tag to indicate it's empty
    }
}
