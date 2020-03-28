using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


/// <summary>
/// 
/// </summary>
public class PostProcessingEffects : MonoBehaviour
{
    public PostProcessVolume ppv;
    private Bloom _bloom;
    private AmbientOcclusion _ambientOcclusion;


    private void Start()
    {
        //If the bloom option is enabled, pass its value out
        ppv.profile.TryGetSettings(out _bloom);
        Debug.Log(_bloom.intensity.value);

        //If the bloom option is enabled, pass its value out
        ppv.profile.TryGetSettings(out _ambientOcclusion);
        Debug.Log(_ambientOcclusion.intensity.value);
    }
}