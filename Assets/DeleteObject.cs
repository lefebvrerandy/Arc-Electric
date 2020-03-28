using UnityEngine;

/// <summary>
/// Simple class to allow an object to delete itself
/// </summary>
public class DeleteObject : MonoBehaviour
{
    [SerializeField]
    public GameObject ObjectToDestroy;


    /// <summary>
    /// Destroy the target object when called
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