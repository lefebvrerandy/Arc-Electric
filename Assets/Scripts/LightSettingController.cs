/*
*  FILE          : LightSettingsController.cs
*  PROJECT       : PROG 3220 - Systems Project
*  PROGRAMMER    : Randy Lefebvre, Bence Karner, Lucas Roes, Kyle Horsley
*  DESCRIPTION   : Contains the LightSettingController
*  REFERENCE     : Script format was copied and altered from the following sources,
*   War dd [Username]. (2019). Flexible Color Picker: free asset for Unity3D. Retrieved on February 22, 2020, from https://www.youtube.com/watch?v=Ng3P_1nc8YE
*   Dehairs, F. (2019). Flexible Color Picker. Retrieved on February 22, 2020, from https://assetstore.unity.com/packages/tools/gui/flexible-color-picker-150497
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
*   NAME    : LightSettingController
*   PURPOSE : Responsible for controlling the animation, and behavior of the light settings panel, and it's children
*/
public class LightSettingController : MonoBehaviour
{
    public GameObject LightSettingsPanel;
    public Animator SlideOutAnimation;
    public Animator LightSettingsBtn;
    private bool PanelOpen;


    public FlexibleColorPicker uiColorPicker;
    public Material material;


    private Light[] LightsInScene;

    /*
    *  METHOD       : awake
    *  DESCRIPTION  : Get a reference to all the light objects present in the scene
    *  PARAMETER    : NA
    *  RETURNS      : NA
    */
    private void awake()
    {
        LightsInScene = FindObjectsOfType<Light>();
        //Alternatively
        //var lightCollection = GameObject.FindGameObjectsWithTag("Light");
    }



    /*
    *  METHOD       : Start
    *  DESCRIPTION  : Default the panel to an inactive state
    *  PARAMETER    : NA
    *  RETURNS      : NA
    */
    private void Start()
    {
        LightSettingsPanel.SetActive(false);
    }


    /*
    *  METHOD       : Update
    *  DESCRIPTION  : DEBUG
    *  PARAMETER    : NA
    *  RETURNS      : NA
    */
    void Update()
    {
        material.color = uiColorPicker.color;
    }


    /*
     *  METHOD       : PanelHandler
     *  DESCRIPTION  : Toggle between an active/inactive state when the UI button is pressed
     *  PARAMETER    : NA
     *  RETURNS      : NA
     */
    public void PanelHandler()
    {
        if (SlideOutAnimation.GetBool("open"))
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }


    /*
     *  METHOD       : OpenInventory
     *  DESCRIPTION  : 
     *  PARAMETER    : NA
     *  RETURNS      : NA
     */
    private void OpenInventory()
    {
        LightSettingsPanel.SetActive(true);
        SlideOutAnimation.SetBool("open", true);
        LightSettingsBtn.SetBool("open", true);
    }


    /*
     *  METHOD       : CloseInventory
     *  DESCRIPTION  : 
     *  PARAMETER    : NA
     *  RETURNS      : NA
     */
    private void CloseInventory()
    {
        SlideOutAnimation.SetBool("open", false);
        LightSettingsBtn.SetBool("open", false);
        StartCoroutine(DisablePanels());
    }


    /*
     *  METHOD       : DisablePanels
     *  DESCRIPTION  : 
     *  PARAMETER    : NA
     *  RETURNS      : IEnumerator : 
     */
    private IEnumerator DisablePanels()
    {
        int counter = 1;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
        }

        if (!SlideOutAnimation.GetBool("open"))
        {
            LightSettingsPanel.SetActive(false);
        }
    }


    /*
     *  METHOD       : ToggleLights
     *  DESCRIPTION  : Toggles all lights on/off for each press of the UI button
     *  PARAMETER    : NA
     *  RETURNS      : NA 
     */
    public void ToggleLights()
    {
        foreach (Light light in LightsInScene)
        {
            light.enabled = !light.enabled;
        }
    }

}//class