using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements.Experimental;

public class NewPlayerController : MonoBehaviour
{
    [Header("Player Components")]
    public VariableJoystick joystick;
    public Rigidbody rb;
    public Transform orientation;

    [Header("Movement")]
    public float moveSpeed;
    private const string IS_MOVING = "IsMoving";
    private bool isMoving;
    public float drag;

    [Header("Dashing")]
    public float dashForce;
    public float dashCooldown;
    public float dashCooldownTimer;
    public bool dashing;
    public AudioClip dashingSFX;
    public GameObject dashVFX;
    public GameObject dashVFXSpawn;

    [Header("Recovering")]
    public float recoverCooldown;
    public float recoverCooldownTimer;
    public bool recovering;

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Plays animation for running and standing still
        animator.SetBool(IS_MOVING, IsMoving());
        HandleMovement();

        // Timer for dash cooldown
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // Timer for dash cooldown
        if (recoverCooldownTimer > 0 && recovering)
        {
            recoverCooldownTimer -= Time.deltaTime;
        }
        else if (recoverCooldownTimer <= 0 && recovering)
        {
            rb.drag = drag;
            recovering = false;
            dashing = false;
        }


    }

    private void HandleMovement() {
        Vector3 direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;

        Vector3 moveDir = new Vector3(direction.x, 0f, direction.z);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove) {
            // cannot move towards moveDir

            // attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove) {
                // can move only on the X
                moveDir = moveDirX;
            }
            else {
                // cannot move only on the X

                // attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove) {
                    // can move only on the Z
                    moveDir = moveDirZ;
                }
                else {
                    // cannot move in any direction
                }
            }
        }
        if (canMove) {
            transform.position += moveDir * moveDistance;
        }

        isMoving = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public bool IsMoving() {
        return isMoving;
    }
    public void Dashing()
    {
        if (dashCooldownTimer > 0) return;
        else dashCooldownTimer = dashCooldown;

        dashing = true;

        //SoundManager.instance.PlaySoundFXclip(dashingSFX, transform, 1f);

        rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);

        Instantiate(dashVFX, dashVFXSpawn.transform);
        recoverCooldownTimer = recoverCooldown;
        rb.drag = 15f;
        recovering = true;
    }

}
