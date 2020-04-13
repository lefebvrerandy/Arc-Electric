/*
*  FILE         : RadialMenu.cs
*  PROJECT      : PROG3220-Systems Project
*  PROGRAMMER   : Bence Karner
*  DESCRIPTION  : This file contains the RadialMenu class which is part of the radial menu system
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
/// Controls the location of where the radial menu buttons are instantiated, adds on click listeners to each menu button, and deletes the menu upon selection of a menu item
/// </summary>
public class RadialMenu : MonoBehaviour
{
#pragma warning disable 649

    #region Properties 


    /// <summary>
    /// Prefab used to instantiate each of the radial menu buttons
    /// </summary>
    public RadialButton buttonPrefab;


    /// <summary>
    /// Ref to the light property menu prefab
    /// </summary>
    [SerializeField]
    private GameObject LightDisplayMenu;


    /// <summary>
    /// Ref to the rotation menu prefab
    /// </summary>
    [SerializeField]
    private GameObject LightRotationMenu;


    /// <summary>
    /// Ref to the post processing menu prefab
    /// </summary>
    [SerializeField]
    private GameObject LightPostProcessingMenu;


    #endregion
    #region MonoBehaviours


    /// <summary>
    /// Get a ref to the prefabs for each menu item
    /// </summary>
    private void Start()
    {
        LightDisplayMenu = GameObject.Find("LightDisplayMenu");
        LightRotationMenu = GameObject.Find("LightRotationMenu");
        LightPostProcessingMenu = GameObject.Find("LightPostProcessingMenu");
    }


    #endregion
    #region PublicMethods


    /// <summary>
    ///  Begin the process of spawning menu items
    /// </summary>
    public void SpawnButtons()
    {
        StartCoroutine("ConfigureButtons");
    }


    #endregion
    #region PrivateMethods


    /// <summary>
    /// Spawn the menu items in a circle around the center of the menus position
    /// </summary>
    private IEnumerator ConfigureButtons ()
    {
        const int offsetDistance = 180;
        const float waitTime = 0.06f;
        int itemNumber = 0;

        var menuItems = RadialMenuSpawner.instance.menuItems;
        var lightFixture = RadialMenuSpawner.instance.SelectedLightFixture;
        var lightComponent = lightFixture.GetComponentInChildren<Light>();
        foreach (var item in menuItems)
        {
            RadialButton menuButton = Instantiate(buttonPrefab) as RadialButton;
            menuButton.transform.SetParent(transform, false);
            switch (item.title)
            {
                case "DeleteLight":
                    menuButton.button.onClick.AddListener(() => DestroyObject(lightFixture));
                    break;

                case "ToggleLight":
                    menuButton.button.onClick.AddListener(() => ToggleLight(lightComponent));
                    break;

                case "LightDisplayMenu":
                    menuButton.button.onClick.AddListener(() => OpenLightDisplayMenu(lightFixture, lightComponent));
                    break;

                case "LightPostProcessingMenu":
                    menuButton.button.onClick.AddListener(() => OpenLightPostProcessingMenu());
                    break;

                case "LightRotationMenu":
                    menuButton.button.onClick.AddListener(() => OpenLightRotationMenu(lightFixture));
                    break;

                case "FlipLight":
                    menuButton.button.onClick.AddListener(() => FlipLight(lightFixture));
                    break;

                default:
                    break;
            }


            //Get the angle of each menu item, in the circle
            float theta = (2 * Mathf.PI / menuItems.Length) * itemNumber++;
            float xPos = Mathf.Sin(theta);
            float yPos = Mathf.Cos(theta);


            //Adjust each items location in a circle based on the total count of items that will be displayed
            menuButton.transform.localPosition = new Vector3(xPos, yPos, 0f) * offsetDistance;


            //Style the buttons as set in the inspector of the radial menu spawner
            menuButton.icon.sprite = item.sprite;
            menuButton.parentMenu = this;
            menuButton.Anim();
            yield return new WaitForSeconds(waitTime);
        }
    }


    /// <summary>
    /// Destroy the selected object
    /// </summary>
    /// <param name="ObjectToDestroy"> Object that will be destroyed </param>
    private void DestroyObject(GameObject ObjectToDestroy)
    {
        RadialMenuSpawner.instance.menuOpen = false;
        PlacementWithDraggingDroppingController.DeleteLight(ObjectToDestroy);
        Destroy(ObjectToDestroy);
        Destroy(gameObject);
    }


    /// <summary>
    /// Toggle a light component ON or OFF 
    /// </summary>
    /// <param name="light"> The light component to toggle </param>
    private void ToggleLight(Light light)
    {
        RadialMenuSpawner.instance.menuOpen = false;
        light.enabled = !light.enabled;
        DestroyImmediate(gameObject);
    }


    /// <summary>
    /// Flip the game object 180 deg along its Z-axis (i.e. it toggles between 
    /// flipping the light on its head or its base)
    /// </summary>
    /// <param name="lightFixture">The game object to flip </param>
    private void FlipLight(GameObject lightFixture)
    {
        RadialMenuSpawner.instance.menuOpen = false;
        lightFixture.transform.RotateAround(lightFixture.transform.position, Vector3.forward, 180f);
        Destroy(gameObject);
    }


    /// <summary>
    /// Activate the LightDisplayMenu prefab
    /// </summary>
    /// <param name="lightComponent"> The light component that will be altered in the menu </param>
    private void OpenLightDisplayMenu(GameObject lightFixture, Light lightComponent)
    {
        //Update the selected light, and set the slider starting values
        var script = LightDisplayMenu.GetComponent<LightDisplayMenuScript>();
        script.Light = lightComponent;
        script.LightFixture = lightFixture;
        script.ConfigureSliders();

        //Display the menu, play the animation/tween, and destroy the radial menu
        var detectOutofBoundsPrefab = LightDisplayMenu.transform.GetChild(0).gameObject;
        var menuPanel = detectOutofBoundsPrefab.transform.GetChild(0).gameObject;
        LightDisplayMenu.GetComponent<Canvas>().enabled = true;
        LeanTween.moveY(menuPanel, 500, 0.8f).setEaseOutBack();
        Destroy(gameObject);
    }


    /// <summary>
    /// Activate the LightPostProcessingMenu prefab
    /// </summary>
    private void OpenLightPostProcessingMenu()
    {
        var detectOutofBoundsPrefab = LightPostProcessingMenu.transform.GetChild(0).gameObject;
        var menuPanel = detectOutofBoundsPrefab.transform.GetChild(0).gameObject;
        LightPostProcessingMenu.GetComponent<Canvas>().enabled = true;
        LeanTween.moveY(menuPanel, 560, 0.75f).setEaseOutBack();
        Destroy(gameObject);
    }


    /// <summary>
    /// Activate the LightRotationMenu prefab
    /// </summary>
    /// <param name="lightFixture"> Game object to rotate in the menu </param>
    private void OpenLightRotationMenu(GameObject lightFixture)
    {
        //Update the selected light and configure the UI elements
        var script = LightRotationMenu.GetComponent<LightRotationScript>();
        script.lightFixture = lightFixture;

        var orientationScript = lightFixture.GetComponent<Orientation>();
        script.SetPreviousOrientation(orientationScript.Angles);
        script.ConfigureSliders();


        //Get the screens bottom center point, and animate the panel upwards to be in view
        var detectOutofBoundsPrefab = LightRotationMenu.transform.GetChild(0).gameObject;
        var menuPanel = detectOutofBoundsPrefab.transform.GetChild(0).gameObject;
        LightRotationMenu.GetComponent<Canvas>().enabled = true;
        LeanTween.moveY(menuPanel, 500, 0.8f).setEaseOutBack();
        Destroy(gameObject);
    }


    #endregion
#pragma warning restore 649
}//class