using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateParticles : MonoBehaviour
{
    private NewInteract interact;
    private ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        interact = GetComponentInChildren<NewInteract>();
        particles.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (interact.enabled == true)
        {
            particles.Play();
        }
        else
        {
            particles.Pause();
        }
    }
}
