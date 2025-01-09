using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : OnRaycast
{
    float openAngle = 120f;
    float closeAngle = 0;

    float currentAngle, speed = 2.5f;

    bool isOpen;
    Collider col;

    private void Start () {
        col = GetComponent<Collider>();
    }

    private void Update () {
        if(isOpen) {
            information = "Close";

            currentAngle = openAngle;
        } else {
            information = "Open";

            currentAngle = closeAngle;
        }
    }

    private void LateUpdate () {
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0, currentAngle, 0), speed);

        col.enabled = transform.localRotation == Quaternion.Euler(0, currentAngle, 0);
    }

    public override void OnInteract () {
        isOpen = !isOpen;
    }
}
