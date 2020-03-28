using System;
using UnityEngine;


/// <summary>
/// Allows UI elements to change the target game objects size
/// </summary>
public class ScaleImage : MonoBehaviour
{
    [SerializeField]
    public GameObject image;

    /// <summary>
    /// Changes the game objects transform.localscale using a UIslider
    /// </summary>
    /// <param name="value">UIslider value</param>
    public void Scale(float sliderValue)
    {
        const float min = 0f;
        const float max = 2.2f;
        const float normalizeFactor = 9.09f;

        //Normalize the slider value so that it's never above the max
        var value = sliderValue / normalizeFactor;
        float truncated = (float)(Math.Truncate((double)value * 100.0) / 100.0);
        float rounded = (float)(Math.Round((double)value, 2));
        if(rounded > max)
        {
            rounded = max;
        }
        else if(rounded < min)
        {
            rounded = min;
        }
        image.transform.localScale = new Vector3(rounded, rounded, 0f);
    }
}
