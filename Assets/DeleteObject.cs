using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    [SerializeField]
    public GameObject ObjectToDestroy;

    /// <summary>
    /// Deletes the referenced object
    /// </summary>
    public void DestroyGameObject()
    {
        if(ObjectToDestroy == null)
        {
            throw new System.ArgumentNullException("Object to destroy is null");
        }

        Destroy(ObjectToDestroy);
    }
}