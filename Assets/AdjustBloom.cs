using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class AdjustBloom : MonoBehaviour
{
    public PostProcessVolume ppv;
    private Bloom bloom;
    private Slider bloomSlider;
    private float minValue = 0;
    private float maxValue = 4;

    /// <summary>
    /// If the ambientOcclusion option is enabled, set the sliders 
    /// starting value to the current value
    /// </summary>
    public void Start()
    {
        //Get the ambient occlusion slider object from the prefab, and then get its slider component
        bloomSlider = GameObject.FindGameObjectWithTag("BloomSlider").GetComponent<Slider>();

        //Set the sliders properties according to the post processing volume settings
        bloomSlider.minValue = minValue;
        bloomSlider.maxValue = maxValue;
        ppv.profile.TryGetSettings(out bloom);
        bloomSlider.value = bloom.intensity.value;


    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sliderValue"></param>
    public void Bloom(float sliderValue)
    {
        //If the bloom option is enabled, pass its value out
        ppv.profile.TryGetSettings(out bloom);
        bloom.intensity.value = sliderValue;
    }
}