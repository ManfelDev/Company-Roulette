using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int numberOfRounds = 3;
    [field: Header("Magazines")]
    [SerializeField] private GameObject magazinePrefab;
    [SerializeField, Range(1, 5)] private int numberOfMagazines = 3;
    [SerializeField] private Transform[] magazineSpots;

    private List<GameObject> currentMagazines;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentMagazines = new List<GameObject>();
        InstantiateMagazines();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateMagazines()
    {
        currentMagazines.Clear();
        for (int i = 0; i < numberOfMagazines; i++)
        {
            GameObject tempMagazine = Instantiate(magazinePrefab, magazineSpots[i].transform);
            tempMagazine.transform.position = magazineSpots[i].position;
            currentMagazines.Add(tempMagazine);
        }

    }

    public void CheckForAliveMagazines()
    {
        foreach(GameObject magazine in currentMagazines)
        {
            if (magazine != null)
                return;
        }

        InstantiateMagazines();
    }
}
