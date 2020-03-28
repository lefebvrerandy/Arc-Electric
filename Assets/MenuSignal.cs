using UnityEngine;


/// <summary>
/// Small class used to adjust the radial menus properties based on the state of the current submenu
/// </summary>
public class MenuSignal : MonoBehaviour
{
    /// <summary>
    /// Reference to the calling radial menu controller
    /// </summary>
    public RadialMenuController rmc;


    /// <summary>
    /// Re-enable activation of the radial menu
    /// </summary>
    public void EnableMenu()
    {
        rmc.menuEnabled = true;
    }
}