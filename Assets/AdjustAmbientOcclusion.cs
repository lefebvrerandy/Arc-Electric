using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class AdjustAmbientOcclusion : MonoBehaviour
{
    public PostProcessVolume ppv;
    private Slider ambientOcclusionSlider;
    private AmbientOcclusion ambientOcclusion;
    private float minValue = 0;
    private float maxValue = 4;

    /// <summary>
    /// If the ambientOcclusion option is enabled, set the sliders 
    /// starting value to the current value
    /// </summary>
    public void Start()
    {
        //Get the ambient occlusion slider object from the prefab, and then get its slider component
        ambientOcclusionSlider = GameObject.FindGameObjectWithTag("AmbientOcclusionSlider").GetComponent<Slider>(); 

        //Set the sliders properties according to the post processing volume settings
        ambientOcclusionSlider.minValue = minValue;
        ambientOcclusionSlider.maxValue = maxValue;
        ppv.profile.TryGetSettings(out ambientOcclusion);
        ambientOcclusionSlider.value = ambientOcclusion.intensity.value;
    }

    /// <summary>
    /// Set the ambient occlusion value to the sliders current value
    /// </summary>
    /// <param name="sliderValue"></param>
    public void AmbientOcclusion(float sliderValue)
    {
        ambientOcclusion.intensity.value = sliderValue;
    }
}