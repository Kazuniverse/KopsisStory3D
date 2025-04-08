using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : OnRaycast
{
    void Start () {
        information = "Take Cube";
    }

    public override void OnInteract () {
        Destroy(gameObject);
    }
}
