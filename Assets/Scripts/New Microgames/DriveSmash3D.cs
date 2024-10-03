using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveSmash3D : MonoBehaviour
{
    public DragController dragControl;
    public DragObjectNoEnd dragObject;
    public ParticleSystem particles;
    public ParticleSystem particles1;

    public AudioSource[] hammerSounds;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Drive")
        {
            hammerSounds[UnityEngine.Random.Range(0, hammerSounds.Length)].Play();
            particles.Play();
            particles1.Play();
            if (dragObject.mouseSpeed > 50f)
            {
                StartCoroutine(dragControl.FinishWait());
            } 
        }
    }
}