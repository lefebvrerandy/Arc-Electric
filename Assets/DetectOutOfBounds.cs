/*
*  FILE         : DetectOutOfBounds.cs
*  PROJECT      : PROG3220 - Systems Project
*  PROGRAMMER   : Bence Karner
*  DESCRIPTION  : Contains the DetectOutOfBounds class
*/

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Contains events for detecting if the user clicked on a UI element.
/// </summary>
public class DetectOutOfBounds : MonoBehaviour, IPointerClickHandler
{

    /// <summary>
    /// Time, in seconds, to wait until the user can place and move lights again
    /// </summary>
    [SerializeField]  private float waitTimeSeconds;


    /// <summary>
    /// Event that fires once the user clicks outside on the game object attached to this script. 
    /// Gets the parent of the object, and deactivate it.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        RadialMenuSpawner.instance.menuOpen = false;
        gameObject.transform.parent.gameObject.GetComponent<Canvas>().enabled = false;
        StartCoroutine(EnablePlacementAndDragnDrop());
    }


    /// <summary>
    /// Waits for the specified amount of time, and then toggles the placement, and DragnDrop feature on again
    /// </summary>
    private IEnumerator EnablePlacementAndDragnDrop()
    {
        yield return new WaitForSeconds(waitTimeSeconds);
        PlacementWithDraggingDroppingController.EnableLightPlacement = true;
        PlacementWithDraggingDroppingController.EnableLightDrag = true;
    }


}//class