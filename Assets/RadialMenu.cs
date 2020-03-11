/*
*  FILE         : RadialMenu.cs
*  PROJECT      : PROG3220-Systems Project
*  PROGRAMMER   : Bence Karner
*  DESCRIPTION  : Sole responsibility is to contain the RadialMenu class
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
    IEnumerator AnimateButtons (RadialMenuController rmc)
    {
        const int offsetDistance = 140;
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
    /// Activate the selected menu item, and delete the menu
    /// </summary>
    void Update()
    {
        //Respond to mouse up, or finger up event
        if (Input.GetMouseButtonUp(0))
        {
            var lightFixture = GameObject.FindGameObjectWithTag("SelectedLight");

            if (selectedButton)
            {
                var light = lightFixture.GetComponent<Light>();
                switch (selectedButton.title)
                {
                    case "ToggleLight":
                        light.enabled = !light.enabled;
                        break;

                    case "ChangeColor":
                        var colorpickerPrefab = Instantiate(Resources.Load("ColorPicker")) as GameObject;
                        var colorpickerComponent = colorpickerPrefab.GetComponent<ColorPicker>();
                        colorpickerComponent.parentObject = lightFixture;
                        break;

                    case "DeleteLight":
                        Destroy(lightFixture);
                        break;

                    case "AdjustRange":
                        //Display the range slider
                        //var sliderPrefab = Instantiate(Resources.Load("RangeSlider")) as GameObject;

                        ////Add the slider script to the light fixture
                        //var sliderScript = lightFixture.AddComponent(typeof(AdjustLightRange)) as AdjustLightRange;

                        //Register the OnValueChanged event of the slider to the lights range
                        //var sliderComponent = sliderPrefab.GetComponentInChildren<Slider>();
                        //sliderComponent.maxValue = 20;
                        //sliderComponent.minValue = 0;
                        //sliderComponent.value = light.range;
                        //sliderComponent.wholeNumbers = true;
                        //sliderComponent.onValueChanged.AddListener(sliderScript.AdjustRange);
                        break;

                    case "RANDY1":
                        Debug.Log("RANDY1 selected");
                        break;

                    case "RANDY2":
                        Debug.Log("RANDY2 selected");
                        break;

                    default:
                        break;
                }
            }
            Destroy(gameObject);

            if(lightFixture != null)
            {
                lightFixture.tag = "LightFixture";
            }
        }
    }
}