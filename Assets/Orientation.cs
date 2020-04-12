/*
*  FILE         : Orientation.cs
*  PROJECT      : PROG3220 - Systems Project
*  PROGRAMMER   : Bence Karner
*  DESCRIPTION  : This file contains the Orientation class
*/

using UnityEngine;

/// <summary>
/// Used to get and set the objects initial orientation as eugler values. Set by the 
/// </summary>
public class Orientation : MonoBehaviour
{

    #region Properties


    /// <summary>
    /// Contains the objects initial orientation as eugler values
    /// </summary>
    public Vector3 Angles { get; set; }


    #endregion
    #region MonoBehaviours


    /// <summary>
    /// Get the starting eugler angles of the object
    /// </summary>
    void Start()
    {
        Angles =  gameObject.transform.localEulerAngles;
    }


    #endregion 
}