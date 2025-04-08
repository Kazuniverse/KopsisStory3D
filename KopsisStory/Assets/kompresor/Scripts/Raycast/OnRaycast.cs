using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnRaycast : MonoBehaviour
{
    public string information;
    public Transform pointInteract;
    public bool HasInteracted { get; protected set; } = false;
    public virtual void OnInteract ()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}
