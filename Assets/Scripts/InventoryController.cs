/*
*  FILE          : InventoryController.cs
*  PROJECT       : PROG 3220 - Systems Project
*  PROGRAMMER    : Randy Lefebvre, Bence Karner, Lucas Roes, Kyle Horsley
*  DESCRIPTION   : DEBUG
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public GameObject HUDSelectedPanel;

    private List<GameObject> lightPrefabs = new List<GameObject>();
    private List<GameObject> lightPrefabsSelected = new List<GameObject>();
    private List<GameObject> instantiatedLightObjectsInventorypanel = new List<GameObject>();

    private void Start()
    {

        LoadLightsFromResources();
        LoadInventoryPanel();
        LoadHudPanel();

        CleanUpHUD();
        InventoryPanel.SetActive(false);
    }

    //*********************************************************************************************
    //
    //                                  Private Methods
    //
    //*********************************************************************************************

    public void LoadLightsFromResources()
    {
        // Load all the lights in the Resources/Lights folder
        var LightsObject = Resources.LoadAll("Lights", typeof(GameObject));
        foreach (var lightObject in LightsObject)
        {
            GameObject lightPrefab = lightObject as GameObject;
            lightPrefabs.Add(lightPrefab);
        }
    }

    private void LoadInventoryPanel()
    {
        int i = 0;
        foreach (var light in lightPrefabs)
        {
            i++;
            var test = lightPrefabs.Count;
            // Start creating the new Inventory item
            GameObject newItem = inventoryTemplate;

            // This is a fail safe incase we dont load anything in.
            if (newItem != null)
            {
                // Create the new light using the prefab list
                GameObject newLight = light;
                newLight.transform.eulerAngles = new Vector3(15f, 0f, 0f);

                // Instantiate and set the parent
                GameObject item = Instantiate(newItem, inventoryContentPanel.transform);
                GameObject lightObject = Instantiate(newLight, item.transform);

                // Set name of each item in List for the viewer
                item.GetComponentInChildren<Text>().text = newLight.name;

                // Set up the properties for each light
                lightObject.transform.localScale = new Vector3(50f, 50f, 50f);
                //lightObject.SetActive(true);
                lightObject.name = newLight.name;
                lightObject.SetActive(false);

                // Set up event handlers to each light
                string name = newLight.name;
                item.GetComponent<Button>().onClick.AddListener(() => UserController.SwitchSelected(newLight.name, lightPrefabsSelected));
                instantiatedLightObjectsInventorypanel.Add(lightObject);
            }
        }
    }

    private void CleanUpHUD()
    {
        // Disable all lights at launch
        foreach (var light in lightPrefabsSelected)
        {
            light.SetActive(false);
        }


        // Find the last used light and place that in the HUD
        if (PlayerPrefs.GetString("Selected") == "" || PlayerPrefs.GetString("Selected") == null)
        {
            lightPrefabsSelected[0].SetActive(true);
            PlayerPrefs.SetString("Selected", lightPrefabsSelected[0].name);
        }
        else
        {
            foreach (var light in lightPrefabsSelected)
            {
                string[] name = light.name.Split('(');
                if (name[0] == PlayerPrefs.GetString("Selected"))
                {
                    light.SetActive(true);
                }
            }
        }
    }

    private void LoadHudPanel()
    {
        foreach (var light in lightPrefabs)
        {
            GameObject newLight = light;
            newLight.transform.localScale = new Vector3(50f, 50f, 50f);
            lightPrefabsSelected.Add(Instantiate(newLight, HUDSelectedPanel.transform));
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
            OpenInventory();
    }
}
