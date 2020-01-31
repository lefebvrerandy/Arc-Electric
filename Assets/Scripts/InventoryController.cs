using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject InventoryPanel;

    public Animator inventoryAnim;
    public Animator inventoryAnimbtn;

    private bool inventoryOpen;

    private void Start()
    {
        InventoryPanel.SetActive(false);
    }


    public void InventoryOpenHandler()
    {
        if (inventoryAnim.GetBool("open"))
            CloseInventory();
        else
            OpenInventory();
    }

    private void OpenInventory()
    {
        InventoryPanel.SetActive(true);
        inventoryAnim.SetBool("open", true);
        inventoryAnimbtn.SetBool("open", true);
    }

    private void CloseInventory()
    {
        inventoryAnim.SetBool("open", false);
        inventoryAnimbtn.SetBool("open", false);
        StartCoroutine(DisablePanels());
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
}
