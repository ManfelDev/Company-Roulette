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

    [field: Header("Shooting Logic")]
    [SerializeField] private DetectAim detectAimScript;

    [field: Header("Game Logic")]
    [SerializeField] private int playerLives = 3;
    private bool playerAlive = true;
    [SerializeField] private int bossLives = 3;
    private bool bossAlive = true;
    private bool playerTurn = true;
    [SerializeField] private int currentSalaryRaise = 1000;

    private void Awake()
    {
        currentMagazines = new List<GameObject>();
    }
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


    public void ChangeTurn()
    {
        playerTurn = !playerTurn;
    }


    public void WhoGotShot()
    {
        switch(detectAimScript.WhoGotShot())
        {
            case 0:
                Debug.Log("Shot missed both");
                break;
            case 1:
                Debug.Log("Shot hit player");
                DamagePlayer(1);
                break;
            case 2:
                Debug.Log("Shot hit boss");
                DamageBoss(1);
                break;
        }

        if(playerAlive && bossAlive)
            ChangeTurn();
    }

    private void DamagePlayer(int damage)
    {
        playerLives -= damage;

        if(playerLives <= 0)
        {
            Debug.Log("Player lost");
            playerAlive = false;
        }
    }

    private void HealPlayer(int amount)
    {
        playerLives += amount;
    }

    private void DamageBoss(int damage)
    {
        bossLives -= damage;

        if (bossLives <= 0)
        {
            Debug.Log("Boss lost");
            bossAlive = false;
        }
    }

    private void HealBoss(int amount)
    {
        bossLives += amount;
    }
}
