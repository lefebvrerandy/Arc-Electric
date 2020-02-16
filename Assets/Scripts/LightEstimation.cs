using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// A component that can be used to access the most
/// recently received light estimation information
/// for the physical environment as observed by an
/// AR device.
/// </summary>
[RequireComponent(typeof(Light))]
public class LightEstimation : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The ARCameraManager which will produce frame events containing light estimation information")]
    ARCameraManager m_CameraManager;

    //Reference to the light component in the directional light object
    Light m_Light;

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
    
    /// <summary>
    /// Get or set the <c>ARCameraManager</c>.
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

    //Get a reference to the light component from the directional light object
    void Awake ()
    {
        m_Light = GetComponent<Light>();
    }

    //If environmental light estimation is enabled
    void OnEnable()
    {
        //Fire the light frame event when available
        if (m_CameraManager != null)
        {
            m_CameraManager.frameReceived += FrameChanged;
        }
    }

    //If environmental light estimation is disabled
    void OnDisable()
    {
        //De-register the lighting update event
        if (m_CameraManager != null)
        {
            m_CameraManager.frameReceived -= FrameChanged;
        }
    }


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
}