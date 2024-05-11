using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerControllerOld : MonoBehaviour
{
    [Header("Player Components")]
    public VariableJoystick variableJoystick;
    public Rigidbody rb;
    public Transform orientation;

    [Header("Movement")]
    public float baseSpeed;
    public float currentSpeed;
    public float rotationSpeed;
    public float speed;

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

    public bool isDashing;
    public bool isRecovering;
    public bool hasDashed;

    public MovementState state;

    public enum MovementState {
        moving,
        dashing,
        recovering,
    }


    private void Update() {
        if(dashCooldownTimer > 0) {
            dashCooldownTimer -= Time.deltaTime;
        }

        if(speedPenaltyTimer > 0) {
            speedPenaltyTimer -= Time.deltaTime;
            isRecovering = true;
            currentSpeed = speedPenalty;
        } else {
            currentSpeed = baseSpeed;
            isRecovering = false;
        }

        StateHandler();

        speed = rb.velocity.magnitude;
    }

    private void StateHandler() {
        if (isRecovering && hasDashed) {
            state = MovementState.recovering;
            hasDashed = false;
            isDashing = false;
            Debug.Log(state);
        } else if (isDashing) {
            state = MovementState.dashing;
            hasDashed = true;
            Debug.Log(state);
        } else if (!isDashing && !hasDashed) {
            state = MovementState.moving;
        }
        
    }

    private void FixedUpdate() {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        rb.AddForce(direction * currentSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

        if(direction != Vector3.zero) {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime);

        }
    }

    // might move interaction to its own script later since this script will start to get long
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

        isDashing = true;

        currentSpeed = speedPenalty;
        speedPenaltyTimer = speedPenaltyDuration;
    }
}
