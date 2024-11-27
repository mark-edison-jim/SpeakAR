using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using static UnityEngine.Rendering.DebugUI;

public class RayPlacer : MonoBehaviour
{
    private GameObject prefab;
    private ARRaycastManager acm;
    private Vector2 touchPosition;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    [SerializeField] private GameObject[] prefabs;
    private List<GameObject> placedObjects = new List<GameObject>();
    [SerializeField] private Text activePrefabTxt;
    private GameObject selectedObject = null;
    private bool isDragging = false;
    [SerializeField] private Slider horizontalSlider;
    [SerializeField] private Slider verticalSlider;
    private bool spawningEnabled = true;    // Controls if spawning is allowed
    private bool interactionEnabled = true; // Controls if interaction is allowed

    void Start()
    {
        acm = GetComponent<ARRaycastManager>();
        GestureManager.Instance.OnRotate += HandleRotate;
        if (prefabs.Length > 0) prefab = prefabs[0]; // Initialize with the first prefab

        horizontalSlider.onValueChanged.AddListener(OnHorizontalSliderChanged);
        verticalSlider.onValueChanged.AddListener(OnVerticalSliderChanged);
    }

    private void OnDestroy()
    {
        if (GestureManager.Instance != null)
            GestureManager.Instance.OnRotate -= HandleRotate;
    }

    private void HandleRotate(object sender, RotateEventArgs e)
    {
        if (selectedObject != null && interactionEnabled) // Check if an object is selected and interaction is enabled
        {
            if (e.RotationDirection == RotateDirection.CCW)
            {
                selectedObject.transform.Rotate(Vector3.up, e.Angle * 2);
            }
            else if (e.RotationDirection == RotateDirection.CW)
            {
                selectedObject.transform.Rotate(Vector3.down, e.Angle * 2);
            }
        }
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                touchPosition = default;
                return false;
            }
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    public void EnableFeatures()
    {
        spawningEnabled = true;
        interactionEnabled = true;
    }
    public void DisableFeatures()
    {
        spawningEnabled = false;
        interactionEnabled = false;
    }

    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (acm.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (interactionEnabled)
                {
                    GameObject tappedObject = GetObjectAtTouchPosition(touchPosition);

                    if (tappedObject != null)
                    {
                        if (selectedObject == tappedObject)
                        {
                            // Toggle the script activation on the child GameObject
                            //ToggleChildScript(selectedObject, false);
                            selectedObject = null;
                            isDragging = false;
                            activePrefabTxt.text = "Active Prefab: None";
                        }
                        else
                        {
                            selectedObject = tappedObject;
                            activePrefabTxt.text = "Active Prefab: " + selectedObject.name  ;
                            //ToggleChildScript(selectedObject, true);
                            isDragging = true;
                        }
                    }
                }

                if (spawningEnabled && !IsPositionOccupied(hitPose.position))
                {
                    prefab = prefabs[Random.Range(0, prefabs.Length)];
                    GameObject newObject = Instantiate(prefab, hitPose.position, hitPose.rotation);
                    newObject.transform.Rotate(0, 180, 0); // Rotate the object 180 degrees around the Y axis
                    if (newObject.GetComponent<Collider>() == null) // Add collider if missing
                    {
                        newObject.AddComponent<BoxCollider>();
                    }
                    placedObjects.Add(newObject);
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && isDragging && selectedObject != null && interactionEnabled)
            {
                selectedObject.transform.position = hitPose.position;
            }
        }
    }

    GameObject GetObjectAtTouchPosition(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            foreach (GameObject placedObject in placedObjects)
            {
                if (hit.transform.gameObject == placedObject)
                {
                    return placedObject;
                }
            }
        }
        return null;
    }

    bool IsPositionOccupied(Vector3 newPosition)
    {
        foreach (GameObject placedObject in placedObjects)
        {
            if (Vector3.Distance(placedObject.transform.position, newPosition) < 0.2f)
            {
                return true;
            }
        }
        return false;
    }

    public void ClearAllPlacedObjects()
    {
        foreach (GameObject placedObject in placedObjects)
        {
            Destroy(placedObject);
        }
        placedObjects.Clear();
        selectedObject = null;
        isDragging = false;
    }

    private void ToggleChildScript(GameObject parentObject, bool isSelected)
    {
        // Assumes the ToggleScript is attached to a child GameObject
        OutlinePrefab toggleScript = parentObject.GetComponentInChildren<OutlinePrefab>();

        if (toggleScript != null)
        {
            toggleScript.gameObject.SetActive(isSelected);
        }
    }

    private void OnHorizontalSliderChanged(float value)
    {
        if (selectedObject != null)
        {
            Vector3 scale = selectedObject.transform.localScale;
            scale.x = value; // Adjust y-axis scale based on slider value
            selectedObject.transform.localScale = scale;
        }
    }

    private void OnVerticalSliderChanged(float value)
    {
        if (selectedObject != null)
        {
            Vector3 scale = selectedObject.transform.localScale;
            scale.y = value; // Adjust x-axis scale based on slider value
            selectedObject.transform.localScale = scale;
        }
    }
    public void deleteSelected()
    {
        if (selectedObject != null)
        {
            placedObjects.Remove(selectedObject); // Remove from placedObjects list
            Destroy(selectedObject);              // Destroy the selected object
            selectedObject = null;                // Set selectedObject to null
            isDragging = false;                   // Reset dragging flag
            activePrefabTxt.text = "Active Prefab: None"; // Update UI text
        }
    }

}
