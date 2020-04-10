/*
*  FILE         : RadialMenuSpawner.cs
*  PROJECT      : PROG3220-Systems Project
*  PROGRAMMER   : Bence Karner
*  DESCRIPTION  : Sole responsibility is to contain the RadialMenuSpawner class
*  REFERENCES   : The following code was adapted from:
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 1) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=WzQdc2rAdZc
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 2) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=HOOGIZu4nxo
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 3) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=XvgUzjXW2Jk
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 4) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=vPeCGO1miMk
*/

using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Controller class for determining double taps, spawning the radial menu, it's buttons, and setting all related properties.
/// </summary>
public class RadialMenuSpawner : MonoBehaviour
{
    #region Properties


    /// <summary>
    /// Defines the visual properties of each menu item
    /// </summary>
    [Serializable]
    public class MenuItem
    {
        public Sprite sprite;
        public string title;
    }

    /// <summary>
    /// Singleton instance of the class
    /// </summary>
    public static RadialMenuSpawner instance;
    
    /// <summary>
    /// Ref to the double tapped light object
    /// </summary>
    public GameObject SelectedLightFixture;
    
    /// <summary>
    /// Disallows the placement of lights, and drag & drop while the menu is open
    /// </summary>
    public bool menuOpen;

    /// <summary>
    /// Menu items set in the inspector
    /// </summary>
    public MenuItem[] menuItems;

    /// <summary>
    /// Game object that defines the structure of the menu
    /// </summary>
    public RadialMenu menuPrefab;

    /// <summary>
    /// Used for detecting double taps. Keeps track of how long
    /// the user kept their finger on the screen
    /// </summary>
    private float touchDuration;

    /// <summary>
    /// Reference to the most recent touch on the screen
    /// </summary>
    private Touch touch;


    #endregion
    #region MonoBehaviours


    /// <summary>
    /// Ensure the menu spawner only ever has a single instance
    /// </summary>
    private void Awake()
    {
        instance = this;
        SelectedLightFixture = null;
        menuOpen = false;
        touchDuration = 0f;
    }


    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        if (Input.touchCount < 1)
        {
            touchDuration = 0f;
            return;
        }
      
        touchDuration += Time.deltaTime;
        touch = Input.GetTouch(0);

        //Only check for a double tap if the user isn't pressing/dragging their finger
        //  After the tapDurationThreshdold, the touch is considered a press/drag action
        const float tapDurationThreshdold = 0.2f;
        if (touch.phase == TouchPhase.Ended && touchDuration < tapDurationThreshdold)
        {
            StartCoroutine("SingleOrDoubleTap");
        }
    }


    #endregion
    #region PublicMethods
    #endregion
    #region PrivateMethods


    /// <summary>
    /// Determines if the user did a single, or double tap action
    /// Reference:
    /// ben-rasooli[username]. (2014). Single Tap/Double Tap script. Retrieved March 31, 2020, from
    /// https://forum.unity.com/threads/single-tap-double-tap-script.83794/
    /// </summary>
    private IEnumerator SingleOrDoubleTap()
    {

        //Wait for the specified amount of time and check the number of taps
        const float timeBetweenTaps = 0.3f;
        yield return new WaitForSeconds(timeBetweenTaps);


        if (touch.tapCount == 2)
        {
            //The coroutine has been called twice now, so stop it from being 
            //  called again before we register two double taps
            StopCoroutine("SingleOrDoubleTap");

            if(!menuOpen)
            {
                if (touch.phase == TouchPhase.Ended)
                {
                    //Raycast at the location of the touch
                    Ray touchRay = GenerateTouchRay(touch.position);
                    RaycastHit hit;

                    ////We want to raycast to only the objects in the "Light" layer, which is set to layer 8
                    ////  so set the layer mask parameter to true for layer 8 (i.e. true = 1)
                    ////Raycast out from the last touch position, and check if any objects on layer 8 are hit
                    if (Physics.Raycast(touchRay.origin, touchRay.direction, out hit, Mathf.Infinity, 1 << 8))
                    {
                        //Get a reference to the touched object
                        SelectedLightFixture = hit.transform.gameObject;
                        SpawnMenu();
                        PlacementWithDraggingDroppingController.EnableLightPlacement = false;
                        PlacementWithDraggingDroppingController.EnableLightDrag = false;
                    }
                }
            }
        }
    }


    /// <summary>
    /// Instantiate the radial menu prefab, along with the buttons for each menu item defined in the inspector
    /// </summary>
    private void SpawnMenu()
    {
        menuOpen = true;
        RadialMenu radialMenu = Instantiate(menuPrefab) as RadialMenu;

        //Disable the worldPositionStays option
        //     i.e. Do not allow the parent-relative position, scale and rotation to be modified such that
        //     the object keeps the same world space position, rotation and scale as before
        radialMenu.transform.SetParent(transform, false);
        radialMenu.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        radialMenu.SpawnButtons();

        //Spawn a model of the selected light as a preview

    }


    /// <summary>
    /// Creates a ray out from the touch position. Works in both perspective, and orthographic camera view modes
    /// Reference:
    /// Holistic3d. (2016). Unity Mobile Dev From Scratch: Understanding Screen and World Coordinates for Raycasting. Retrieved March 13, 2020, 
    ///     from https://www.youtube.com/watch?v=7QomGnOyQoY&list=PLi-ukGVOag_1lNphWV5S-xxWDe3XANpyE&index=5
    /// Holistic3d. (2016). Unity Mobile From Scratch: TouchPhases and Touch Count. Retrieved March 13, 2020, 
    ///     from https://www.youtube.com/watch?v=ay9bbWJQ01w
    /// </summary>
    /// <param name="touchPos">Position of the touch on the screen</param>
    /// <returns>Ray struct with the origin, and direction of the touch in the world view</returns>
    private Ray GenerateTouchRay(Vector3 touchPos)
    {
        Vector3 touchPosFar = new Vector3(touchPos.x, touchPos.y, Camera.main.farClipPlane);
        Vector3 touchPosNear = new Vector3(touchPos.x, touchPos.y, Camera.main.nearClipPlane);
        Vector3 touchPosF = Camera.main.ScreenToWorldPoint(touchPosFar);
        Vector3 touchPosN = Camera.main.ScreenToWorldPoint(touchPosNear);
        Ray touchRay = new Ray(touchPosN, touchPosF - touchPosN);
        return touchRay;
    }


    #endregion
}//class