using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;
using System;

public class CaptureScreenshot : MonoBehaviour
{
    //The screenshotcapture button
    public Button camButton;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        camButton = GetComponent<Button>();
        // listen for button press
        camButton.onClick.AddListener(() => SS());
        yield return null;
    }

    private void SS()
    {

        // Coroutine that captures and saves a screenshot
        StartCoroutine(SaveScreenshot());

    }

    // Coroutine that captures and saves a screenshot
    IEnumerator SaveScreenshot()
    {
        // Wait till the last possible moment before screen rendering to hide the UI
        yield return null;
        // hide UI
        //GameObject.FindGameObjectsWithTag("Canvas").GetComponent<Canvas>().enabled = false;

        GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Canvas");

        foreach (GameObject go in gameObjectArray)
        {
            go.GetComponent<Canvas>().enabled = false;
        }

        // Wait until the end of frame
        yield return new WaitForEndOfFrame();


        // The SaveScreenshot() method returns the path of the saved image
        // The provided file name will be added a ".png" extension automatically
        string filename = "ARc-Light_Screenshot" + DateTime.Now.Ticks;

        string filePath = Sharing.ShareScreenshot(filename, "This is a sample message", "Hello");
        Debug.Log("filePath created: " + filePath);

        // Show UI after we're done
        //GameObject.FindGameObjectsWithTag("Canvas").enabled = true;

        foreach (GameObject go in gameObjectArray)
        {
            go.GetComponent<Canvas>().enabled = true;
        }

    }



}
