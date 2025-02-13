using UnityEngine;

public class ObjectFallOffTable : MonoBehaviour
{

    [SerializeField] private LayerMask flallOffLayer;

    [SerializeField] public bool destroyOnCollision = false;

    private Vector3 intialPosition;
    private void Start()
    {
        intialPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        int x = 1 << other.gameObject.layer;



        // Trigger Clown Falling
        if (x == flallOffLayer.value)
        {
            if(destroyOnCollision)
                Destroy(gameObject);
            else
                transform.position = intialPosition;
        }
    }
}
