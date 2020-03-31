/*
*  FILE         : RadialMenuSpawner.cs
*  PROJECT      : PROG3220-Systems Project
*  PROGRAMMER   : Bence Karner
*  DESCRIPTION  : Sole responsibility is to contain the RadialMenuSpawner class
*  REFERENCES   : The following code was taken from a series of online tutorials, and altered to suit the needs of the project
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 1) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=WzQdc2rAdZc
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 2) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=HOOGIZu4nxo
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 3) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=XvgUzjXW2Jk
*   Board To Bits Games. (Nov 6, 2015). Unity Tutorial: Radial Menu (Part 4) from Board to Bits [Video file]. Retrieved Feb 24, 2020, from https://www.youtube.com/watch?v=vPeCGO1miMk
*/

using System;
using UnityEngine;

/// <summary>
/// Singleton controller used to set the position of where the radial menu will spawn, and the displayed name of the menu
/// </summary>
public class RadialMenuSpawner : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the class
    /// </summary>
    public static RadialMenuSpawner instance;

    /// <summary>
    /// Game object that defines the structure of the menu
    /// </summary>
    public RadialMenu menuPrefab;


    /// <summary>
    /// Ensure the menu spawner only ever has a single instance
    /// </summary>
    private void Awake()
    {
        instance = this;
    }


    /// <summary>
    /// Create the menu, and attach it to the clicked game object
    /// </summary>
    /// <param name="rmc"> Menu controller attached to the selected game object</param>
    public void SpawnMenu(RadialMenuController rmc)
    {
         RadialMenu radialMenu = Instantiate(menuPrefab) as RadialMenu;

        //Disable the worldPositionStays option
        //     i.e. Do not allow the parent-relative position, scale and rotation to be modified such that
        //     the object keeps the same world space position, rotation and scale as before
        radialMenu.transform.SetParent(transform, false);

        //Set the menus starting position to point clicked by the user
        radialMenu.transform.position = Input.mousePosition;
        radialMenu.menuLabel.text = rmc.menuTitle.ToUpper();
        radialMenu.SpawnButtons(rmc);
    }
}
