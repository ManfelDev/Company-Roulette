using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HandDetection : MonoBehaviour
{
    private bool isRightHand = false;
    private bool isLeftHand = false;

    public void OnSelectEnter(SelectEnterEventArgs args)
    {
        XRBaseInteractor interactor = args.interactorObject as XRBaseInteractor;

        if (interactor != null)
        {
            if (interactor.gameObject.tag == "Right Hand")
            {
                isRightHand = true;
                Debug.Log("Right Hand Detected");
            }
            else if (interactor.gameObject.tag == "Left Hand")
            {
                isLeftHand = true;
                Debug.Log("Left Hand Detected");
            }
        }
    }

    public bool IsRightHand()
    {
        return isRightHand;
    }

    public bool IsLeftHand()
    {
        return isLeftHand;
    }

    public void HandDetectionReset()
    {
        isRightHand = false;
        isLeftHand = false;
    }
}
