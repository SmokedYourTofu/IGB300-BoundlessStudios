using DeveloperToolbox;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ImpulseCollision : MonoBehaviour
{
    public float impulseStrength = 10f; // Strength of the impulse force
    public GameObject player;
    private NewPlayerController controller;
    private ParticleSystem thisparticle;
    public GameObject sceneCamera;
    private ScreenShake shaker;
    private SoundManager soundManager;
    public AudioClip[] HitEffects;

    private void Start()
    {
        controller = player.GetComponent<NewPlayerController>();
        thisparticle = GetComponent<ParticleSystem>();
        shaker = sceneCamera.GetComponent<ScreenShake>();
        soundManager = FindObjectOfType<SoundManager>();

        thisparticle.Pause();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the other object has a Rigidbody component
        Rigidbody otherRigidbody = collision.collider.attachedRigidbody;
        if (otherRigidbody != null)
        {
            thisparticle.Play();
            ScreenShake.AddShake(10f);
            soundManager.PlaySoundFXclip(HitEffects[Random.RandomRange(0, HitEffects.Length)], sceneCamera.transform, 0.2f);
            // Calculate the direction from this object to the other object
            Vector3 direction = (otherRigidbody.position - transform.position).normalized;
            direction.y = 0f;

            // Apply an impulse force to the other object in the calculated direction
            otherRigidbody.AddForce(direction * impulseStrength, ForceMode.Impulse);

            StartCoroutine(playerPause());
        }
    }

    private IEnumerator playerPause()
    {
        controller.enabled = false;
        yield return new WaitForSeconds(0.5f);
        controller.enabled = true;
    }
}
