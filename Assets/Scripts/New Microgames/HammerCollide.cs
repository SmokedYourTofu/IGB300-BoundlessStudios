using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class HammerCollide : MonoBehaviour
{
    public ParticleSystem particles;
    public DragController dragControl;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Drive")
        {
            particles.Play();
            StartCoroutine(dragControl.FinishWait());
        }
    }
}