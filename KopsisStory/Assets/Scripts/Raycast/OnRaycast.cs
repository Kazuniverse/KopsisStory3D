using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnRaycast : MonoBehaviour
{
    public string information;
    public Transform pointInteract;
    public virtual void OnInteract () {}
}
