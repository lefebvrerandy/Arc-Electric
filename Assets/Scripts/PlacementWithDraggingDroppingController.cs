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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;


/*
*   NAME    : PlacementWithDraggingDroppingController
*   PURPOSE : This class is used to detect planes, place, orient, and delete objects according to the users input
*/
[RequireComponent(typeof(ARRaycastManager))]
public class PlacementWithDraggingDroppingController : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;


    private GameObject placedPrefab;
    private GameObject placedObject;
    private GameObject[] clickedObjects;
    private Vector2 touchPosition = default;
    private ARRaycastManager arRaycastManager;
    private bool onTouchHold = false;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private List<GameObject> LightList = new List<GameObject>();

    // We'll use these to get the current lights name
    private bool haveLight;
    private GameObject heldObject;

    // Used to keep the dropping of a light to once a second
    private bool canDropLight;

    private string selectedLight = "";

    //[SerializeField]
    //private Text DEBUGMENU;

    private int amountOfPlacedLights;

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

    private void Start()
    {
        // Default the users selected light to our basic light.

        if (PlayerPrefs.GetString("Selected") == "")
        {
            selectedLight = "Circle_lamp3 + Point Light 1";
        }
        else
        {
            selectedLight = PlayerPrefs.GetString("Selected");
        }

        canDropLight = true;
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

            /*
             * Three options here:
             * 1. The beginning of the touch
             *      During the beginning of the press, we will find out what the user if pressing. This has three options as well:
             *      a1. The user pressed a UI object
             *          Here, we will just return
             *      b2. The user is touching an already existing lamp
             *          Here, we will let the user drag and move the lamp until they end the touch
             *              We will need to get the location of the lamp
             *      c3. The user is trying to place a lamp
             *          Here, we will get the location they are trying to place
             * 2. The end of the touch
             *      During the end of the press, we have two options:
             *      a4. If the user is placing, place the object at the location we've stored
             *      b5. If the user is moving an object, we want to stop moving
             * 3. The user is moving an object
             *      Move the object
             */

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;

                if (Physics.Raycast(ray, out hitObject))
                {
                    //DEBUGMENU.text = hitObject.transform.name;
                    var selection = hitObject.transform;
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    selectionRenderer.material.color = Color.red;

                    /*
                     * Three options here. 
                     * a1. We can be touching an User Interface object
                     * b2. We can be touching an already existing lamp
                     * c3. We can be touching an empty plane
                     */

                    // Lets first check to see if the user is clicking on a UI element. 
                    //  If they are, lets break out of this method and not place the object
                    if ((hitObject.transform.CompareTag("UserInterface")))
                    //if (hitObject.transform.gameObject.CompareTag("UserInterface"))
                    {
                        //DEBUGMENU.text = "Touched UserInterface";
                        canDropLight = false;
                        return;
                    }
                    // If we are hitting an object
                    else if (hitObject.transform.name.Contains("lamp"))
                    {
                        //DEBUGMENU.text = "Touched Light";
                        canDropLight = false;
                        onTouchHold = true;

                        // We'll loop through the LightList and look for the selected light and store it
                        //  this will be used to move this specific light later
                        foreach (var item in LightList)
                        {
                            if (hitObject.transform.name == item.name)
                            {
                                heldObject = item;
                                haveLight = true;
                                break;
                            }
                        }
                    }
                    // If we are hitting an placable area
                    else if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                    {
                        //DEBUGMENU.text = "Touched Placeable Plane";
                        canDropLight = true;
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                heldObject = null;
                haveLight = false;
                onTouchHold = false;

                // If we are placing the object
                if (canDropLight)
                {
                    // Instantly trigger to false to avoid multiple dropping
                    canDropLight = false;


                    if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon)
                        && placedObject == null)
                    {
                        Pose hitPose = hits[0].pose;

                        // Start the timer to dropping the light
                        canDropLight = false;

                        // Get the selected light game object
                        if (PlayerPrefs.GetString("Selected") == "")
                        {
                            selectedLight = "Circle_lamp3 + Point Light 1";
                        }
                        else
                        {
                            selectedLight = PlayerPrefs.GetString("Selected");
                        }
                        placedPrefab = InventoryController.GetSelectedLight(selectedLight);

                        if (placedPrefab != null)
                        {
                            // Create a new Quaternion with the hitPose. This will be where the user clicks (MUST BE ON A PLANE FOR HIT TO REGISTER)
                            var placedLocation = new Quaternion(hitPose.rotation.x, hitPose.rotation.y, hitPose.rotation.z, hitPose.rotation.w);

                            amountOfPlacedLights++;

                            // Lets make sure the scale is set to 1, 1, 1 just incase it was messed up somewhere
                            placedPrefab.transform.localScale = new Vector3(1f, 1f, 1f);
                            placedPrefab.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                            placedObject = Instantiate(placedPrefab, hitPose.position, placedLocation);
                            placedObject.name = placedPrefab.name + amountOfPlacedLights;
                            placedObject.SetActive(true);
                            LightList.Add(placedObject);
                            placedObject = null;

                            // Destroy the returned object from InventoryController.GetSelectedLight() since we dont need it anymore
                            Destroy(placedPrefab);
                        }
                    }
                }
            }

            // This is used to move the object, but only if we are currently holding one
            if (onTouchHold
                && arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon)
                && Input.touchCount > 0)
            {
                Pose hitPose = hits[0].pose;
                if (heldObject != null && haveLight)
                {
                    heldObject.transform.position = hitPose.position;
                    heldObject.transform.rotation = hitPose.rotation;
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
        foreach (var item in LightList)
        {
            Destroy(item);
        }
        LightList.Clear();
        amountOfPlacedLights = 0;
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