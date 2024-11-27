using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class AddToParent : MonoBehaviour
{
    private List<GameObject> prefabs;
    private GameObject selected;
    void Start()
    {
        prefabs = new List<GameObject>();
    }


    // Add placed object to the list
    public void AddParent(ARObjectPlacementEventArgs args)
    {
        GameObject placedObject = args.placementObject;
        prefabs.Add(placedObject);
        Debug.Log("amongPrefab added to parent0: " + placedObject);
        Debug.Log("amongSize: " + prefabs.Count);
        foreach (GameObject child in prefabs)
        {
            Debug.Log("amongPrefab added to parentPoop: " + child);
        }
    }

    // Disable selection for all prefabs
    public void DisableSelection()
    {
        foreach (GameObject child in prefabs)
        {
            Debug.Log("amongus Disable selection for " + child);
            child.GetComponent<ARSelectionInteractable>().enabled = false;
            
        }
    }

    // Enable selection for all prefabs
    public void EnableSelection()
    {
        foreach (GameObject child in prefabs)
        {
            Debug.Log("Enable selection for " + child);
            child.GetComponent<ARSelectionInteractable>().enabled = true;
        }
    }

    public void DeletePrefab()
    {
        foreach (GameObject child in prefabs)
        {
            if (child.GetComponent<ARSelectionInteractable>().selected)
            {
                Debug.Log("amongus otherside 1: " + child);
                prefabs.Remove(child);
                Destroy(child);
                Debug.Log("amongus otherside 2: " + child);
            }
        }
       
    }
}
