/*
*  FILE         : RadialButton.cs
*  PROJECT      : PROG3220-Systems Project
*  PROGRAMMER   : Bence Karner
*  DESCRIPTION  : Sole responsibility is to contain the RadialButton class
*  REFERENCES   : The following code was taken from:
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
/// Controls how the button "animates" into the scene.
/// </summary>
public class RadialButton : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// UI Image element thats in the shape of a circle, and acts as a background of each menu item
    /// </summary>
    public Button button;

    /// <summary>
    /// Color of the menu items background
    /// </summary>
    public Image icon; 
    
    /// <summary>
    /// Reference to the radial menu where the button has been instantiated
    /// </summary>
    public RadialMenu parentMenu;

    /// <summary>
    /// Determines the length of time the button will "grow" until it reaches it's full size
    /// </summary>
    public float animSpeed;


    #endregion
    #region PublicMethods


    /// <summary>
    /// Activates the coroutine to animate the buttons appearance onto the screen
    /// </summary>
    public void Anim()
    {
        StartCoroutine(AnimateButtonIn());
    }


    #endregion
    #region PrivateMethods


    /// <summary>
    /// The buttons will "grow" in size, for the length of time specified by the AnimationLength property
    /// </summary>
    /// <returns></returns>
    private IEnumerator AnimateButtonIn()
    {
        //Scale the button down to 0 upon instantiation
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


    #endregion
}//class