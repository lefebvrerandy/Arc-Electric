using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

// Script format was copied and altered from:
//      https://www.youtube.com/watch?v=nBftG-gXUE8

public class PlacementWithDraggingDroppingController : MonoBehaviour
{
    public GameObject placedPrefab;
    [SerializeField]
    private Camera arCamera;

    private GameObject placedObject;
    private Vector2 touchPosition = default;
    private ARRaycastManager arRaycastManager;
    private bool onTouchHold = false;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();


    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
                    if (hitObject.transform.name.Contains("lamp"))
                    {
                        onTouchHold = true;
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                onTouchHold = false;
            }
        }

        if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon) && Input.touchCount > 0)
        {
            Pose hitPose = hits[0].pose;

            if (placedObject == null)
            {
                string selectedLight = "";
                if (PlayerPrefs.GetString("Selected") == "")
                    selectedLight = "lamp1";
                else
                    selectedLight = PlayerPrefs.GetString("Selected");

                switch (selectedLight)
                {
                    case "Light1":
                        selectedLight = "lamp1";
                        break;
                    case "Light2":
                        selectedLight = "lamp2";
                        break;
                    case "Light3":
                        selectedLight = "lamp3";
                        break;
                }

                //placedPrefab = Resources.Load<GameObject>("Lights/" + selectedLight);
                //if (placedPrefab != null)
                    placedObject = Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
            }
            else
            {
                if (onTouchHold)
                {
                    placedObject.transform.position = hitPose.position;
                    placedObject.transform.rotation = hitPose.rotation;
                }
            }
        }
    }

    public void DeleteLight()
    {
        if (placedPrefab != null)
            Destroy(placedObject);
    }
}
