using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class ARAnchorSpawner : MonoBehaviour
{
    private ARAnchorManager anchorManager;
    [SerializeField] private ARPlaneManager planeManager; // Reference to ARPlaneManager
    [SerializeField] private ARRaycastManager raycastManager; // Reference to ARRaycastManager
    [SerializeField] public GameObject parent;
    private GameObject prefabToAnchor;
    [SerializeField] private float forwardOffset = 2f;
    [SerializeField] private Text text;
    private GameObject selectedObject; // Object being dragged
    private bool isDragging = false; // To track if the user is dragging an object

    // Start is called before the first frame update
    void Start()
    {
        anchorManager = GetComponent<ARAnchorManager>();
        planeManager = GetComponent<ARPlaneManager>();
        raycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                text.text = "Touch detected...";
                // Check if the user touched an existing object to drag
                if (TrySelectObject(touch.position))
                {
                    isDragging = true; // Start dragging the object
                    text.text = "Dragging object...";
                }
                else
                {
                    // If no object was touched, place a new one
                    if (TryGetTouchPositionOnPlane(touch.position, out Vector3 hitPosition))
                    {
                        AnchorObject(hitPosition);
                        text.text = "Placed object...";
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // If dragging, update the object's position
                text.text = "Dragging object...";
                DragObject(touch.position);
                
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // Release the object when the touch ends
                isDragging = false;
                selectedObject = null;
            }
        }
    }

    // Check if the user touched a spawned object and select it for dragging
    private bool TrySelectObject(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        bool touchingGameObject = false;
        Collider[] colliders = Physics.OverlapSphere(touchPosition, 0.1f);
        GameObject found = null;

        foreach (Transform child in parent.transform)
        {
            if (child.gameObject == colliders[0].gameObject)
            {
                found = child.gameObject;
                touchingGameObject = true;
                break;
            }
        }

        // Use Physics.Raycast to detect if the user touched an object (with a collider)
        if (Physics.Raycast(ray, out hit) || touchingGameObject)
        {
            text.text = "Entered";
            selectedObject = found;
            // If the user touches a spawned object with the "SpawnedObject" tag
            if (hit.collider != null && hit.collider.gameObject.tag == "SpawnedObject")
            {
                selectedObject = hit.collider.gameObject;

                return true;
            }
        }
        return false;
    }

    // Move the selected object based on the touch position
    private void DragObject(Vector2 touchPosition)
    {
        // Raycast to check where the object should move on the plane
        if (TryGetTouchPositionOnPlane(touchPosition, out Vector3 hitPosition))
        {
            selectedObject.transform.localPosition = hitPosition;
        }
    }

    // Check if the touch position is on a plane, using ARRaycastManager for raycasts
    private bool TryGetTouchPositionOnPlane(Vector2 touchPosition, out Vector3 hitPosition)
    {
        hitPosition = Vector3.zero;

        // Use ARRaycastManager to perform raycast against planes
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            // If a plane is hit, get the position
            hitPosition = hits[0].pose.position;
            return true;
        }
        return false;
    }

    // Anchor an object at the given world position
    public void AnchorObject(Vector3 worldPos)
    {
        if (prefabToAnchor != null)
        {
            // Create a new anchor at the given world position
            GameObject newAnchor = new GameObject("NewAnchor");
            newAnchor.transform.position = worldPos;
            ARAnchor anchor = newAnchor.AddComponent<ARAnchor>();

            // Instantiate the object and parent it to the anchor
            GameObject obj = Instantiate(prefabToAnchor, newAnchor.transform);
            obj.transform.localPosition = Vector3.zero;

            // Assign a tag to the spawned object to make it selectable
            obj.tag = "SpawnedObject";

            // Optional: Parent to another object in the hierarchy
            obj.transform.parent = parent.transform;
        }
    }

    public void ChangePrefab(GameObject newPrefab)
    {
        prefabToAnchor = newPrefab;
    }
}




/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class AnchorPlacer : MonoBehaviour
{
    [SerializeField] private GameObject contentPrefab;
    [SerializeField] private Vector3 contentOffset = new Vector3(0, 0.5f, 0);
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits;

    ARAnchorManager anchorManager;
    [SerializeField] ARPointCloudManager pointCloudManager;
    [SerializeField] public GameObject parent;

    private GameObject prefabToAnchor;
    [SerializeField] private float forwardOffset = 2f;

    [SerializeField] private GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        anchorManager = GetComponent<ARAnchorManager>();
        pointCloudManager = GetComponent<ARPointCloudManager>();
        //raycastManager = GetComponent<ARRaycastManager>();
        //hits = new List<ARRaycastHit>();
    }

    public void changePrefab(GameObject newPrefab)
    {
        prefabToAnchor = newPrefab;
        contentPrefab = newPrefab;
    }

    // Update is called once per frame
    private void Update()
    {
        //if(Input.GetTouch(0).phase == TouchPhase.Began)
        //{
        //    Ray r = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        //    Debug.DrawRay(r.origin, r.direction * 100, Color.red, 1.5f);
        //    if (raycastManager.Raycast(r, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        //    {
        //        Debug.Log($"Hit Count: {hits.Count}");
        //        AnchorRay(hits[0]);
        //    }
        //}
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //if point detected by touch is within plane, then you put, else you don't
            Vector3 spawnPos = Camera.main.ScreenPointToRay(Input.GetTouch(0).position)
                .GetPoint(forwardOffset);
            AnchorObject(spawnPos);
        }
    }

    public void AnchorRay(ARRaycastHit hit)
    {
        GameObject newAnchor = new GameObject("Anchor");
        newAnchor.transform.position = hit.pose.position;
        newAnchor.AddComponent<ARAnchor>();

        GameObject obj = Instantiate(contentPrefab, newAnchor.transform);
        obj.transform.localPosition = contentOffset;
        obj.transform.parent = parent.transform;
    }

    public void AnchorObject(Vector3 worldPos)
    {
        if (panel.activeSelf == false)
        {
            // Check if there is a GameObject at the specified position
            Collider[] colliders = Physics.OverlapSphere(worldPos, 0.1f);
            if (colliders.Length > 0)
            {
                foreach (Transform child in parent.transform)
                {
                    if (child.gameObject == colliders[0].gameObject)
                    {
                        Destroy(child.gameObject);
                    }
                }
                // Destroy the first GameObject found
            }
            else
            {
                GameObject newAnchor = new GameObject("NewAnchor");
                newAnchor.transform.position = worldPos;
                newAnchor.AddComponent<ARAnchor>();

                GameObject obj = Instantiate(prefabToAnchor, newAnchor.transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.parent = parent.transform;
            }

        }

    }
} 
*/