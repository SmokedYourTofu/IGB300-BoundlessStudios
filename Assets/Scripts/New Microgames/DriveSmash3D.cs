using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveSmash3D : MonoBehaviour
{
    public DragController dragControl;
    public DragObjectNoEnd dragObject;
    public ParticleSystem particles;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Drive")
        {
            particles.Play();
            if (dragObject.touchSpeed < 0.0014f)
            {
                StartCoroutine(dragControl.FinishWait());
            } 
        }
    }
}
