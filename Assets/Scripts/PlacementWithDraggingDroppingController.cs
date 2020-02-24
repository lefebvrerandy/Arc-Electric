using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

// Script format was copied and altered from:
//      https://www.youtube.com/watch?v=nBftG-gXUE8

public class PlacementWithDraggingDroppingController : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;


    private GameObject placedPrefab;
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
        //if (true)
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

                placedPrefab = Resources.Load<GameObject>("Lights/" + selectedLight);


                if (placedPrefab != null)
                {
                    var placedLocation = new Quaternion(hitPose.rotation.x, hitPose.rotation.y, hitPose.rotation.z, hitPose.rotation.w);
                    placedObject = Instantiate(placedPrefab, hitPose.position, placedLocation);
                    //var placedLocation = new Quaternion(hitPose.rotation.x, hitPose.rotation.y, hitPose.rotation.z - 180, hitPose.rotation.w);
                    //placedObject = Instantiate(placedPrefab, hitPose.position, placedLocation);
                }
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
        if (placedObject != null)
            Destroy(placedObject);
    }

    public void FlipObject()
    {
        if (placedObject != null)
        {
            placedObject.transform.rotation = new Quaternion(placedObject.transform.rotation.x, placedObject.transform.rotation.y, placedObject.transform.rotation.z - 180, placedObject.transform.rotation.w);
        }
    }
}
