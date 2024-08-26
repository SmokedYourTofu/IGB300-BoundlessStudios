using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseCollision : MonoBehaviour
{
    public float impulseStrength = 10f; // Strength of the impulse force
    public GameObject player;
    private NewPlayerController controller;

    private void Start()
    {
        controller = player.GetComponent<NewPlayerController>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the other object has a Rigidbody component
        Rigidbody otherRigidbody = collision.collider.attachedRigidbody;
        if (otherRigidbody != null)
        {
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
