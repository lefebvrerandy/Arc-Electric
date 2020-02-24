/*
*  FILE          : PlacementWithDraggingDroppingController.cs
*  PROJECT       : PROG 3220 - Systems Project
*  PROGRAMMER    : Randy Lefebvre, Bence Karner, Lucas Roes, Kyle Horsley
*  DESCRIPTION   : This file contains the placement controller, and the relevant code to control the applications UI
*  REFERENCE     : Script format was copied and altered from the following sources,
*  Valecillos. (2019). AR Foundation with Unity3d and Adding Dragging Functionality with 
*       AR Raycast and Physics Raycast. Retrieved on January 15th, 2020, from 
*       https://www.youtube.com/watch?v=nBftG-gXUE8
*/


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


/*
*   NAME    : PlacementWithDraggingDroppingController
*   PURPOSE : This class is used to detect planes, place, orient, and delete objects according to the users input
*/
[RequireComponent(typeof(ARRaycastManager))]
public class PlacementWithDraggingDroppingController : MonoBehaviour
{
    #region Properties

    public GameObject placedPrefab;

    [SerializeField]
    private Camera arCamera;
    private GameObject placedObject;
    private GameObject[] clickedObjects;
    private Vector2 touchPosition = default;
    private ARRaycastManager arRaycastManager;
    private bool onTouchHold = false;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    #endregion


    /*
    *  METHOD       : Awake
    *  DESCRIPTION  : Get a reference to the ARRaycastManager component of the AR Session Origin object
    *  PARAMETER    : NA
    *  RETURNS      : NA
    */
    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }


    /*
    *  METHOD       : Update
    *  DESCRIPTION  : For every update, get the users input, and either place, orient, or delete an AR object according to their actions
    *  PARAMETER    : NA
    *  RETURNS      : NA
    */
    private void Update()
    {
        //Check if the user touched the screen
        if (Input.touchCount > 0)
        {
            //Get reference to the touch
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
                    var selection = hitObject.transform;
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    selectionRenderer.material.color = Color.red;
                    //var clickedObject = hitObject.transform.GetComponent<GameObject>();
                    //bool lightSelected = clickedObject != null && clickedObject.tag == "LightFixture";
                    //if (clickedObject != null)
                    //{
                    //    ToggleLight(clickedObject);
                    //}

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
                {
                    selectedLight = "lamp1";
                }
                else
                {
                    selectedLight = PlayerPrefs.GetString("Selected");
                }

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

                    
                    //DEBUG BRING UP THE RADIAL MENU FOR THE SELECTED OBJECT
                }
            }
        }
    }


    /*
    *  METHOD       : ToggleLight
    *  DESCRIPTION  : Toggles the AR light on or off when clicked by the user
    *  PARAMETER    : NA
    *  RETURNS      : NA
    */
    private void ToggleLight(GameObject clickedObject)
    {
        //var lights = clickedObject.GetComponentsInChildren(typeof(Light));
        clickedObject.SetActive(false);
        //if (clickedObject.activeSelf == true )
        //{
        //    clickedObject.SetActive(false);
        //}
        //else
        //{
        //    clickedObject.SetActive(true);
        //}
    }


    /*
    *  METHOD       : DeleteLight
    *  DESCRIPTION  : Removes the placed light from the scene
    *  PARAMETER    : NA
    *  RETURNS      : NA
    */
    public void DeleteLight()
    {
        if (placedObject != null)
        {
            Destroy(placedObject);
        }
    }


    /*
    *  METHOD       : FlipObject
    *  DESCRIPTION  : Rotates the orientation of the placed object along it's Z axis by 180 deg
    *  PARAMETER    : NA
    *  RETURNS      : NA
    */
    public void FlipObject()
    {
        if (placedObject != null)
        {
            placedObject.transform.rotation = new Quaternion(placedObject.transform.rotation.x, placedObject.transform.rotation.y, placedObject.transform.rotation.z - 180, placedObject.transform.rotation.w);
        }
    }

}//class