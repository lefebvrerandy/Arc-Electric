/*
*  FILE          : InventoryController.cs
*  PROJECT       : PROG 3220 - Systems Project
*  PROGRAMMER    : Randy Lefebvre, Bence Karner, Lucas Roes, Kyle Horsley
*  DESCRIPTION   : DEBUG
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


/*
*   NAME    : InventoryController
*   PURPOSE : Responsible for controlling the UI 
*/
public class InventoryController : MonoBehaviour
{
    public GameObject InventoryPanel;

    public Animator inventoryAnim;
    public Animator inventoryAnimbtn;

    private bool inventoryOpen;

    public GameObject inventoryContentPanel;
    public GameObject inventoryTemplate;

    private List<Tuple<GameObject, string>> lightPrefabs = new List<Tuple<GameObject, string>>();
    private List<GameObject> lightPrefabsSelected = new List<GameObject>();
    private static List<Tuple<GameObject, string>> instantiatedLightObjectsInventorypanel = new List<Tuple<GameObject, string>>();

    public static UnityEvent selectionChangedEvent;

    private void Start()
    {
        // Lets start the inventory panel as disabled
        InventoryPanel.SetActive(false);


        // Here we set an event on selection changed. This will handle closing the panel when a selection has been made
        if (selectionChangedEvent == null)
            selectionChangedEvent = new UnityEvent();
        selectionChangedEvent.AddListener(InventoryOpenHandler);
    }

    //*********************************************************************************************
    //
    //                                  Private Methods
    //
    //*********************************************************************************************

    public void LoadLightsFromResources(string folder)
    {
        // Load all the lights in the Resources/Lights folder
        var LightsObject = Resources.LoadAll("Lights/" + folder, typeof(GameObject));
        foreach (var lightObject in LightsObject)
        {
            GameObject lightPrefab = lightObject as GameObject;
            var tuple = new Tuple<GameObject, string>(lightPrefab, folder);
            lightPrefabs.Add(tuple);
        }
    }

    private void LoadInventoryPanel()
    {
        foreach (var light in lightPrefabs)
        {
            // Start creating the new Inventory item
            GameObject newItem = inventoryTemplate;

            // This is a fail safe incase we dont load anything in.
            if (newItem != null)
            {
                // Create the new light using the prefab list
                GameObject newLight = light.Item1;

                // Adjust the rotation of the light
                newLight.transform.eulerAngles = new Vector3(0f, 0f, 0f);

                // Instantiate and set the parent
                GameObject item = Instantiate(newItem, inventoryContentPanel.transform);
                GameObject lightObject = Instantiate(newLight, item.transform);

                // Set name of each item in List for the viewer
                string[] splitName = newLight.name.Split('+');
                splitName[0] = splitName[0].Replace('_', ' ');
                item.GetComponentInChildren<Text>().text = splitName[0];

                // Set up the properties for each light
                if (light.Item2 == "Ceiling")
                {
                    lightObject.transform.localScale = new Vector3(50f, 50f, 50f);
                    lightObject.transform.eulerAngles = new Vector3(15f, 0f, 0f);
                }
                else if (light.Item2 == "Floor")
                {
                    lightObject.transform.localScale = new Vector3(50f, 10f, 10f);
                    lightObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    lightObject.transform.localPosition = new Vector3(lightObject.transform.localPosition.x, -93f, lightObject.transform.localPosition.z);
                }
                else if (light.Item2 == "Wall")
                {
                    lightObject.transform.localScale = new Vector3(50f, 50f, 50f);
                    lightObject.transform.eulerAngles = new Vector3(180f, 0f, 0f);
                    lightObject.transform.localPosition = new Vector3(lightObject.transform.localPosition.x, -41.5f, lightObject.transform.localPosition.z);
                }
                else
                {
                    lightObject.transform.localScale = new Vector3(50f, 50f, 50f);
                    lightObject.transform.eulerAngles = new Vector3(15f, 0f, 0f);
                }

                lightObject.name = newLight.name;
                lightObject.SetActive(true);

                // Set up event handlers to each light
                string name = newLight.name;
                item.GetComponent<Button>().onClick.AddListener(() => UserController.SwitchSelected(newLight.name));
                var tuple = new Tuple<GameObject, string>(lightObject, light.Item2);
                instantiatedLightObjectsInventorypanel.Add(tuple);
            }
        }
    }

    private void OpenInventory()
    {
        InventoryPanel.SetActive(true);
        inventoryAnim.SetBool("open", true);
        inventoryAnimbtn.SetBool("open", true);
        StartCoroutine(EnableImages());
    }

    private void CloseInventory()
    {
        inventoryAnim.SetBool("open", false);
        inventoryAnimbtn.SetBool("open", false);
        StartCoroutine(DisablePanels());
        StartCoroutine(DisableImages());
    }

    IEnumerator EnableImages()
    {
        int counter = 1;
        while (counter > 0)
        {
            yield return new WaitForSeconds(0.4f);
            counter--;
        }
        foreach(var light in instantiatedLightObjectsInventorypanel)
        {
            if (light != null)
                light.Item1.SetActive(true);
        }
    }

    IEnumerator DisableImages()
    {
        int counter = 1;
        while (counter > 0)
        {
            yield return new WaitForSeconds(0.3f);
            counter--;
        }
        foreach (var light in instantiatedLightObjectsInventorypanel)
        {
            light.Item1.SetActive(false);
        }
    }

    IEnumerator DisablePanels()
    {
        int counter = 1;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
        }

        if (!inventoryAnim.GetBool("open"))
        {
            InventoryPanel.SetActive(false);
        }
    }

    //*********************************************************************************************
    //
    //                                  Public Methods
    //
    //*********************************************************************************************
    public void InventoryOpenHandler()
    {
        if (inventoryAnim.GetBool("open"))
            CloseInventory();
        else
        {
            // Enable folder buttons and child old inventory items
            foreach (Transform child in inventoryContentPanel.transform)
            {
                if (child.gameObject.name.Contains("Clone"))
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    child.gameObject.SetActive(true);
                }
            }

            OpenInventory();
        }
    }

    public void LoadLights(string folder)
    {
        // Clear any lights that are currently stored before loading new lights
        lightPrefabs.Clear();
        instantiatedLightObjectsInventorypanel.Clear();

        switch (folder)
        {
            case "Ceiling":
                LoadLightsFromResources("Ceiling");
                break;
            case "Floor":
                LoadLightsFromResources("Floor");
                break;
            case "Wall":
                LoadLightsFromResources("Wall");
                break;
        }

        // Disable folder buttons
        foreach (Transform child in inventoryContentPanel.transform)
        {
            child.gameObject.SetActive(false);
        }

        LoadInventoryPanel();
    }

    public static Tuple<GameObject,string> GetSelectedLight(string name)
    {
        //  Find the selected light from the list, given the name
        GameObject newLight = new GameObject();
        string folder = string.Empty;
        foreach (var light in instantiatedLightObjectsInventorypanel)
        {
            if (light.Item1.name == name)
            {
                newLight = light.Item1;
                folder = light.Item2;
                break;
            }
        }
        if (newLight != null)
        {
            // Strip off (Clone)
            string[] stripClone = newLight.name.Split('(');
            newLight.name = stripClone[0];

            // Create that new GameObject and return it
            return new Tuple<GameObject, string>(Instantiate(newLight), folder);
        }
        else
            return null;
    }
}
