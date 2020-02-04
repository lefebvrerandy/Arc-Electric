using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserController : MonoBehaviour
{
    public Image image;
    public void SwitchSelected(string selectedName)
    {
        //image.sprite = Resources.Load<Sprite>("Images/" + selectedName);
        image.sprite = Resources.Load<Sprite>("Images/" + selectedName);
        PlayerPrefs.SetString("Selected", selectedName);
    }
}
