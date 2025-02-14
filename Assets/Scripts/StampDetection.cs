using UnityEngine;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticsUtility;

public class StampDetection : MonoBehaviour
{
    [SerializeField] private LayerMask paperLayer;
    [SerializeField] private float detectionDistance = 0.05f;
    [SerializeField] private GameObject stampMarkPrefab; // Prefab for the stamp mark
    [SerializeField] private HandDetection handDetection;
    [SerializeField] private AudioSource stampSound;

    private ObjectFallOffTable objectFallOffTable;

    private GameObject paperModel;
    private bool hasStamped; // Prevents multiple detections

    private GameManager gameManager;

    private void Start()
    {
        paperModel = GameObject.FindGameObjectWithTag("Paper");
        hasStamped = false;
        objectFallOffTable = GetComponentInParent<ObjectFallOffTable>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void FixedUpdate()
    {
        if (!hasStamped)
        {
            CheckForPaper();
        }
    }

    void CheckForPaper()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, detectionDistance, paperLayer))
        {
            Debug.Log("Stamp touched the paper at: " + hit.point);
            PlaceStampMark(hit.point, hit.normal);
            hasStamped = true;
        }
    }

    void PlaceStampMark(Vector3 position, Vector3 normal)
    {
        if (stampMarkPrefab != null)
        {
            // Instantiate the stamp mark at the hit position
            GameObject stampMark = Instantiate(stampMarkPrefab, position, Quaternion.identity);

            // Adjust rotation to match the stamp's orientation
            stampMark.transform.rotation = Quaternion.LookRotation(normal) * Quaternion.Euler(90, 0, 0);

            // Match the Y-axis rotation of the stamp
            stampMark.transform.rotation = Quaternion.Euler(stampMark.transform.eulerAngles.x, transform.eulerAngles.y + 180, stampMark.transform.eulerAngles.z);

            // Offset slightly to avoid Z-fighting
            stampMark.transform.position += normal * 0.001f;

            stampMark.transform.SetParent(paperModel.transform);

            gameManager.playerCanPlayTwice = true;

            objectFallOffTable.destroyOnCollision = true;
        }
        else
        {
            Debug.LogWarning("Stamp prefab not assigned!");
        }

        stampSound.Play();
        SendHapticFeedback(1f, 0.05f, 1f);
    }
    
    private void SendHapticFeedback(float hapticAmplitude, float hapticDuration, float hapticFrequency)
    {
        if (handDetection.IsRightHand())
        {
            SendHapticImpulse(hapticAmplitude, hapticDuration, Controller.Right, hapticFrequency);
        }
        else if (handDetection.IsLeftHand())
        {
            SendHapticImpulse(hapticAmplitude, hapticDuration, Controller.Left, hapticFrequency);
        }
    }
}