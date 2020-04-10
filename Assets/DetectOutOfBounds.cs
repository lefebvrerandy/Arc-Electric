/*
*  FILE         : DetectOutOfBounds.cs
*  PROJECT      : PROG3220 - Systems Project
*  PROGRAMMER   : Bence Karner
*  DESCRIPTION  : Contains the DetectOutOfBounds class
*/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Contains events for detecting if the user clicked on a UI element.
/// </summary>
public class DetectOutOfBounds : MonoBehaviour, IPointerClickHandler
{
#pragma warning disable 649


    #region Properties


    /// <summary>
    /// Time, in seconds, to wait until the user can place and move lights again
    /// </summary>
    [SerializeField] private float waitTimeSeconds;


    #endregion

    /// <summary>
    /// Event that fires once the user clicks outside on the game object attached to this script. 
    /// Gets the parent of the object, and deactivate it.
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        RadialMenuSpawner.instance.menuOpen = false;
        gameObject.transform.parent.gameObject.GetComponent<Canvas>().enabled = false;
        AnimateOut();
        StartCoroutine(EnablePlacementAndDragnDrop());
    }


    /// <summary>
    /// Ease out the menu panel below the screens bottom most edge
    /// </summary>
    private void AnimateOut()
    {
        var parentName = transform.parent.name;
        float easeOutDistance = 0;
        switch (parentName)
        {
            case "LightDisplayMenu":
                easeOutDistance = transform.parent.GetComponent<LightDisplayMenuScript>().HideDistance;
                break;

            case "LightRotationMenu":
                easeOutDistance = transform.parent.GetComponent<LightRotationScript>().HideDistance;
                break;

            case "LightPostProcessingMenu":
                easeOutDistance = transform.parent.GetComponent<LightPostProcessingScript>().HideDistance;
                break;

            default:
                break;
        }
        var menuPanel = gameObject.transform.GetChild(0).gameObject;
        LeanTween.moveLocalY(menuPanel, -easeOutDistance, 0.1f).setEaseLinear();
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

#pragma warning restore 649
}//class