using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class ARPrefabDeleter : MonoBehaviour
{
    public static ARPrefabDeleter Instance { get; private set; }
    private Dictionary<GameObject, ARSelectionInteractable> prefabDictionary = new Dictionary<GameObject, ARSelectionInteractable>();
    private GameObject selectedPrefab;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Add placed object to the dictionary
    public void AddParent(ARObjectPlacementEventArgs args)
    {
        GameObject placedObject = args.placementObject;

        var selectionInteractable = placedObject.GetComponent<ARSelectionInteractable>();
        if (selectionInteractable != null)
        {
            placedObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            // Add to the dictionary
            prefabDictionary.Add(placedObject, selectionInteractable);
            Debug.Log("amongPrefab added: " + placedObject.name);

            // Automatically assign the Select Entered event to the spawned prefab's ARSelectionInteractable
            selectionInteractable.selectEntered.AddListener((SelectEnterEventArgs eventArgs) =>
            {
                OnPrefabSelected(eventArgs);
            });
        }
        else
        {
            Debug.LogWarning("amongPlaced object does not have an ARSelectionInteractable component!");
        }
    }

    // Disable selection for all prefabs
    public void DisableSelection()
    {
        foreach (var kvp in prefabDictionary)
        {
            kvp.Value.enabled = false;
            Debug.Log("amongDisabled selection for: " + kvp.Key.name);
        }
    }

    // Enable selection for all prefabs
    public void EnableSelection()
    {
        foreach (var kvp in prefabDictionary)
        {
            kvp.Value.enabled = true;
            Debug.Log("amongEnabled selection for: " + kvp.Key.name);
        }
    }

    // Handle selection of a prefab
    public void OnPrefabSelected(SelectEnterEventArgs args)
    {
        selectedPrefab = args.interactableObject.transform.gameObject; // Store the selected prefab
        Debug.Log("amongPrefab selected: " + selectedPrefab.name);
    }

    // Unselect the prefab
    public void UnselectPrefab()
    {
        Debug.Log("amongPrefab unselected: " + selectedPrefab?.name);
        selectedPrefab = null;
    }

    // Delete the selected prefab
    public void DeleteSelectedPrefab()
    {
        if (selectedPrefab == null)
        {
            Debug.LogWarning("amongNo prefab selected to delete!");
            return;
        }

        if (prefabDictionary.ContainsKey(selectedPrefab))
        {
            // Remove from the dictionary and destroy the prefab
            var selectionInteractable = prefabDictionary[selectedPrefab];
            prefabDictionary.Remove(selectedPrefab);
            Destroy(selectedPrefab);
            Debug.Log("amongPrefab deleted: " + selectedPrefab.name);

            selectedPrefab = null; // Reset the selection
        }
        else
        {
            Debug.LogWarning("amongSelected prefab not found in the dictionary!");
        }
    }
}
    