using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
*   NAME    : RadialMenuSpawner
*   PURPOSE : 
*/
public class RadialMenuSpawner : MonoBehaviour
{
    public static RadialMenuSpawner instance;   //Singleton class
    public RadialMenu radialMenuPrefab;         //Menu that will be spawned


    
    private void Awake()
    {
        instance = this;
    }


    public void SpawnMenu(RadialMenuController menuController)
    {
        //Create the radial menu
        RadialMenu newMenu = Instantiate(radialMenuPrefab) as RadialMenu;

        //Disable the worldPositionStays option
        //     i.e. Do not allow the parent-relative position, scale and rotation to be modified such that
        //     the object keeps the same world space position, rotation and scale as before
        newMenu.transform.SetParent(transform, false);

        //Place the menu at the users click location
        newMenu.transform.position = Input.mousePosition;
        newMenu.SpawnButtons(menuController);
    }
}
