/*
*  FILE          : InventoryController.cs
*  PROJECT       : PROG 3220 - Systems Project
*  PROGRAMMER    : Randy Lefebvre, Bence Karner, Lucas Roes, Kyle Horsley
*  DESCRIPTION   : DEBUG
*/


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

    private List<GameObject> lightPrefabs = new List<GameObject>();
    private List<GameObject> lightPrefabsSelected = new List<GameObject>();
    private static List<GameObject> instantiatedLightObjectsInventorypanel = new List<GameObject>();

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
            lightPrefabs.Add(lightPrefab);
        }
    }

    private void LoadInventoryPanel()
    {
        foreach (var light in lightPrefabs)
        {
            var test = lightPrefabs.Count;
            // Start creating the new Inventory item
            GameObject newItem = inventoryTemplate;

            // This is a fail safe incase we dont load anything in.
            if (newItem != null)
            {
                // Create the new light using the prefab list
                GameObject newLight = light;

                // Adjust the rotation of the light
                newLight.transform.eulerAngles = new Vector3(0f, 0f, 0f);

                // Instantiate and set the parent
                GameObject item = Instantiate(newItem, inventoryContentPanel.transform);
                GameObject lightObject = Instantiate(newLight, item.transform);

                // Set name of each item in List for the viewer
                //item.GetComponentInChildren<Text>().text = newLight.name;

                // Set up the properties for each light
                lightObject.transform.localScale = new Vector3(10f, 10f, 10f);
                
                lightObject.name = newLight.name;
                lightObject.SetActive(true);

                // Set up event handlers to each light
                string name = newLight.name;
                item.GetComponent<Button>().onClick.AddListener(() => UserController.SwitchSelected(newLight.name));
                instantiatedLightObjectsInventorypanel.Add(lightObject);
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
                light.SetActive(true);
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
            light.SetActive(false);
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
//DELETING HUD        //LoadHudPanel();
        //CleanUpHUD();
    }

    public static GameObject GetSelectedLight(string name)
    {
        //  Find the selected light from the list, given the name
        GameObject newLight = instantiatedLightObjectsInventorypanel.Where(obj => obj.name == name).SingleOrDefault();
        if (newLight != null)
        {
            // Strip off (Clone)
            string[] stripClone = newLight.name.Split('(');
            newLight.name = stripClone[0];

            // Create that new GameObject and return it
            GameObject newInstantiatedLight = Instantiate(newLight);
            return newInstantiatedLight;
        }
        else
            return null;
    }
}
