using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePanels : MonoBehaviour
{
    public GameObject Heirarchy;
    //public Animator HeirarchyAnimator;
    public Animator PanelAnimator;
    public GameObject Inspector;
    //public Animator InspectorAnimator;
    public Text text;

    private bool heirarchyActive;
    private bool inspectorActive;

    private string openText  = "Click to OPEN Hierarchy/Inspector Panels";
    private string closeText = "Click to CLOSE Hierarchy/Inspector Panels";
    // Start is called before the first frame update
    void Start()
    {
        Heirarchy.SetActive(false);
        Inspector.SetActive(false);
    }

    public void ButtonHandler()
    {
        if (PanelAnimator.GetBool("open"))
            ClosePanels();
        else
            OpenPanels();
    }

    private void OpenPanels()
    {
        text.text = closeText;
        Heirarchy.SetActive(true);
        Inspector.SetActive(true);
        PanelAnimator.SetBool("open", true);

    }

    private void ClosePanels()
    {
        text.text = openText;
        PanelAnimator.SetBool("open", false);
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

        if (!PanelAnimator.GetBool("open"))
        {
            Heirarchy.SetActive(false);
            Inspector.SetActive(false);
        }
    }
}
