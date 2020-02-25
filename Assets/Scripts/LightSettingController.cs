//War dd [Username]. (2019). Flexible Color Picker: free asset for Unity3D. Retrieved on February 22, 2020, from https://www.youtube.com/watch?v=Ng3P_1nc8YE
//Dehairs, F. (2019). Flexible Color Picker. Retrieved on February 22, 2020, from https://assetstore.unity.com/packages/tools/gui/flexible-color-picker-150497

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LightSettingController : MonoBehaviour
{
    public FlexibleColorPicker uiColorPicker;
    public Material material; 



    void Update()
    {
        material.color = uiColorPicker.color;
    }
}