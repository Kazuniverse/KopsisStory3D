using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class Pintu : OnRaycast
{
    // Public properties to set the angles from Inspector
    public float openAngleX = 0f;
    public float openAngleY = 0f;
    public float openAngleZ = 0f;

    public float closeAngleX = 0f;
    public float closeAngleY = 0f;
    public float closeAngleZ = 0f;

    public float speed = 2.5f;

    private Vector3 openAngle;
    private Vector3 closeAngle;
    private Vector3 currentAngle;

    public bool isOpen;
    private Collider col;

    // Localized strings
    public LocalizedString openText;
    public LocalizedString closeText;

    // Audio clips for open and close sounds
    public AudioClip openSound;
    public AudioClip closeSound;
    private AudioSource audioSource;

    private void Start()
    {
        col = GetComponent<Collider>();
        openAngle = new Vector3(openAngleX, openAngleY, openAngleZ);
        closeAngle = new Vector3(closeAngleX, closeAngleY, closeAngleZ);
        currentAngle = closeAngle;

        // Set initial rotation
        transform.localRotation = Quaternion.Euler(-90, 0, 180);

        // Initialize audio source
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (isOpen)
        {
            information = closeText.GetLocalizedString();
            currentAngle = openAngle;
        }
        else
        {
            information = openText.GetLocalizedString();
            currentAngle = closeAngle;
        }
    }

    private void LateUpdate()
    {
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(currentAngle), speed * Time.deltaTime);

        col.enabled = transform.localRotation == Quaternion.Euler(currentAngle);
    }

    public override void OnInteract()
    {
        isOpen = !isOpen;
        PlaySound();
    }

    private void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.clip = isOpen ? openSound : closeSound;
            audioSource.Play();
        }
    }
}
