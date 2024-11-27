using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class togglePlane : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    private bool isPlaneActive = true;

    private void Start()
    {
        SetPlanesActive(isPlaneActive);
    }

    public void onButtonClick()
    {
        isPlaneActive = !isPlaneActive;
        SetPlanesActive(isPlaneActive);
    }

    private void SetPlanesActive(bool isActive)
    {
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(isActive);
        }
    }
}
