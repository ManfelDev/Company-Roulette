using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HandDetection : MonoBehaviour
{
    public void OnSelectEnter(SelectEnterEventArgs args)
    {
        XRBaseInteractor interactor = args.interactorObject as XRBaseInteractor;

        if (interactor != null)
        {
            Debug.Log("Objeto pegado com: " + interactor.gameObject.tag);
        }
    }
}
