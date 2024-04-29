using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Components")]
    public VariableJoystick variableJoystick;
    public Rigidbody rb;
    public Transform orientation;

    [Header("Movement")]
    public float baseSpeed;
    public float currentSpeed;
    public float rotationSpeed;

    [Header("Dashing")]
    public float dashForce;
    public float dashCooldown;
    public float dashCooldownTimer;
    public float speedPenalty;
    public float speedPenaltyTimer;
    public float speedPenaltyDuration;

    public Camera camera_1;
    public Camera camera_2;
    public GameObject environment;
    public GameObject microgame1;
    public GameObject controls;

    private void Update() {
        if(dashCooldownTimer > 0) {
            dashCooldownTimer -= Time.deltaTime;
        }

        if(speedPenaltyTimer > 0) {
            speedPenaltyTimer -= Time.deltaTime;
        } else {
            currentSpeed = baseSpeed;
        }
    }

    private void FixedUpdate() {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        rb.AddForce(direction * currentSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

        if(direction != Vector3.zero) {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void Interact() {
        Debug.Log("Interacted");
        //if (Physics.SphereCast())
        camera_2.gameObject.SetActive(true);
        microgame1.gameObject.SetActive(true);
        camera_1.gameObject.SetActive(false);
        environment.SetActive(false);
        controls.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void Dash() {
        // checks if dashing is on cooldown
        if (dashCooldownTimer > 0) return;
        else dashCooldownTimer = dashCooldown;

        // Apply Dash force to player
        Vector3 dash = orientation.forward * dashForce;
        rb.AddForce(dash, ForceMode.VelocityChange);

        speedPenaltyTimer = speedPenaltyDuration;
        currentSpeed = speedPenalty;
    }
}
