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
/// Controls the rotation of an object using a series of UI sliders
/// </summary>
[RequireComponent(typeof(Canvas))]
public class LightRotationScript : MonoBehaviour
{
#pragma warning disable 649


    /// <summary>
    /// Defines the axis an object can rotate
    /// </summary>
    public enum AxisOfRotation
    { 
        xAxis,
        yAxis,
        zAxis
    }
    
    #region Properties

    /// <summary>
    /// Singleton instance of the class and accompanying prefab
    /// </summary>
    public static LightRotationScript instance { get; set; }

    /// <summary>
    /// Light fixture that will be rotated using the UI
    /// </summary>
    public GameObject lightFixture;

    /// <summary>
    /// UI slider that controls the light objects X rotation
    /// </summary>
    [SerializeField] private Slider xAxisSlider;

    /// <summary>
    /// UI slider that controls the light objects Y rotation
    /// </summary>
    [SerializeField] private Slider yAxisSlider;

    /// <summary>
    /// UI slider that controls the light objects Z rotation
    /// </summary>
    [SerializeField] private Slider zAxisSlider;

    /// <summary>
    /// Y-axis distance from the anchor to the point where the panel is off the users screen
    /// </summary>
    public float HideDistance;

    /// <summary>
    /// Minimal rotation angle using the slider
    /// </summary>
    private int minAngle;

    /// <summary>
    /// Max rotation angle using the slider
    /// </summary>
    private int maxAngle;

    /// <summary>
    /// Current slider value representing the objects angle
    /// </summary>
    private int startingAngle;

    /// <summary>
    /// Previous eugler value of the objects x axis
    /// </summary>
    public int prevXAxis = 0;

    /// <summary>
    /// Previous eugler value of the objects y axis
    /// </summary>
    public int prevYAxis = 0;

    /// <summary>
    /// Previous eugler value of the objects z axis
    /// </summary>
    public int prevZAxis = 0;

    /// <summary>
    /// Previous value of the x slider property
    /// </summary>
    private int prevXSliderValue = 0;

    /// <summary>
    /// Previous value of the y slider property
    /// </summary>
    private int prevYSliderValue = 0;

    /// <summary>
    /// Previous value of the z slider property
    /// </summary>
    private int prevZSliderValue = 0;

    #region AlternativeSliderRotation
    //[SerializeField]
    //private Slider[] sliders;
    #endregion

    #endregion
    #region MonoBehaviours


    /// <summary>
    /// Set the instance of the class, and default the slider values
    /// </summary>
    private void Awake()
    {
        instance = this;
        minAngle = 0;
        maxAngle = 360;
        startingAngle = 0;
        ConfigureSliders();

        #region AlternativeSliderRotation

        //if(sliders != null)
        //{
        //    sliders[0].value = transform.localEulerAngles.x;
        //    sliders[1].value = transform.localEulerAngles.y;
        //    sliders[2].value = transform.localEulerAngles.z;

        //    foreach (Slider slider in sliders)
        //    {
        //        slider.onValueChanged.AddListener(RoateObject);
        //    }
        //}

        #endregion
    }


    #endregion
    #region PublicMethods


    /// <summary>
    /// Set the min, max, and starting values of the sliders according to the orientation 
    /// of the referenced game object
    /// </summary>
    public void ConfigureSliders()
    {
        ConfigureXAxis();
        ConfigureYAxis();
        ConfigureZAxis();
    }


    #region AlternativeSliderRotation
    /// <summary>
    /// Set the objects rotation to the x, y, and z slider values
    /// </summary>
    /// <param name="value"> New EulerAngle value from the slider</param>
    public void RoateObject(float value)
    {
        lightFixture.transform.localEulerAngles = new Vector3(
            xAxisSlider.value,
            yAxisSlider.value,
            zAxisSlider.value
        );
    }
    #endregion


    /// <summary>
    /// Rotates the object along its X-axis according to the value set by the UI slider
    /// </summary>
    /// <param name="value"> New eulerAngle value </param>
    public void RotateAlongX(float value)
    {
        if(lightFixture != null)
        {
            Rotate(AxisOfRotation.xAxis, value);
        }
    }


    /// <summary>
    /// Rotates the object along its Y-axis according to the value set by the UI slider
    /// </summary>
    /// <param name="value"> New eulerAngle value </param>
    public void RotateAlongY(float value)
    {
        if (lightFixture != null)
        {
            Rotate(AxisOfRotation.yAxis, value);
        }
    }


    /// <summary>
    /// Rotates the object along its Z-axis according to the value set by the UI slider
    /// </summary>
    /// <param name="value"> New eulerAngle value </param>
    public void RotateAlongZ(float value)
    {
        if (lightFixture != null)
        {
            Rotate(AxisOfRotation.zAxis, value);
        }
    }


    /// <summary>
    /// Sets the previous axis values for the object based on its orientation
    /// </summary>
    /// <param name="orientation"> Vector containing the 3D eugler values representing the objects orientation</param>
    public void SetPreviousOrientation(Vector3 orientation)
    {
        prevXAxis = Convert.ToInt32(orientation.x);
        prevYAxis = Convert.ToInt32(orientation.y);
        prevZAxis = Convert.ToInt32(orientation.z);
    }


    #endregion
    #region PrivateMethods


    /// <summary>
    /// Rotates the object along the specified axis and value
    /// </summary>
    /// <param name="axis"> Axis to rotate the object around </param>
    /// <param name="sliderValue"> Used to calc the number of units to move the object along its axis</param>
    private void Rotate(AxisOfRotation axis, float sliderValue)
    {
        int intSliderValue = Convert.ToInt32(sliderValue);
        switch (axis)
        {
            case AxisOfRotation.xAxis:
                if (intSliderValue < prevXSliderValue)
                {
                    //Slider was moved left
                    prevXAxis += prevXSliderValue - intSliderValue;
                }
                else
                {
                    //Slider was moved right
                    prevXAxis -= intSliderValue - prevXSliderValue;
                }
                prevXSliderValue = intSliderValue;
                break;

            case AxisOfRotation.yAxis:
                if (intSliderValue < prevYSliderValue)
                {
                    //Slider was moved left
                    prevYAxis += prevYSliderValue - intSliderValue;
                }
                else
                {
                    //Slider was moved right
                    prevYAxis -= intSliderValue - prevYSliderValue;
                }
                prevYSliderValue = intSliderValue;
                break;

            case AxisOfRotation.zAxis:
                if (intSliderValue < prevZSliderValue)
                {
                    //Slider was moved left
                    prevZAxis += prevZSliderValue - intSliderValue;
                }
                else
                {
                    //Slider was moved right
                    prevZAxis -= intSliderValue - prevZSliderValue;
                }
                prevZSliderValue = intSliderValue;
                break;
        }

        //Apply the rotation
        lightFixture.transform.rotation = Quaternion.Euler(prevXAxis, prevYAxis, prevZAxis);
        Debug.Log(lightFixture.transform.rotation);
    }


    /// <summary>
    /// Set the X-axis sliders min, max, and starting values according to the selected lights properties
    /// </summary>
    private void ConfigureXAxis()
    {
        if (lightFixture == null)
        {
            xAxisSlider.minValue = minAngle;
            xAxisSlider.maxValue = maxAngle;
            xAxisSlider.value = startingAngle;
        }
        else
        {
            xAxisSlider.value = prevXAxis;
        }
    }


    /// <summary>
    /// Set the y-axis sliders min, max, and starting values according to the selected lights properties
    /// </summary>
    private void ConfigureYAxis()
    {
        if (lightFixture == null)
        {
            yAxisSlider.minValue = minAngle;
            yAxisSlider.maxValue = maxAngle;
            yAxisSlider.value = startingAngle;
        }
        else
        {
            yAxisSlider.value = prevYAxis;
        }
    }


    /// <summary>
    /// Set the z-axis sliders min, max, and starting values according to the selected lights properties
    /// </summary>
    private void ConfigureZAxis()
    {
        if (lightFixture == null)
        {
            zAxisSlider.minValue = minAngle;
            zAxisSlider.maxValue = maxAngle;
            zAxisSlider.value = startingAngle;
        }
        else
        {
            zAxisSlider.value = prevZAxis;
        }
    }


    #endregion
#pragma warning restore 649
}//class