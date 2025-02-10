using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int numberOfRounds = 3;
    [field: Header("Magazines")]
    [SerializeField] private GameObject magazinePrefab;
    [SerializeField, Range(1, 5)] private int numberOfMagazines = 3;
    [SerializeField] private Transform[] magazineSpots;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InstantiateMagazines();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateMagazines()
    {

        for (int i = 0; i < numberOfMagazines; i++)
        {
            GameObject tempMagazine = Instantiate(magazinePrefab, magazineSpots[i].transform);
            tempMagazine.transform.position = magazineSpots[i].position;
        }

    }
}
