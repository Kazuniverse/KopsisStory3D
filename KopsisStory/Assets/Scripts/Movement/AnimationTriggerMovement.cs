using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AnimationTriggerMovement : MonoBehaviour
{
    AudioSource audioSource;
    Movement movement;
    int walkFootStep;

    void Start () {
        audioSource = GetComponent<AudioSource>();
        movement = GetComponentInParent<Movement>();
        audioSource.clip = movement.audioWalk;
    }

    public void SoundWalk () {
        if(walkFootStep == 0) {
            audioSource.pitch = .9f;
            walkFootStep = 1;
        } else {
            audioSource.pitch = 1f;
            walkFootStep = 0;
        }

        audioSource.Play();
    }

    public void PlaySound () {
        audioSource.PlayOneShot(movement.audioGrounded);
    }
}
