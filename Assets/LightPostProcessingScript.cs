/*
*  FILE         : LightPostProcessingScript.cs
*  PROJECT      : PROG3220 - Systems Project
*  PROGRAMMER   : Bence Karner
*  DESCRIPTION  : This file contains the LightPostProcessingScript menu script, and is part of the Radial Menu system.
*/

using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

/// <summary>
///  Controller script for setting the cameras post processing effects, including the bloom, and ambient occlusion properties
/// </summary>
public class LightPostProcessingScript : MonoBehaviour
{
    #region Properties


    /// <summary>
    /// Reference to the post processing volume component attached to the AR Session Origin camera
    /// </summary>
    public PostProcessVolume ppv;

    /// <summary>
    /// Reference to the bloom component in the PPV
    /// </summary>
    private Bloom bloom;

    /// <summary>
    /// Reference to the aoc component in the PPV
    /// </summary>
    private AmbientOcclusion ambientOcclusion;

    /// <summary>
    /// UI slider controlling the bloom value
    /// </summary>
    public Slider bloomSlider;

    /// <summary>
    /// UI slider controlling the ambient occlusion value
    /// </summary>
    public Slider ambientOcclusionSlider;

    /// <summary>
    /// Minimal ppv intensity you can set in the inspector
    /// </summary>
    private float minValue = 0;


    /// <summary>
    /// Maximal ppv intensity you can set in the inspector
    /// </summary>
    private float maxValue = 4;


    #endregion
    #region MonoBehaviours


    /// <summary>
    /// If the ppv effects have been enabled get their values and set starting values for the UI sliders
    /// </summary>
    public void Awake()
    {

        //Default the camera effects to null so we can check if they've
        //  been set in their accompanying Set() methods
        bloom = null;
        ambientOcclusion = null;

        ppv.profile.TryGetSettings(out bloom);
        ppv.profile.TryGetSettings(out ambientOcclusion);

        ConfigureBloomSlider();
        ConfigureAmbientOcclusionSlider();
    }


    #endregion
    #region PublicMethods


    /// <summary>
    /// Sets intensity of the cameras bloom effect
    /// </summary>
    /// <param name="sliderValue"> New bloom value from the UI slider </param>
    public void SetBloom(float sliderValue)
    {
        if (bloom != null)
        {
            bloom.intensity.value = sliderValue;
        }
    }


    /// <summary>
    /// Set the intensity of the aoc effect
    /// </summary>
    /// <param name="sliderValue">  New aoc value from the UI slider</param>
    public void SetAmbientOcclusion(float sliderValue)
    {
        if(ambientOcclusion != null)
        {
            ambientOcclusion.intensity.value = sliderValue;
        }
    }


    #endregion
    #region PrivateMethods


    /// <summary>
    /// Set the min, max and starting values for the bloom slider
    /// </summary>
    private void ConfigureBloomSlider()
    {
        if(bloom!= null)
        {
            bloomSlider.minValue = minValue;
            bloomSlider.maxValue = maxValue;
            bloomSlider.value = bloom.intensity.value;
        }
    }

    /// <summary>
    /// Set the min, max, and starting values for the aoc slider
    /// </summary>
    private void ConfigureAmbientOcclusionSlider()
    {
        if(ambientOcclusion != null)
        {
            ambientOcclusionSlider.minValue = minValue;
            ambientOcclusionSlider.maxValue = maxValue;
            ambientOcclusionSlider.value = ambientOcclusion.intensity.value;
        }
    }


    #endregion
}//class