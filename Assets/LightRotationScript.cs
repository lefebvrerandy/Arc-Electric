/*
*  FILE         : LightRotationScript.cs
*  PROJECT      : PROG3220 - Systems Project
*  PROGRAMMER   : Bence Karner
*  DESCRIPTION  : This file contains the LightRotationScript menu script, and is part of the Radial Menu system.
*/

using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class LightRotationScript : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// Singleton instance of the class and accompanying prefab
    /// </summary>
    public static LightRotationScript instance { get; set; }

    /// <summary>
    /// Light fixture to rotate and change its orientation
    /// </summary>
    public GameObject lightFixture;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Slider xAxisSlider;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Slider yAxisSlider;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Slider zAxisSlider;


    private float previousDirection;

    #endregion
    #region MonoBehaviours


    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        instance = this;
        previousDirection = 0f;
    }


    #endregion
    #region PublicMethods


    /// <summary>
    /// 
    /// </summary>
    public void ConfigureSliders()
    {
        ConfigureXAxis();
        ConfigureYAxis();
        ConfigureZAxis();
    }


    /// <summary>
    /// Rotates the object along its X-axis according to the value set by the UI slider
    /// </summary>
    /// <param name="value"> New eulerAngle value </param>
    public void RotateAlongX(float value)
    {
        var yAxis = lightFixture.transform.rotation.eulerAngles.y;
        var zAxis = lightFixture.transform.rotation.eulerAngles.z;
        var rotationAngle = value - 180;
        Debug.Log(rotationAngle);
        lightFixture.transform.rotation = Quaternion.Euler(rotationAngle, yAxis, zAxis);
        Debug.Log(lightFixture.transform.rotation.eulerAngles.x);
    }


    /// <summary>
    /// Rotates the object along its Y-axis according to the value set by the UI slider
    /// </summary>
    /// <param name="value"> New eulerAngle value </param>
    public void RotateAlongY(float value)
    {
        var xAxis = lightFixture.transform.rotation.eulerAngles.x;
        var zAxis = lightFixture.transform.rotation.eulerAngles.z;
        var rotationAngle = value - 180;
        lightFixture.transform.rotation = Quaternion.Euler(xAxis, rotationAngle, zAxis);
    }


    /// <summary>
    /// Rotates the object along its Z-axis according to the value set by the UI slider
    /// </summary>
    /// <param name="value"> New eulerAngle value </param>
    public void RotateAlongZ(float value)
    {
        var xAxis = lightFixture.transform.rotation.eulerAngles.x;
        var yAxis = lightFixture.transform.rotation.eulerAngles.y;
        var rotationAngle = value - 180;
        lightFixture.transform.rotation = Quaternion.Euler(xAxis, yAxis, rotationAngle);
    }


    #endregion
    #region PrivateMethods


    private void ConfigureZAxis()
    {
       
    }


    private void ConfigureYAxis()
    {
       
    }


    private void ConfigureXAxis()
    {
        
    }


    #endregion
}//class