
//https://www.youtube.com/watch?v=WzQdc2rAdZc
//https://www.youtube.com/watch?v=HOOGIZu4nxo
//https://www.youtube.com/watch?v=XvgUzjXW2Jk

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RadialMenu : MonoBehaviour
{
    public RadialButton buttonPrefab;   //UI button to create when the menu is created
    public RadialButton selectedButton; //Tracks the selected button
    public int offsetDistance;

    public void  Awake()
    {
        //If not set in the inspector, default the offsetDistance
        if(offsetDistance == null || offsetDistance < 1)
        {
            offsetDistance = 150;
        }
    }

    public void SpawnButtons(RadialMenuController menuController)
    {
        //Create a new button for each of the defined menu items in the controller
        int itemNumber = 0;
        foreach (var item in menuController.menuItems)
        {
            RadialButton newButton = Instantiate(buttonPrefab) as RadialButton;
            newButton.transform.SetParent(transform, false);

            //Get the angle of each menu item, in the circle
            float theta = (2 * Mathf.PI / menuController.menuItems.Length) * itemNumber;
            float xPos = Mathf.Sin(theta);
            float yPos = Mathf.Cos(theta);

            //Our new button is currently set to the center of the menu (i.e. where the user clicked),
            //  so we're going to offset it's location to form a circle around the click location
            newButton.transform.localPosition = new Vector3(xPos, yPos, 0f) * offsetDistance;

            //Style the buttons as set in the inspector for the menuController
            newButton.background.color = item.color;
            newButton.icon.sprite = item.sprite;
            newButton.title = item.title;

            //Associate the button to the menu where it was created
            newButton.parentMenu = this;

            //Increment to the new menu item
            itemNumber++;
        }
    }


    private void Update ()
    {

        //Once the left mouse button is released, delete the radial menu
        if(Input.GetMouseButtonUp(0))
        {
            Destroy(gameObject);
        }
    }
}
