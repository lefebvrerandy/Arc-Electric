/*
*  FILE          : CaptureScreenshot.cs
*  PROJECT       : PROG 3220 - Systems Project
*  PROGRAMMER    : Randy Lefebvre, Bence Karner, Lucas Roes, Kyle Horsley
*  DESCRIPTION   : This script controls the functionality for taking and sharing screenshots
*/
using System.Collections;
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
        // call easymobile api
        if (!RuntimeManager.IsInitialized())
            RuntimeManager.Init();

        // get button component
        camButton = GetComponent<Button>();
        // listen for button press
        camButton.onClick.AddListener(() => SS());
        yield return null;
    }

    private void SS()
    {
        // goes to Coroutine that captures and saves a screenshot
        StartCoroutine(SaveScreenshot());
    }

    // Coroutine that captures and saves a screenshot
    IEnumerator SaveScreenshot()
    {
        // Wait till the last possible moment before screen rendering to hide the UI
        yield return null;

        // hide UI to make screenshot look pretty
        GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Canvas");

        // loop through and hide each canvas object
        foreach (GameObject go in gameObjectArray)
        {
            go.GetComponent<Canvas>().enabled = false;
        }

        // Wait until the end of frame
        yield return new WaitForEndOfFrame();


        // The SaveScreenshot() method returns the path of the saved image
        // The provided file name will be added a ".png" extension automatically
        string filename = "ARc-Light_Screenshot" + DateTime.Now.Ticks;

        filename = Sharing.SaveScreenshot(filename);

        // Wait until the end of frame
        yield return new WaitForEndOfFrame();
        Sharing.ShareImage(filename, "This is a sample message");
        //string filePath = Sharing.ShareScreenshot(filename, "This is a sample message", "Hello");
        Debug.Log("filePath created: " + filename);

        // Show UI after we're done
        foreach (GameObject go in gameObjectArray)
        {
            go.GetComponent<Canvas>().enabled = true;
        }
    }



}
