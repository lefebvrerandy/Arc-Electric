using UnityEngine;


/// <summary>
/// Provides methods for adjusting a Light components properties
/// </summary>
public class AdjustLightRange : MonoBehaviour
{
    public GameObject lightFixture;

    /// <summary>
    /// Change the lights range to the provided argument
    /// </summary>
    /// <param name="value">New value for the light range</param>
    public void AdjustRange (float value)
    {
        var light = lightFixture.GetComponent<Light>();
        light.range = value;
    }
}
