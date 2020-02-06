using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class LightMenuToggle : MonoBehaviour
{
    public GameObject panel;
    public Button menuButton;

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
        menuButton = GetComponent<Button>();
        menuButton.onClick.AddListener(() => PanelToggle(panel));
    }

    private void Update()
    {
        //menuButton.onClick.AddListener(() => PanelToggle(panel));
    }

    public void PanelToggle(GameObject panel)
    {
        Debug.Log("Button Pressed!!!)");
        if (panel.activeInHierarchy)
        {
            panel.SetActive(false);

        }
        else
        {
            panel.SetActive(true);
        }

    }

    void Destroy()
    {
        menuButton.onClick.RemoveListener(() => PanelToggle(panel));
    }
}
