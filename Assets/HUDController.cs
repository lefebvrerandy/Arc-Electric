using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public GameObject DropDownPanel;

    public Animator DropDownAnimation;

    private bool DropDownOpen;


    //*********************************************************************************************
    //
    //                                  Private Methods
    //
    //*********************************************************************************************
    private void OpenDropDown()
    {
        DropDownAnimation.SetBool("open", true);
    }

    private void CloseDropDown()
    {
        DropDownAnimation.SetBool("open", false);
    }


    //*********************************************************************************************
    //
    //                                  Public Methods
    //
    //*********************************************************************************************

    public void SettingsOpenHandler()
    {
        var test = DropDownAnimation.GetBool("open");
        if (DropDownAnimation.GetBool("open"))
            CloseDropDown();
        else
        {
            OpenDropDown();
        }
    }
}
