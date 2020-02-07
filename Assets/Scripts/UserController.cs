using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserController : MonoBehaviour
{
    public Image image;

    public GameObject[] Lights;

    private void Start()
    {
        foreach(var light in Lights)
        {
            light.SetActive(false);
        }
    }
    public void SwitchSelected(string selectedName)
    {
        //image.sprite = Resources.Load<Sprite>("Images/" + selectedName);
        foreach (var light in Lights)
        {
            if (light.name == selectedName)
                light.SetActive(true);
            else
                light.SetActive(false);
        }
        PlayerPrefs.SetString("Selected", selectedName);
    }
}
