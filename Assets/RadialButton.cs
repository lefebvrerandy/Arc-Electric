/*
*  FILE         : RadialButton.cs
*  PROJECT      : PROG3220-Systems Project
*  PROGRAMMER   : Bence Karner
*  DESCRIPTION  : Sole responsibility is to contain the RadialButton class
*  REFERENCES   : The following code was taken from a series of online tutorials, and altered to suit the needs of the project
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 1) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=WzQdc2rAdZc
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 2) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=HOOGIZu4nxo
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 3) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=XvgUzjXW2Jk
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 4) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=vPeCGO1miMk
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Used to define the properties and behavior of the game objects assigned as buttons in the radial menu. 
/// Responds to pointer enters/exits events, and controls how the button "animates" into the scene.
/// </summary>
public class RadialButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Properties
    /// <summary>
    /// UI Image element thats in the shape of a circle, and acts as a background of each menu item
    /// </summary>
    public Image circle; 
    
    /// <summary>
    /// Color of the menu items background
    /// </summary>
    public Image icon; 
    
    /// <summary>
    /// Name of the menu item
    /// </summary>
    public string title; 
    
    /// <summary>
    /// Reference to the radial menu where the button has been instantiated
    /// </summary>
    public RadialMenu parentMenu;

    /// <summary>
    /// Determines the length of time the button will "grow" until it reaches it's full size
    /// </summary>
    public float animSpeed;

    /// <summary>
    /// Default color of the buttons background, set in the OnPointerEnter, and OnPointerExit events
    /// </summary>
    private Color defaultColor;
    #endregion


    /// <summary>
    /// Activates the coroutine to animate the buttons appearance onto the screen
    /// </summary>
    public void Anim()
    {
        StartCoroutine(AnimateButtonIn());
    }


    /// <summary>
    /// The buttons will "grow" in size, for the length of time specified by the AnimationLength property
    /// </summary>
    /// <returns></returns>
    IEnumerator AnimateButtonIn()
    {
        //Default the buttons size to zero
        transform.localScale = Vector3.zero;
        
        
        float timer = 0f;
        while(timer < (1 / animSpeed))
        {
            //Track elapsed time
            timer += Time.deltaTime;

            //Increase the buttons size proportional to the time passed
            // i.e. Button will be X% of full size once X% of the animation time has passed
            transform.localScale = Vector3.one * timer * animSpeed;
            yield return null;
        }

        //Maximize the button to full size once the animation time is up
        transform.localScale = Vector3.one;
    }


    /// <summary>
    /// When the pointer goes over the button, set it as the selected item
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        parentMenu.selectedButton = this;
        defaultColor = circle.color;
        circle.color = Color.white;
    }


    /// <summary>
    /// When leaving the buttons space it is no longer labeled as selected
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        //parentMenu.selectedButton = null;
        circle.color = defaultColor;
    }
}