using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public float range;
    public LayerMask interacted;

    public GameObject buttonRaycast;
    public Transform interactRaycast;
    public UnityEngine.UI.Text textInteract;
    public Camera cam;

    bool isRaycast;
    OnRaycast onRaycast;
    RaycastHit hit;
    ControllerMode controllerMode;

    private void Start () {
        controllerMode = GetComponentInParent<Movement>().readControllerMode;
    }

    private void Update () {
        isRaycast = Physics.Raycast(transform.position, transform.forward, out hit, range, interacted);
        buttonRaycast.SetActive(isRaycast);
        interactRaycast.gameObject.SetActive(isRaycast);

        if(isRaycast) {
            onRaycast = hit.transform.GetComponent<OnRaycast>();

            Vector3 pointRaycast = Vector3.zero;

            if(onRaycast.pointInteract == null) {
                pointRaycast = hit.transform.position;
            } else {
                pointRaycast = onRaycast.pointInteract.position;
            }

            if(Input.GetKeyDown(KeyCode.Mouse0) && controllerMode == ControllerMode.PC)
                onRaycast.OnInteract();

            interactRaycast.transform.position = cam.WorldToScreenPoint(pointRaycast);
            textInteract.text = onRaycast.information;
        }
    }

    public void Interact () {
        if(!isRaycast) return;

        onRaycast.OnInteract();
    }
}
