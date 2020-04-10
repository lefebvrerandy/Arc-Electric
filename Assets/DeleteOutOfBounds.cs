using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// When the user clicks on the object attached to this script, delete its parent
/// </summary>
public class DeleteOutOfBounds : MonoBehaviour, IPointerClickHandler
{
#pragma warning disable 649

    /// <summary>
    /// Time, in seconds, to wait until the user can place and move lights again
    /// </summary>
    [SerializeField] private float waitTimeSeconds;


    /// <summary>
    /// Delete the parent object
    /// </summary>
    /// <param name="eventData"> Position data of the input that triggered the event</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(EnablePlacementAndDragnDrop());
        RadialMenuSpawner.instance.menuOpen = false;
        Destroy(gameObject.transform.parent.gameObject);
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
}