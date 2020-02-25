using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*   NAME    : Interchangeable
*   PURPOSE : 
*/
public class RadialMenuController : MonoBehaviour
{
    

    [System.Serializable]
    public class MenuItem
    {
        public Color color;
        public Sprite sprite;
        public string title;
    }

    public MenuItem[] menuItems;

    void OnMouseDown()
    {
        //Tell canvas to spawn radial menu on mouse click
        RadialMenuSpawner.instance.SpawnMenu(this);
    }
}
