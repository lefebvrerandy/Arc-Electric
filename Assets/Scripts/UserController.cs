using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserController : MonoBehaviour
{

    private void Start()
    {
    }

    //*********************************************************************************************
    //
    //                                  Public Methods
    //
    //*********************************************************************************************
    static public void SwitchSelected(string selectedName/*, List<GameObject> lightPrefabsSelected*/)
    {
        //foreach (var light in lightPrefabsSelected)
        //{
        //    string[] name = light.name.Split('(');
        //    if (name[0] == selectedName)
        //        light.SetActive(true);
        //    else
        //        light.SetActive(false);
        //}
        PlayerPrefs.SetString("Selected", selectedName);
    }
}
