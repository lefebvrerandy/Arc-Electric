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
    public GameObject parentGameObject;

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

        if(parentGameObject == null)
        {
            parentGameObject = gameObject;
        }
    }

    /// <summary>
    /// Mark the parent object as selected, and spawn the radial menu when the item is clicked
    /// </summary>
    private void OnMouseDown()
    {
        parentGameObject.tag = "SelectedLight";
        RadialMenuSpawner.instance.SpawnMenu(this);
    }
}