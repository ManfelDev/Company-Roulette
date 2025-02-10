using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DetectAim : MonoBehaviour
{

    [SerializeField] private Transform gunPoint;
    [SerializeField] private float maxDistance;
    public GameObject PointingAt {  get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        DetectObject();
    }

    private void DetectObject()
    {
        Ray ray = new Ray(gunPoint.transform.position, -gunPoint.transform.right);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red); // Draw debug ray


        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            PointingAt = hit.collider.gameObject;
            Debug.Log("Looking at: " + hit.collider.gameObject.name);

        }
    }
}
