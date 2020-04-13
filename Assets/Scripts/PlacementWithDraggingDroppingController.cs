/*
*  FILE          : PlacementWithDraggingDroppingController.cs
*  PROJECT       : PROG 3220 - Systems Project
*  PROGRAMMER    : Randy Lefebvre, Bence Karner
*  DESCRIPTION   : This file contains the placement controller, and the relevant code to control the applications UI
*  REFERENCE     : Script format was copied and altered from the following sources,
*  Valecillos. (2019). AR Foundation with Unity3d and Adding Dragging Functionality with 
*       AR Raycast and Physics Raycast. Retrieved on January 15th, 2020, from 
*       https://www.youtube.com/watch?v=nBftG-gXUE8
*/


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;


/*
*   NAME    : PlacementWithDraggingDroppingController
*   PURPOSE : This class is used to detect planes, place, orient, and delete objects according to the users input
*/
[RequireComponent(typeof(ARRaycastManager))]
public class PlacementWithDraggingDroppingController : MonoBehaviour
{
#pragma warning disable 649

    #region Properties
    /// <summary>
    /// 
    /// </summary>
    public static bool EnableLightPlacement = true;

    /// <summary>
    /// 
    /// </summary>
    public static bool EnableLightDrag = true;

    /// <summary>
    /// 
    /// </summary>
    public ToastScript ToastMessage;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private Camera arCamera;

    /// <summary>
    /// 
    /// </summary>
    private GameObject placedObject;

    /// <summary>
    /// 
    /// </summary>
    private GameObject[] clickedObjects;

    /// <summary>
    /// 
    /// </summary>
    private Vector2 touchPosition = default;

    /// <summary>
    /// 
    /// </summary>
    private ARRaycastManager arRaycastManager;

    /// <summary>
    /// 
    /// </summary>
    private bool onTouchHold = false;

    /// <summary>
    /// 
    /// </summary>
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    /// <summary>
    /// 
    /// </summary>
    private static List<GameObject> LightList = new List<GameObject>();

    /// <summary>
    /// 
    /// </summary>
    // We'll use these to get the current lights name
    private bool haveLight;

    /// <summary>
    /// 
    /// </summary>
    private GameObject heldObject;

    /// <summary>
    /// 
    /// </summary>
    // Used to keep the dropping of a light to once a second
    private bool canDropLight;

    /// <summary>
    /// 
    /// </summary>
    private string selectedLight = "";

    /// <summary>
    /// 
    /// </summary>
    private int amountOfPlacedLights;

    /// <summary>
    /// 
    /// </summary>
    private bool flip = true;



    #endregion
    #region MonoBehaviours



    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }


    /// <summary>
    /// 
    /// </summary>
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


    /// <summary>
    /// 
    /// </summary>  
    private void Update()
    {

        if (Input.touchCount < 1)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);
        touchPosition = touch.position;

         /* Three options here:
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
                /*
                    * Three options here. 
                    * a1. We can be touching an User Interface object
                    * b2. We can be touching an already existing lamp
                    * c3. We can be touching an empty plane
                    */

                // Lets first check to see if the user is clicking on a UI element. 
                //  If they are, lets break out of this method and not place the object
                if (IsPointerOverUIObject())
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
                // If we are hitting a placeable area
                else if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    //DEBUGMENU.text = "Touched Placeable Plane";
                    canDropLight = true;
                }
            }
        }

        else if (touch.phase == TouchPhase.Ended)
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

                    // Get the selected light game object
                    if (PlayerPrefs.GetString("Selected") == "")
                    {
                        selectedLight = "Circle_lamp3 + Point Light 1";
                    }
                    else
                    {
                        selectedLight = PlayerPrefs.GetString("Selected");
                    }
                    Tuple<GameObject, string> foundTupleLightFolder = InventoryController.GetSelectedLight(selectedLight);
                    GameObject placedPrefab = foundTupleLightFolder.Item1;
                    if (placedPrefab != null)
                    {
                        Debug.Log(
                            "Rotation = X: " + hitPose.rotation.x
                            + "|Y: " + hitPose.rotation.y
                            + "|Z: " + hitPose.rotation.z
                            + "|W: " + hitPose.rotation.w);

                        // Create a new Quaternion with the hitPose. This will be where the user clicks (MUST BE ON A PLANE FOR HIT TO REGISTER)
                        var placedLocation = new Quaternion(hitPose.rotation.x, hitPose.rotation.y, hitPose.rotation.z - 180, hitPose.rotation.w);

                        amountOfPlacedLights++;

                        // Lets make sure the scale is set to 1, 1, 1 just in case it was messed up somewhere
                        placedPrefab.transform.localScale = new Vector3(1f, 1f, 1f);
                        placedObject = Instantiate(placedPrefab, hitPose.position, placedLocation);
                        placedObject.name = placedPrefab.name + amountOfPlacedLights;
                        placedObject.layer = 8; // This is the Light Layer

                        // Determine if we are placing on the ceiling or floor
                        bool needToDestroy = false;
                        string invalidString = string.Empty;
                        switch (foundTupleLightFolder.Item2)
                        {
                            case "Ceiling":
                                if (hitPose.rotation.y == 0 && hitPose.rotation.w == 0)
                                {
                                    // Ceiling
                                    placedObject.transform.eulerAngles = new Vector3(0f, -180f, 0f);
                                }
                                else if (hitPose.rotation.x == 0 && hitPose.rotation.z == 0)
                                {
                                    // Floor
                                    //placedObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                                }
                                else
                                {
                                    // Wall - Dont place ceiling lights on the wall
                                    needToDestroy = true;
                                    invalidString = "Cannot place CEILING LAMP on the WALL";
                                }
                                break;


                            case "Floor":
                                if (hitPose.rotation.y == 0 && hitPose.rotation.w == 0)
                                {
                                    // Ceiling
                                    needToDestroy = true;
                                    invalidString = "Cannot place FLOOR LAMP on the CEILING";
                                }
                                else if (hitPose.rotation.x == 0 && hitPose.rotation.z == 0)
                                {
                                    // Floor
                                    //placedObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                                    placedObject.transform.eulerAngles = new Vector3(0f, -180f, 0f);
                                }
                                else
                                {
                                    // Wall - Dont place floor lights on the wall
                                    needToDestroy = true;
                                    invalidString = "Cannot place FLOOR LAMP on the WALL";
                                }
                                break;


                            case "Wall":
                                if (hitPose.rotation.y == 0 && hitPose.rotation.w == 0)
                                {
                                    // Ceiling
                                    needToDestroy = true;
                                    invalidString = "Cannot place WALL LAMP on the CEILING";
                                }
                                else if (hitPose.rotation.x == 0 && hitPose.rotation.z == 0)
                                {
                                    // Floor
                                    needToDestroy = true;
                                    invalidString = "Cannot place WALL LAMP on the FLOOR";
                                }
                                else
                                {
                                    // Wall
                                    placedObject.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                                }
                                break;


                            default:
                                if (hitPose.rotation.y == 0 && hitPose.rotation.w == 0)
                                {
                                    // Ceiling
                                    placedObject.transform.eulerAngles = new Vector3(0f, -180f, 0f);
                                }
                                else if (hitPose.rotation.x == 0 && hitPose.rotation.z == 0)
                                {
                                    // Floor
                                }
                                else
                                {
                                    // Wall
                                }
                                break;
                        }

                        if (!needToDestroy)
                        {
                            placedObject.SetActive(true);
                            LightList.Add(placedObject);
                            placedObject = null;
                        }
                        else
                        {
                            ToastMessage.showToast(invalidString, 3);
                            Destroy(placedObject);
                        }


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
                if (hitPose.rotation.y == 0 && hitPose.rotation.x != 0)
                {
                    Debug.Log("Ceiling");
                    placedObject.transform.eulerAngles = new Vector3(0f, -180f, 0f);
                }
                else
                {
                    Debug.Log("Floor");
                    //placedObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                }
            }
        }
    }


    #endregion
    #region PublicMethods


    /// <summary>
    /// Delete every instantiated light in the scene
    /// </summary>
    public void DeleteLight()
    {
        foreach (var item in LightList)
        {
            Destroy(item);
        }
        LightList.Clear();
        amountOfPlacedLights = 0;
    }


    /// <summary>
    /// Flip the most recently placed light 180 deg along it's Z axis
    /// </summary>
    public void FlipObject()
    {
        if (placedObject != null)
        {
            placedObject.transform.RotateAround(placedObject.transform.position, Vector3.forward, 180f);
        }
    }

    /// <summary>
    /// This method is called in RadialMenu.cs and is called when the radialMenu is deleting a specific
    /// light. We call this method to insure that the list is also up to date
    /// </summary>
    /// <param name="lightToDelete"> Object to Destroy</param>
    public static void DeleteLight(GameObject lightToDelete)
    {
        LightList.Remove(lightToDelete);
    }


    #endregion
    #region PrivateMethods


    /// <summary>
    /// Deactivates the selected object
    /// </summary>
    /// <param name="clickedObject"> Object to deactivate</param>
    private void ToggleLight(GameObject clickedObject)
    {
        clickedObject.SetActive(false);
    }


    /// <summary>
    /// Checks if the users last click location was on a UI element
    /// References: 
    /// Fabian-mkv. (2015). IsPointerOverGameObject not working with touch input. Retrieved April 6, 2020, from
    /// https://answers.unity.com/questions/1115464/ispointerovergameobject-not-working-with-touch-inp.html
    /// https://drive.google.com/file/d/0B__1zp7jwQOKNVhFaGxhbWt5TVU/view
    /// </summary>
    /// <returns>True if the user click on a UI element, false otherwise </returns>
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


    #endregion
#pragma warning restore 649
}//class