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

    public MovementState state;

    public enum MovementState {
        moving,
        dashing,
        recovering,
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.drag = drag;
    }

    // Update is called once per frame
    void Update()
    {
        StateHandler();
        currentSpeed = rb.velocity.magnitude;
    }

    private void FixedUpdate() {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        rb.AddForce(direction.normalized * moveSpeed * 10f, ForceMode.Force);

        if (direction != Vector3.zero) {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void StateHandler() {
        // if dash is pressed change to dashing
        // if drag is = to 10 change to recovering
        // else change to moving
    }

    public void Dashing() {
        Vector3 dash = orientation.forward * dashForce;
        rb.AddForce(dash, ForceMode.Impulse);
    }
}
