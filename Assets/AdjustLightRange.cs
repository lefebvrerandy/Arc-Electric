using UnityEngine;


/// <summary>
/// Provides methods for adjusting a Light components properties
/// </summary>
public class AdjustLightRange : MonoBehaviour
{
    public Light light;

    /// <summary>
    /// Change the lights range to the provided argument
    /// </summary>
    /// <param name="value">New value for the light range</param>
    public void AdjustRange (float value)
    {
        light.range = value;
    }
}
