/*
*  FILE         : RadialMenu.cs
*  PROJECT      : PROG3220-Systems Project
*  PROGRAMMER   : Bence Karner
*  DESCRIPTION  : This file contains the RadialMenu class
*  REFERENCES   : The following code was taken from a series of online tutorials, and altered to suit the needs of the project
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 1) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=WzQdc2rAdZc
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 2) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=HOOGIZu4nxo
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 3) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=XvgUzjXW2Jk
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 4) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=vPeCGO1miMk
*/

using RuntimeInspectorNamespace;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// Controls the location of where the radial menu buttons are instantiated, and deletes the menu items when they're no longer selected
/// </summary>
public class RadialMenu : MonoBehaviour
{

    /// <summary>
    /// UI text element that will display the name of the menu
    /// </summary>
    public Text menuLabel;

    /// <summary>
    /// Game object to instantiate as a menu item
    /// </summary>
    public RadialButton buttonPrefab;

    /// <summary>
    /// Reference to the currently selected button in the menu
    /// </summary>
    public RadialButton selectedButton;

    /// <summary>
    ///  Begin the process of spawning menu items
    /// </summary>
    /// <param name="rmc"> Activated instance of the menu controller </param>
    public void SpawnButtons(RadialMenuController rmc)
    {
        StartCoroutine(AnimateButtons(rmc));
    }

    /// <summary>
    /// Spawn the menu items in a circle around the point that was clicked on the game object
    /// </summary>
    /// <param name="rmc"> Activated instance of the menu controller </param>
    private IEnumerator AnimateButtons (RadialMenuController rmc)
    {
        const int offsetDistance = 155;
        const float waitTime = 0.06f;
        int itemNumber = 0;
        foreach (var item in rmc.menuItems)
        {
            RadialButton menuButton = Instantiate(buttonPrefab) as RadialButton;
            menuButton.transform.SetParent(transform, false);

            //Get the angle of each menu item, in the circle
            float theta = (2 * Mathf.PI / rmc.menuItems.Length) * itemNumber++;
            float xPos = Mathf.Sin(theta);
            float yPos = Mathf.Cos(theta);

            //Adjust each items location in a circle based on the total count of items that will be displayed
            menuButton.transform.localPosition = new Vector3(xPos, yPos, 0f) * offsetDistance;

            //Style the buttons as set in the inspector for the menuController
            menuButton.circle.color = item.color;
            menuButton.icon.sprite = item.sprite;
            menuButton.title = item.title;
            menuButton.parentMenu = this;
            menuButton.Anim();
            yield return new WaitForSeconds(waitTime);
        }
    }

    /// <summary>
    /// Performs the following tasks: 
    /// *   Activates the selected menu item
    /// *   Disables the radial menu until the selected item is closed
    /// *   Deletes the menu upon releasing the button
    /// *   Reverts the selected lights tag
    /// </summary>
    private void Update()
    {
        //Was the screen touched?
        if (Input.touchCount < 1)
        {
            return;
        }

        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Ended)
        {
            var lightFixture = GameObject.FindGameObjectWithTag("SelectedLight");
            RadialMenuController radialMenuController = lightFixture.GetComponent<RadialMenuController>() as RadialMenuController;
            if (selectedButton)
            {
                var light = lightFixture.GetComponentInChildren<Light>();
                switch (selectedButton.title)
                {
                    case "ToggleLight":
                        light.enabled = !light.enabled;
                        break;

                    case "ChangeColor":
                        var colorpickerPrefab = Instantiate(Resources.Load("ColorPicker")) as GameObject;

                        //Disable the radial menu until the window is closed
                        var menuSignalScript = colorpickerPrefab.GetComponent<MenuSignal>();
                        radialMenuController.menuEnabled = false;
                        menuSignalScript.rmc = radialMenuController;


                        //Set the light to change in the submenu
                        var colorpickerComponent = colorpickerPrefab.GetComponent<ColorPicker>();
                        colorpickerComponent.parentObject = lightFixture;
                        break;

                    case "DeleteLight":
                        Destroy(lightFixture);
                        break;

                    case "AdjustRange":
                        var lraPrefab = Instantiate(Resources.Load("LightRangeAdjuster")) as GameObject;

                        //Disable the radial menu until the window is closed
                        menuSignalScript = lraPrefab.GetComponent<MenuSignal>();
                        radialMenuController.menuEnabled = false;
                        menuSignalScript.rmc = radialMenuController;


                        //Set the light to change its range with the slider component
                        var rangeScript = lraPrefab.GetComponent<AdjustLightRange>();
                        rangeScript.lightFixture = lightFixture;
                        break;

                    case "FlipLight":
                        lightFixture.transform.rotation = new Quaternion(lightFixture.transform.rotation.x, lightFixture.transform.rotation.y, lightFixture.transform.rotation.z - 180, lightFixture.transform.rotation.w);
                        break;

                    case "CameraOptions":
                        var postProcessingPrefab = Instantiate(Resources.Load("PostProcessingAdjuster")) as GameObject;

                        //Disable the radial menu until the window is closed
                        menuSignalScript = postProcessingPrefab.GetComponent<MenuSignal>();
                        radialMenuController.menuEnabled = false;
                        menuSignalScript.rmc = radialMenuController;

                        var ARCamera = GameObject.FindGameObjectWithTag("MainCamera");
                        var postProcessVolume = ARCamera.GetComponent<PostProcessVolume>();

                        //Set the lights to change when any of the three post processing effect sliders are changed
                        var bloomSliderScript = postProcessingPrefab.GetComponent<AdjustBloom>();
                        bloomSliderScript.ppv = postProcessVolume;

                        var ambientOcclusionSliderScript = postProcessingPrefab.GetComponent<AdjustAmbientOcclusion>();
                        ambientOcclusionSliderScript.ppv = postProcessVolume;
                        break;

                    default:
                        break;
                }
            }
            Destroy(gameObject);

            if (lightFixture != null)
            {
                lightFixture.tag = "LightFixture";
            }
        }
    }
}