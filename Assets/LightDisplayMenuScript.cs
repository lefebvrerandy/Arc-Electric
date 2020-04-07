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
/// Controller script for setting a lights color, alpha channel, range, and intensity.
/// </summary>
[RequireComponent(typeof(Canvas))]
public class LightDisplayMenuScript : MonoBehaviour
{
    #region Properties


    /// <summary>
    /// Singleton instance of the class and accompanying prefab
    /// </summary>
    public static LightDisplayMenuScript Instance { get; set; }

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
    
    
    #endregion
    #region MonoBehaviours


    /// <summary>
    /// Instantiate the prefab, and set the default colors for the color palette
    /// </summary>
    private void Awake()
    {
        Instance = this;
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
}//class

////BeeKeeper
//colorPalette[0].buttonColor.color = new Color(246, 229, 141, 1.0f);
//colorPalette[0].patchName = "BeeKeeper";

////Turbo
//colorPalette[1].buttonColor.color = new Color(249, 202, 36, 1.0f);
//colorPalette[1].patchName = "Turbo";

////SpicedNectarine
//colorPalette[2].buttonColor.color = new Color(255, 190, 118, 1.0f);
//colorPalette[2].patchName = "SpicedNectarine";

////Qiuincejelly
//colorPalette[3].buttonColor.color = new Color(240, 147, 43, 1.0f);
//colorPalette[3].patchName = "Qiuincejelly";

////PinkGlamour
//colorPalette[4].buttonColor.color = new Color(255, 121, 121, 1.0f);
//colorPalette[4].patchName = "PinkGlamour";

////CarminePink
//colorPalette[5].buttonColor.color = new Color(235, 77, 75, 1.0f);
//colorPalette[5].patchName = "CarminePink";

////JuneBud
//colorPalette[6].buttonColor.color = new Color(186, 220, 88, 1.0f);
//colorPalette[6].patchName = "";

////PureApple
//colorPalette[7].buttonColor.color = new Color(106, 176, 76, 1.0f);
//colorPalette[7].patchName = "PureApple";

////CoastalBreeze
//colorPalette[8].buttonColor.color = new Color(223, 249, 251, 1.0f);
//colorPalette[8].patchName = "CoastalBreeze";

////HintOfIcePack
//colorPalette[9].buttonColor.color = new Color(199, 236, 238, 1.0f);
//colorPalette[9].patchName = "HintOfIcePack";

////MiddleBlue
//colorPalette[10].buttonColor.color = new Color(126, 214, 223, 1.0f);
//colorPalette[10].patchName = "MiddleBlue";

////GreenlandGreen
//colorPalette[11].buttonColor.color = new Color(34, 166, 179, 1.0f);
//colorPalette[11].patchName = "GreenlandGreen";

////Heliotrope
//colorPalette[12].buttonColor.color = new Color(224, 86, 253, 1.0f);
//colorPalette[12].patchName = "Heliotrope";

////SteelPink
//colorPalette[13].buttonColor.color = new Color(190, 46, 221, 1.0f);
//colorPalette[13].patchName = "SteelPink";

////ExodusFruit
//colorPalette[14].buttonColor.color = new Color(104, 109, 224, 1.0f);
//colorPalette[14].patchName = "ExodusFruit";

////Blurple
//colorPalette[15].buttonColor.color = new Color(72, 52, 212, 1.0f);
//colorPalette[15].patchName = "Blurple";

////DeepKoamaru
//colorPalette[16].buttonColor.color = new Color(48, 51, 107, 1.0f);
//colorPalette[16].patchName = "DeepKoamaru";

////DeepCove
//colorPalette[17].buttonColor.color = new Color(19, 15, 64, 1.0f);
//colorPalette[17].patchName = "DeepCove";

////SoaringEagle
//colorPalette[18].buttonColor.color = new Color(149, 175, 192, 1.0f);
//colorPalette[18].patchName = "SoaringEagle";

////WizardGrey
//colorPalette[19].buttonColor.color = new Color(83, 92, 104, 1.0f);
//colorPalette[19].patchName = "WizardGrey";

////White
//colorPalette[20].buttonColor.color = Color.white;
//colorPalette[20].patchName = "White";

////Black
//colorPalette[21].buttonColor.color = Color.black;
//colorPalette[21].patchName = "Black";