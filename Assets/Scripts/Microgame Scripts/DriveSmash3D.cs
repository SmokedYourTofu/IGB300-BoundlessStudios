using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveSmash3D : MonoBehaviour
{
    public Camera camera_2;
    public Camera camera_1;
    public GameObject environment;
    public GameObject controls;
    public GameObject player;
    private Vector3 startPos;

    private AudioSource source;

    private void Start()
    {
        source = this.GetComponent<AudioSource>();
        startPos = transform.parent.position;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Drive")
        {
            source.Play();
            //do points and such

            transform.parent.position = startPos;
            transform.parent.transform.parent.gameObject.SetActive(false);
            camera_1.gameObject.SetActive(true);
            camera_2.gameObject.SetActive(false);
            environment.SetActive(true);
            controls.SetActive(true);
            player.SetActive(true);
        }
    }
}
