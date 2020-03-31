using UnityEngine;


/// <summary>
/// Simple class for signaling the PlacementWithDraggingDroppingController script 
/// </summary>
public class ToggleLightPlacement : MonoBehaviour
{

    /// <summary>
    /// After a one second duration, enable new lights to be instantiated
    /// </summary>
    public void EnableLightPlacement()
    {
        Debug.Log("LightPlacementTrigger Invoked1");
        Invoke("LightPlacementTrigger", 1);
        Debug.Log("LightPlacementTrigger Invoked2");
    }


    /// <summary>
    /// Toggle the EnableLightPlacement property to true
    /// </summary>
    private static void LightPlacementTrigger()
    {
        Debug.Log("LightPlacementTrigger activated");
        PlacementWithDraggingDroppingController.EnableLightPlacement = true;
    }

    /// <summary>
    /// Immediately disable the ability to instantiate lights
    /// </summary>
    public void DisableLightPlacement()
    {
        PlacementWithDraggingDroppingController.EnableLightPlacement = false;
    }
}