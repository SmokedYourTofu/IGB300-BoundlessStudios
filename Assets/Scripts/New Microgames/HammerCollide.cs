using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class HammerCollide : MonoBehaviour
{
    public ParticleSystem particles;
    public DragController dragControl;

    //when the hammer hits the drive, finish the game
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Drive")
        {
            particles.Play();
            StartCoroutine(dragControl.FinishWait());
        }
    }
}
