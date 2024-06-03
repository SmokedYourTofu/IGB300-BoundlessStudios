using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseCollision : MonoBehaviour
{
    public float impulseStrength = 10f; // Strength of the impulse force

    void OnCollisionEnter(Collision collision)
    {
        // Check if the other object has a Rigidbody component
        Rigidbody otherRigidbody = collision.collider.attachedRigidbody;
        if (otherRigidbody != null)
        {
            // Calculate the direction from this object to the other object
            Vector3 direction = (otherRigidbody.position - transform.position).normalized;

            // Apply an impulse force to the other object in the calculated direction
            otherRigidbody.AddForce(direction * impulseStrength, ForceMode.Impulse);
        }
    }
}
