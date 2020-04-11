/*
*  FILE         : DetectOutOfBounds.cs
*  PROJECT      : PROG3220 - Systems Project
*  PROGRAMMER   : Bence Karner
*  DESCRIPTION  : Contains the DetectOutOfBounds class
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Contains events for detecting if the user clicked on a UI element.
/// </summary>
public class DetectOutOfBounds : MonoBehaviour, IPointerClickHandler
{
#pragma warning disable 649


    /// <summary>
    /// Time, in seconds, to wait until the user can place and move lights again
    /// </summary>
    [SerializeField] private float waitTimeSeconds;

    /// <summary>
    /// First object considered out of bounds
    /// </summary>
    [SerializeField] private GameObject outOfBounds;

    /// <summary>
    /// Contains list of all objects hit by a raycast at the users touch position
    /// </summary>
    private List<RaycastResult> raycastHits { get; set; }

    /// <summary>
    /// Instantiate the properties
    /// </summary>
    private void Awake()
    {
        raycastHits = new List<RaycastResult>();
    }


    /// <summary>
    /// Event that fires once the user clicks outside on the game object attached to this script. 
    /// Gets the parent of the object, and deactivate it.
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsPointerOverUIObject())
        {
            return;
        }

        if(GetSelectedUIObject() != outOfBounds.name)
        {
            return;
        }

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


    /// <summary>
    /// Checks if the users last click location was on a UI element
    /// References: 
    /// Fabian-mkv. (2015). IsPointerOverGameObject not working with touch input. Retrieved April 6, 2020, from
    /// https://answers.unity.com/questions/1115464/ispointerovergameobject-not-working-with-touch-inp.html
    /// https://drive.google.com/file/d/0B__1zp7jwQOKNVhFaGxhbWt5TVU/view
    /// </summary>
    /// <returns>True if the user click on a UI element, false otherwise </returns>
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        var touch = Input.GetTouch(0);
        eventDataCurrentPosition.position = new Vector2(touch.position.x, touch.position.y);
        raycastHits.Clear();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, raycastHits);
        return raycastHits.Count > 0;
    }


    /// <summary>
    /// Get the top most UI element hit by the raycast
    /// </summary>
    /// <returns>Name of the selected UI element </returns>
    private string GetSelectedUIObject()
    {
        return raycastHits[0].gameObject.name;
    }

#pragma warning restore 649
}//class