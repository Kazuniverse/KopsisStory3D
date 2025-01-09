using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : OnRaycast
{
    public override void OnInteract () {
        Destroy(this.gameObject);
    }
}
