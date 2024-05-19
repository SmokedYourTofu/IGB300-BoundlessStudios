using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Components")]
    public VariableJoystick variableJoystick;
    public Rigidbody rb;
    public Transform orientation;

    [Header("Movement")]
    public float moveSpeed;
    public float currentSpeed;
    public float rotationSpeed;
    public float drag;

    [Header("Dashing")]
    public float dashForce;
    public float dashCooldown;
    public float dashCooldownTimer;
    public bool dashing;

    [Header("Recovering")]
    public float recoverCooldown;
    public float recoverCooldownTimer;
    public bool recovering;

    // Start is called before the first frame update
    void Start()
    {
        rb.drag = drag;
    }

    // Update is called once per frame
    void Update()
    {
        currentSpeed = rb.velocity.magnitude;

        if (!dashing) {
            if (currentSpeed > moveSpeed)
            {
                currentSpeed = moveSpeed;
            }
        }
        

        // Timer for dash cooldown
        if (dashCooldownTimer > 0) {
            dashCooldownTimer -= Time.deltaTime;
        }

        // Timer for dash cooldown
        if (recoverCooldownTimer > 0 && recovering) {
            recoverCooldownTimer -= Time.deltaTime;
        } else if (recoverCooldownTimer <= 0 && recovering) {
            rb.drag = drag;
            recovering = false;
            dashing = false;
        }
    }

    private void FixedUpdate() {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        rb.AddForce(direction.normalized * moveSpeed * direction.magnitude * 6f, ForceMode.Force);

        if (direction != Vector3.zero) {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    public void Dashing() {
        if (dashCooldownTimer > 0) return;
        else dashCooldownTimer = dashCooldown;

        dashing = true;

        Vector3 dash = orientation.forward * dashForce;
        rb.AddForce(dash, ForceMode.Impulse);

        recoverCooldownTimer = recoverCooldown;
        rb.drag = 15f;
        recovering = true;
    }
}
