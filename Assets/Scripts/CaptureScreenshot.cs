using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;
using System;

public class CaptureScreenshot : MonoBehaviour
{
    
    public Button camButton;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        Debug.Log("In Start method");
        camButton = GetComponent<Button>();
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
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;

        // Wait until the end of frame
        yield return new WaitForEndOfFrame();


        // The SaveScreenshot() method returns the path of the saved image
        // The provided file name will be added a ".png" extension automatically
        string filename = "ARc-Light_Screenshot" + DateTime.Now.Ticks;
        
        string filePath = Sharing.ShareScreenshot(filename, "This is a sample message", "Hello");
        Debug.Log("filePath created: " + filePath);

        // Show UI after we're done
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;

    }

    

}
