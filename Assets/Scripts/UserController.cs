using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserController : MonoBehaviour
{


    //*********************************************************************************************
    //
    //                                  Public Methods
    //
    //*********************************************************************************************
    static public void SwitchSelected(string selectedName)
    {
        PlayerPrefs.SetString("Selected", selectedName);
        InventoryController.selectionChangedEvent.Invoke();
    }
}
