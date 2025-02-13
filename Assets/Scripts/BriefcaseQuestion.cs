using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BriefcaseQuestion : MonoBehaviour
{

    [SerializeField] private Text moneyText;
    [SerializeField] private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //gameManager = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GiveBriefcase()
    {
        gameManager.AcceptBriefcase();
    }

    public void UpdateBriefcaseCost(int amount)
    {
        moneyText.text = amount.ToString() + "$";
    }
}
