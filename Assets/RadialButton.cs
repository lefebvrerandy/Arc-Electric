using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class RadialButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image background;    
    public Image icon;          
    public string title;        
    public RadialMenu parentMenu;


    //Required for the IPointerEnterHandler
    public void OnPointerEnter(PointerEventData eventData)
    {
        //When the pointer enters a buttons space, mark that button as selected
        parentMenu.selectedButton = this;
    }


    //Required for the IPointerExitHandler
    public void OnPointerExit(PointerEventData eventData)
    {
        //When the pointer leaves the buttons space, deselect it in the menu
        parentMenu.selectedButton = null;
    }
}
