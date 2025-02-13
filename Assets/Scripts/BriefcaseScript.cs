using UnityEngine;

public class BriefcaseScript : MonoBehaviour
{

    [SerializeField] private Transform itemSlot1;
    [SerializeField] private Transform itemSlot2;
    [SerializeField] private GameObject[] toolPrefabs;

    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();

        //SpawnItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItems()
    {
        animator.SetTrigger("Open");

        GameObject item = Instantiate(toolPrefabs[Random.Range(0, toolPrefabs.Length)], itemSlot1);
        item.transform.position = itemSlot1.position;

        item.transform.parent = null;

        item = Instantiate(toolPrefabs[Random.Range(0, toolPrefabs.Length)], itemSlot2);
        item.transform.position = itemSlot2.position;

        item.transform.parent = null;
    }
}
