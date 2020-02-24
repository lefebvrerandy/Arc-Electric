/*
*  FILE          : LightEstimation.cs
*  PROJECT       : PROG 3220 - Systems Project
*  PROGRAMMER    : Randy Lefebvre, Bence Karner, Lucas Roes, Kyle Horsley
*  DESCRIPTION   : This file contains the LightEstimation class, which is used to control the built in 
*                  AR light estimation system in both iOS, and Android devices
*  REFERENCE     : Script format was copied and altered from the following sources,
*   Unity. (2020). AR Foundation Samples. Retrieved on February 3, 2020, from 
*   https://github.com/Unity-Technologies/arfoundation-samples
*/


using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;


/*
*   NAME    : LightEstimation
*   PURPOSE : Uses the recently received light estimation information for the physical environment
*             as observed by an AR device, in order to adjust the AR shadows, and surface visuals
*/
[RequireComponent(typeof(Light))]
public class LightEstimation : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The ARCameraManager which will produce frame events containing light estimation information")]
    ARCameraManager m_CameraManager;

    /// <summary>
    /// Get or set the ARCameraManager
    /// </summary>
    public ARCameraManager cameraManager
    {
        get { return m_CameraManager; }
        set
        {
            if (m_CameraManager == value) { return; }

            if (m_CameraManager != null)
            {
                m_CameraManager.frameReceived -= FrameChanged;
                m_CameraManager = value;
            }

            if (m_CameraManager != null & enabled) { m_CameraManager.frameReceived += FrameChanged; }
        }
    }


    /// <summary>
    /// Reference to the light component in the directional light object
    /// </summary>
    Light m_Light;

    #region LightProperties

    /// <summary>
    /// The estimated direction of the main light of the physical environment, if available.
    /// </summary>
    public Vector3? mainLightDirection { get; private set; }

    /// <summary>
    /// The estimated color of the main light of the physical environment, if available.
    /// </summary>
    public Color? mainLightColor { get; private set; }

    /// <summary>
    /// The estimated spherical harmonics coefficients of the physical environment, if available.
    /// </summary>
    public SphericalHarmonicsL2? sphericalHarmonics { get; private set; }

    #endregion


    /*
    *  METHOD       : Awake
    *  DESCRIPTION  : Get a reference to the light component from the directional light object
    *  PARAMETER    : NA
    *  RETURNS      : NA
    */
    void Awake ()
    {
        m_Light = GetComponent<Light>();
    }


    /*
    *  METHOD       : OnEnable
    *  DESCRIPTION  : If environmental light estimation is enabled, start processing the incoming light data
    *  PARAMETER    : NA
    *  RETURNS      : NA
    */
    void OnEnable()
    {
        //Fire the light frame event when available
        if (m_CameraManager != null)
        {
            m_CameraManager.frameReceived += FrameChanged;
        }
    }


    /*
    *  METHOD       : OnDisable
    *  DESCRIPTION  : If environmental light estimation is disabled, stop processing the incoming light data
    *  PARAMETER    : NA
    *  RETURNS      : NA
    */
    void OnDisable()
    {
        //De-register the lighting update event
        if (m_CameraManager != null)
        {
            m_CameraManager.frameReceived -= FrameChanged;
        }
    }


    /*
    *  METHOD       : FrameChanged
    *  DESCRIPTION  : If light estimation is enabled, and set to environmental HDR, use the incoming light data to adjust the 
    *                 AR object shadows, and surface reflections to align with the real world lighting and color
    *  PARAMETER    : ARCameraFrameEventArgs : Incoming light data captured for current frame
    *  RETURNS      : NA
    */
    void FrameChanged(ARCameraFrameEventArgs args)
    {
        if (args.lightEstimation.mainLightDirection.HasValue)
        {
            mainLightDirection = args.lightEstimation.mainLightDirection;
            m_Light.transform.rotation = Quaternion.LookRotation(mainLightDirection.Value);
        }

        if (args.lightEstimation.ambientSphericalHarmonics.HasValue)
        {
            sphericalHarmonics = args.lightEstimation.ambientSphericalHarmonics;
            RenderSettings.ambientMode = AmbientMode.Skybox;
            RenderSettings.ambientProbe = sphericalHarmonics.Value;
        }

        if (args.lightEstimation.mainLightColor.HasValue)
        {
            mainLightColor = args.lightEstimation.mainLightColor;

            //ARCore requires light/colors to be expressed in gamma mode
            m_Light.color = mainLightColor.Value / Mathf.PI;
            m_Light.color = m_Light.color.gamma;
        }

        //Get a ref to the ARSessionOrigin camera, and change its 
        //ARCore returns color in HDR format (can be represented as FP16 and have values above 1.0)
        var camera = m_CameraManager.GetComponentInParent<Camera>();
    }

}//class