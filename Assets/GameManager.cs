using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    private int currentRound = 0;
    [field: Header("Magazines")]
    [SerializeField] private GameObject magazinePrefab;
    [SerializeField, Range(1, 5)] private int numberOfMagazines = 3;
    [SerializeField] private Transform[] magazineSpots;

    private List<GameObject> currentMagazines;

    [field: Header("Shooting Logic")]
    [SerializeField] private DetectAim detectAimScript;

    [field: Header("Game Logic")]
    [SerializeField] private int maxLives = 5;
    [SerializeField] private int playerLives = 3;
    private int startPlayerLives;
    private bool playerAlive = true;
    [SerializeField] private int bossLives = 3;
    private int startBossLives;
    private bool bossAlive = true;
    private bool playerTurn = true;
    [SerializeField] private int currentSalaryRaise = 250;
    public int CurrentSalary { get { return currentSalaryRaise; } }
    [SerializeField] private int salaryMultiplier = 2;
    public int SalaryMultiplier { get { return salaryMultiplier; } }
    [SerializeField] private MeshFilter[] salaryIndex;

    [field: Header("Briefcase Logic")]
    [SerializeField] private GameObject briefcaseQuestion;
    [SerializeField] private GameObject briefcasePrefab;
    [SerializeField] private Transform briefcaseSpawnPosition;
    [SerializeField] private Transform briefcaseSpotOnTable;
    [SerializeField] private float briefcaseTravelDuration = 2f;
    [SerializeField] private int briefcaseCost = 200;
    [SerializeField] private int briefcaseMultiplier = 2;

    private GameObject briefcase;

    [field: Header("Game Over Visuals")]
    [SerializeField] private GameObject gameOverPrompt;
    [SerializeField] private Text youGotThisMoneyText;

    [field: Header("Lives Visuals")]
    [SerializeField] private MeshFilter playerLivesRenderer;
    [SerializeField] private MeshFilter bossLivesRenderer;
    [SerializeField] private Mesh[] numbersMesh;

    [field: Header("Boss Visuals")]
    [SerializeField] private BossAnimationLogic bossAnimationLogic;

    [SerializeField] private GameObject pistol;
    private Vector3 pistolSpawnPosition;

    public bool playerCanPlayTwice = false;
    

    private void Awake()
    {
        currentMagazines = new List<GameObject>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pistolSpawnPosition = pistol.transform.position;

        startPlayerLives = playerLives;
        startBossLives = bossLives;

        currentRound++;
        StartRound(currentRound);


        //AcceptBriefcase();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            //DamageBoss(1);
        }
    }

    public void StartRound(int currentRound)
    {

        InstantiateMagazines();
        IncreaseSalary();
        UpdateSalary();
        playerLives = startPlayerLives;
        bossLives = startBossLives;
        UpdatePlayerLives();
        UpdateBossLives();
        ShowBriefcaseQuestion();

        playerTurn = true;

    }

    private void IncreaseSalary()
    {
        currentSalaryRaise *= salaryMultiplier;
        UpdateSalary();
    }

    private void UpdateSalary()
    {
        

        string salaryString = currentSalaryRaise.ToString();

        while (salaryString.Length < 6)
        {
            salaryString = "0" + salaryString;
        }


        if (salaryString.Length > 6)
        {
            for (int i = 0; i < salaryString.Length; i++)
                salaryIndex[i].mesh = numbersMesh[(int)salaryString[9]];
        }
        else
        {
            for (int i = 0; i < salaryString.Length; i++)
            {

                int index = (salaryString[i]) - '0';

                salaryIndex[i].mesh = numbersMesh[index];

            }
        }
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

    public void AddMoney(int amount)
    {
        currentSalaryRaise += amount;
    }

    public void RemoveMoney(int amount)
    {
        currentSalaryRaise -= amount;
    }

    public void ShowBriefcaseQuestion()
    {
        briefcaseQuestion.SetActive(true);
        briefcaseQuestion.GetComponent<BriefcaseQuestion>().UpdateBriefcaseCost(briefcaseCost);
    }

    public void AcceptBriefcase()
    {
        RemoveMoney(briefcaseCost);
        briefcaseCost *= briefcaseMultiplier;

        UpdateSalary();

        if(briefcase != null)
            Destroy(briefcase);

        briefcase = Instantiate(briefcasePrefab);

        StartCoroutine(BriefcaseTravel(briefcase));
    }

    private IEnumerator BriefcaseTravel(GameObject briefcase)
    {
        float lerpValue = 0f;

        while (lerpValue < 1)
        {
            lerpValue += Time.deltaTime / briefcaseTravelDuration;
            briefcase.transform.position = Vector3.Lerp(briefcaseSpawnPosition.position, briefcaseSpotOnTable.position, lerpValue);
            yield return null;
        }

        briefcase.GetComponent<BriefcaseScript>().SpawnItems();
    }

    public void CheckForAliveMagazines()
    {
        foreach (GameObject magazine in currentMagazines)
        {
            if (magazine != null)
                return;
        }

        InstantiateMagazines();
    }


    public void BossTurn()
    {
        playerTurn = false;

        StartCoroutine(StartBossTurn());
    }

    public IEnumerator StartBossTurn()
    {

        // play boss sound

        yield return new WaitForSeconds(3f);

        pistol.SetActive(false);

        bossAnimationLogic.ShootBoss();
    }

    public void AnimatorBossShot(bool playerShot, bool shotIsReal)
    {

        if (shotIsReal)
        {
            if(playerShot)
            {
                DamagePlayer(1);
                StartPlayerTurn();
            }
            else
            {
                DamageBoss(1);
                StartPlayerTurn();
            }
        }
        else
        {
            if(playerShot)
            {
                StartPlayerTurn();

            }
            else
            {
                StartCoroutine(StartBossTurn());
            }
        }
    }

    public void StartPlayerTurn()
    {
        pistol.SetActive(true);
        pistol.transform.position = pistolSpawnPosition;

        //ShowBriefcaseQuestion();

        playerTurn = true;
    }

    public void WhoGotShot(bool shotIsReal)
    {
        switch (detectAimScript.WhoGotShot())
        {
            case 0:
                Debug.Log("Shot missed both");
                if (shotIsReal)
                {
                    // player turn over
                    if (playerAlive && bossAlive && !playerCanPlayTwice)
                        BossTurn();

                    if (playerCanPlayTwice) playerCanPlayTwice = false;
                }
                else
                {
                    // player turn keeps going
                }
                break;
            case 1:
                Debug.Log("Shot hit player");
                if (shotIsReal)
                {
                    // player turn over
                    DamagePlayer(1);
                    if (playerAlive && bossAlive && !playerCanPlayTwice)
                        BossTurn();

                    if (playerCanPlayTwice) playerCanPlayTwice = false;
                }
                else
                {
                    // player turn keeps going
                }

                break;
            case 2:
                Debug.Log("Shot hit boss");
                if (shotIsReal)
                {
                    // player turn over
                    DamageBoss(1);
                    if (playerAlive && bossAlive && !playerCanPlayTwice)
                        BossTurn();

                    if (playerCanPlayTwice) playerCanPlayTwice = false;
                }
                else
                {
                    // player turn over
                    if (playerAlive && bossAlive)
                        BossTurn();

                    if (playerCanPlayTwice) playerCanPlayTwice = false;
                }
                break;
        }


    }

    public void DamagePlayer(int damage)
    {
        playerLives -= damage;

        if (playerLives <= 0)
        {
            Debug.Log("Player lost");
            playerAlive = false;

            //Game over
            gameOverPrompt.SetActive(true);
            youGotThisMoneyText.text = $"You got: {currentSalaryRaise}$";
        }

        UpdatePlayerLives();
    }



    public void HealPlayer(int amount)
    {
        playerLives += amount;

        if (playerLives > maxLives)
        {
            playerLives = maxLives;
        }

        UpdatePlayerLives();
    }

    private void UpdatePlayerLives()
    {
        playerLivesRenderer.mesh = numbersMesh[playerLives];
    }

    public void DamageBoss(int damage)
    {

        // play boss hurt sound

        bossLives -= damage;

        if (bossLives <= 0)
        {
            Debug.Log("Boss lost");
            bossAlive = false;

            StartRound(currentRound++);
        }

        UpdateBossLives();
    }

    public void HealBoss(int amount)
    {
        bossLives += amount;

        if (bossLives > maxLives)
        {
            bossLives = maxLives;
        }
        UpdateBossLives();
    }

    private void UpdateBossLives()
    {
        bossLivesRenderer.mesh = numbersMesh[bossLives];
    }

}
