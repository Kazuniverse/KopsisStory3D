using UnityEngine;

public class Interact_Trash : OnRaycast
{
    public Transform carryPosition; // Position in front of the player where the object will be carried
    public GameObject objectToPickup;
    private bool isCarryingObject = false;

    // Variabel publik untuk mengatur posisi relatif objek yang di-pickup
    public Vector3 customPickupPosition = Vector3.zero;
    public Vector3 customPickupRotation = Vector3.zero;
    public AudioClip pickup;

    private AudioSource pickupsound;

    void Start()
    {
        pickupsound = gameObject.AddComponent<AudioSource>();
    }

    public override void OnInteract()
    {
        // Periksa apakah carryPosition sudah memiliki anak (objek yang sedang dibawa)
        if (carryPosition.childCount > 0)
        {
            Debug.Log("Cannot pick up another object. Already carrying one.");
            return;
        }

        gameObject.layer = LayerMask.NameToLayer("AlwaysSee");
        pickupsound.clip = pickup;
        pickupsound.Play();
        PickUpObject();
    }

    private void PickUpObject()
    {
        if (objectToPickup != null)
        {
            Debug.Log("Picking up object: " + objectToPickup.name);
            isCarryingObject = true;
            objectToPickup.transform.SetParent(carryPosition);
            objectToPickup.transform.localPosition = customPickupPosition;
            objectToPickup.transform.localRotation = Quaternion.Euler(customPickupRotation);
            objectToPickup.GetComponent<Collider>().enabled = false;
        }
        else
        {
            Debug.LogError("objectToPickup is not assigned!");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the localized string changed event
    }

    void Update()
    {
    }
}
