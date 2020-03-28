/*
*  FILE         : RadialMenuController.cs
*  PROJECT      : PROG3220-Systems Project
*  PROGRAMMER   : Bence Karner
*  DESCRIPTION  : Sole responsibility is to contain the RadialMenuController class
*  REFERENCES   : The following code was taken from a series of online tutorials, and altered to suit the needs of the project
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 1) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=WzQdc2rAdZc
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 2) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=HOOGIZu4nxo
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 3) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=XvgUzjXW2Jk
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 4) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=vPeCGO1miMk
*/

using System;
using UnityEngine;

/// <summary>
/// Allows the user to define the visual properties of each menu item in the inspector. Also allows the user
/// to spawn a radial menu on all interactable objects with this script attached. 
/// </summary>
public class RadialMenuController : MonoBehaviour
{
    public GameObject parentGameObject = null;
    public bool menuEnabled = true;
    public bool mouseEnabled = true;
    public bool touchEnabled = false;
    private bool lightSelected = false;

    /// <summary>
    /// Defines the visual properties of each menu item
    /// </summary>
    [Serializable]
    public class MenuItem
    {
        public Color color;
        public Sprite sprite;
        public string title;
    }

    /// <summary>
    /// Title/label text to show in the center of the menu
    /// </summary>
    public string menuTitle;

    /// <summary>
    /// Menu items set in the inspector
    /// </summary>
    public MenuItem[] menuItems;

    /// <summary>
    /// Ensure the menu's text element always has a title 
    /// </summary>
    void Start()
    {
        if(menuTitle == string.Empty || menuTitle == null)
        {
            menuTitle = "Settings";
        }
    }

    /// <summary>
    /// Mark the parent object as selected, and spawn the radial menu when the item is clicked
    /// </summary>
    private void OnMouseDown()
    {
        if(!menuEnabled || !mouseEnabled)
        {
            return;
        }
        
        parentGameObject.tag = "SelectedLight";
        RadialMenuSpawner.instance.SpawnMenu(this);
    }


    /// <summary>
    /// Check for touches, and bring up the radial menu when the user clicks on the AR lights
    /// </summary>
    public void Update()
    {
        if (!menuEnabled || !touchEnabled)
        {
            return;
        }

        //Was the screen touched?
        if (Input.touchCount < 1)
        {
            return;
        }

        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            //Raycast at the location of the touch
            Ray touchRay = GenerateTouchRay(Input.GetTouch(0).position);
            RaycastHit hit;

            //Determine if an object was hit by the raycast
            if (Physics.Raycast(touchRay.origin, touchRay.direction, out hit))
            {
                //Get a reference to the touched object
                parentGameObject = hit.transform.gameObject;
                parentGameObject.tag = "SelectedLight";
                RadialMenuSpawner.instance.SpawnMenu(this);
            }
        }

        //Dragging finger across the screen
        else if (touch.phase == TouchPhase.Moved && parentGameObject != null)
        {

        }

        //No longer touching the screen
        else if (touch.phase == TouchPhase.Ended && parentGameObject != null)
        {
            parentGameObject = null;
        }
    }


    //Creates a ray out from the touch position. Works in both perspective, and orthographic camera view modes
    //Holistic3d. (2016). Unity Mobile Dev From Scratch: Understanding Screen and World Coordinates for Raycasting. Retrieved March 13, 2020, from https://www.youtube.com/watch?v=7QomGnOyQoY&list=PLi-ukGVOag_1lNphWV5S-xxWDe3XANpyE&index=5
    //Holistic3d. (2016). Unity Mobile From Scratch: TouchPhases and Touch Count. Retrieved March 13, 2020, from https://www.youtube.com/watch?v=ay9bbWJQ01w
    private Ray GenerateTouchRay(Vector3 touchPos)
    {
        Vector3 touchPosFar = new Vector3(touchPos.x, touchPos.y, Camera.main.farClipPlane);
        Vector3 touchPosNear = new Vector3(touchPos.x, touchPos.y, Camera.main.nearClipPlane);
        Vector3 touchPosF = Camera.main.ScreenToWorldPoint(touchPosFar);
        Vector3 touchPosN = Camera.main.ScreenToWorldPoint(touchPosNear);
        Ray touchRay = new Ray(touchPosN, touchPosF - touchPosN);
        return touchRay;
    }
}