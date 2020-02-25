using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenuTextScript : MonoBehaviour
{
    public string title = "Title";

    private void Start()
    {
        transform.GetComponent<TextMesh>().text = title;
    }

    public void ChangeTitle(string newTitle)
    {
        transform.GetComponent<TextMesh>().text = newTitle;
    }

    public void CloseMenu()
    {
        Destroy(this);
    }
}
