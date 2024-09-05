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
    public SoundManager soundManager;

    [Header("Movement")]
    public float moveSpeed;
    private const string IS_MOVING = "IsMoving";
    private bool isMoving;
    public float drag;

    [Header("Dashing")]
    public bool isDashing;
    public float dashDis;
    public float dashTime;
    public float dashCooldown;
    public GameObject dashVFX;
    public AudioClip dashAudio;

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
        StartCoroutine(Dash());

        // Dash mechanic on PC
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > dashTime) {
            IsDashing();
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
        if (canMove && isDashing) {
            transform.forward += moveDir * moveDistance;
        } else {
            transform.position += moveDir * moveDistance;
        }

        isMoving = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public bool IsMoving() {
        return isMoving;
    }

    public void IsDashing() {
        if (Time.time > dashTime) {
            isDashing = true;
            dashTime = Time.time + dashCooldown;
            
            soundManager.PlaySoundFXclip(dashAudio, transform, 1);
        }
    }

    private IEnumerator Dash() {
        if (isDashing) {
            moveSpeed += dashDis;
            yield return new WaitForSeconds(0.2f);
            moveSpeed -= dashDis;
            isDashing = false;
            GameObject dashVisual = Instantiate(dashVFX, transform.position, transform.rotation);
            Destroy(dashVisual, 0.2f);
        }
    }
}
