using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Dumpster : OnRaycast
{
    public Transform carryPosition;
    public AudioClip putdown;


    private GameObject carriedObject;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    public override void OnInteract()
    {
        audioSource.clip = putdown;
        audioSource.Play();
        if (carryPosition.childCount > 0)
        {
            carriedObject = carryPosition.GetChild(0).gameObject;

            if (carriedObject != null)
            {
                // Hancurkan objek yang dibawa
                Destroy(carriedObject);

                // Tambahkan objective ke objective manager
                Debug.Log("Object dropped and destroyed: " + carriedObject.name);
            }
        }
       
    }
}
