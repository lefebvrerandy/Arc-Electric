/*
*  FILE         : LightDisplayMenuScript.cs
*  PROJECT      : PROG3220 - Systems Project
*  PROGRAMMER   : Bence Karner
*  DESCRIPTION  : This file contains the LightDisplayMenuScript menu script, and is part of the Radial Menu system.
*/

using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller script for setting a lights color, range, and intensity
/// </summary>
[RequireComponent(typeof(Canvas))]
public class LightDisplayMenuScript : MonoBehaviour
{
#pragma warning disable 649
    #region Properties


    /// <summary>
    /// Singleton instance of the class and accompanying prefab
    /// </summary>
    public static LightDisplayMenuScript instance { get; set; }

    /// <summary>
    /// Reference to the light component that can be changed using the script
    /// </summary>
    public Light Light { get; set; }

    /// <summary>
    /// The most recently selected color patch from the UI
    /// </summary>
    private Color selectedColor;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Slider AlphaSlider;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Slider IntensitySlider;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Slider RangeSlider;

    /// <summary>
    /// Y-axis distance from the anchor to the point where the panel is off the users screen
    /// </summary>
    public float HideDistance;


    #endregion
    #region MonoBehaviours


    /// <summary>
    /// Instantiate the prefab, and set the default colors for the color palette
    /// </summary>
    private void Awake()
    {
        instance = this;
        selectedColor = Color.white;

        var uiColorPatchButtons = GameObject.FindGameObjectsWithTag("ColorPatch");
        int i = 0;
        foreach(var item in uiColorPatchButtons)
        {
            var buttonColor = item.GetComponent<Image>().color;
            item.GetComponent<Button>().onClick.AddListener(() => SelectColor(buttonColor));
            i++;
        }
    }


    #endregion
    #region PublicMethods


    /// <summary>
    /// Set the selected colors alpha value from the UISlider value
    /// </summary>
    /// <param name="value">New alpha value from the slider component</param>
    public void SetLightAlpha(float value)
    {
        selectedColor = new Color(selectedColor.r, selectedColor.g, selectedColor.b, value);
        Light.color = selectedColor;
    }

    /// <summary>
    /// Set the lights intensity to the provided value
    /// </summary>
    /// <param name="value">New value for the light intensity</param>
    public void SetLightIntensity(float value)
    {
        Light.intensity = value;
    }


    /// <summary>
    /// Set the lights range to the provided value
    /// </summary>
    /// <param name="value">New value for the light range</param>
    public void SetLightRange(float value)
    {
        Light.range = value;
    }


    /// <summary>
    /// Meant to be called once the menu is visible, and the selected light component has been updated.
    /// </summary>
    public void ConfigureSliders()
    {
        ConfigureAlphaSlider(Light);
        ConfigureIntensitySlider(Light);
        ConfigureRangeSlider(Light);
    }


    #endregion
    #region PrivateMethods


    /// <summary>
    /// Sets the alpha sliders min, max, and current values
    /// </summary>
    /// <param name="light">Light component used to set the selected colors alpha</param>
    private void ConfigureAlphaSlider(Light light)
    {
        if (Light != null)
        {
            const int minAlpha = 0;
            const int maxAlpha = 1;
            AlphaSlider.minValue = minAlpha;
            AlphaSlider.maxValue = maxAlpha;
            AlphaSlider.value = light.color.a;
        }
    }


    /// <summary>
    /// Sets the intensity sliders min, max, and current values
    /// </summary>
    /// <param name="light">Component used to set the selected lights intensity value</param>
    private void ConfigureIntensitySlider(Light light)
    {
        if (Light != null)
        {
            const int minIntensity = 0;
            const int maxIntensity = 10;
            IntensitySlider.minValue = minIntensity;
            IntensitySlider.maxValue = maxIntensity;
            IntensitySlider.value = light.intensity;
        }
    }


    /// <summary>
    /// Sets the range sliders min, max, and current values
    /// </summary>
    /// <param name="light">Component used to set the selected lights range value</param>
    private void ConfigureRangeSlider(Light light)
    {
        if (Light != null)
        {
            const int mineRange = 0;
            const int maxRange = 20;
            RangeSlider.minValue = mineRange;
            RangeSlider.maxValue = maxRange;
            RangeSlider.value = light.range;
        }
    }


    /// <summary>
    /// Save the selected color patch using the name of the color
    /// </summary>
    private void SelectColor(Color buttonColor)
    {
        selectedColor = buttonColor;
        Light.color = selectedColor;
    }


    #endregion
#pragma warning restore 649
}//class