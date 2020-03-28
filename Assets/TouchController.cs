


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TouchController : MonoBehaviour
{
    public GameObject gObj = null;          //Parent game object the script is attached to (i.e. the touched object)
    private Plane objPlane;                 //Plane across which the object is moving
    Vector3 m0;                             //Offset?


    //Creates a ray out from the touch position. Works in both perspective, and orthographic camera view modes
    //Holistic3d. (2016). Unity Mobile Dev From Scratch: Understanding Screen and World Coordinates for Raycasting. Retrieved March 13, 2020, from https://www.youtube.com/watch?v=7QomGnOyQoY&list=PLi-ukGVOag_1lNphWV5S-xxWDe3XANpyE&index=5
    //Holistic3d. (2016). Unity Mobile From Scratch: TouchPhases and Touch Count. Retrieved March 13, 2020, from https://www.youtube.com/watch?v=ay9bbWJQ01w
    private Ray GenerateTouchRay(Vector3 touchPos)
    {
        Vector3 touchPosFar = new Vector3(touchPos.x, touchPos.y, Camera.main.farClipPlane);
        Vector3 touchPosNear = new Vector3(touchPos.x, touchPos.y, Camera.main.nearClipPlane);
        Vector3 touchPosF = Camera.main.ScreenToWorldPoint(touchPosFar);
        Vector3 touchPosN = Camera.main.ScreenToWorldPoint(touchPosNear);
        Ray touchRay = new Ray(touchPosN, touchPosF - touchPosN);
        return touchRay;
    }

    //REMEBER THE OBJECT TO HIT MUST HAVE A COLLIDER COMPONENT TO ACTIVATE THE RAYCAST HIT

    public void Update()
    {
        //Check if the screen was touched, and grab the location of the fist touch
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //Raycast at the location of the touch
                Ray touchRay = GenerateTouchRay(Input.GetTouch(0).position);
                RaycastHit hit;

                //Determine if an object was hit by the raycast
                if (Physics.Raycast(touchRay.origin, touchRay.direction, out hit))
                {
                    // //DEBUG NEW
                    
                    // Instantiate(RadialMenu, hit.transform.position, Quaternion,identity);
                    // Destroy(hit.transform.gameObject);
                    // //DEBUG NEW END

                    //Get a reference to the touched object
                    gObj = hit.transform.gameObject;
                    objPlane = new Plane(Camera.main.transform.forward * -1,gObj.transform.position);


                    //Calc the touch offset
                    Ray mRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    float rayDistance;
                    objPlane.Raycast(mRay, out rayDistance);
                    m0 = gObj.transform.position - mRay.GetPoint(rayDistance);
                }
            }

            //Dragging finger across the screen
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && gObj)
            {

                //Raycast on the current touch position, and move the selected object to the new location
                Ray mRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                float rayDistance;
                if (objPlane.Raycast(mRay, out rayDistance))
                {
                    gObj.transform.position = mRay.GetPoint(rayDistance) + m0;
                }
            }

            //No longer touching the screen
            else if (Input.GetTouch(0).phase == TouchPhase.Ended && gObj)
            {
                gObj = null;
            }
        }
    }
}
