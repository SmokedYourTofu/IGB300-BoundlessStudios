using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragCard : MonoBehaviour
{
    private bool isDragging = false;
    private float holdDistance;

    public float rotationSpeed = 1f; // Speed of rotation
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private Vector2 newTouchPosition;

    public float mouseSpeed;
    public GameObject endScreen;

    private Rigidbody rb;
    bool freeDrag = true;
    public GameObject secondCollider;
    public GameObject gameController;
    private CardController cardContoller;
    public GameObject outline;

    public AudioSource Sound;

    private void Start()
    {
        holdDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        rb = GetComponent<Rigidbody>();
        cardContoller = gameController.GetComponent<CardController>();
    }

    private void Update()
    {
        if (freeDrag)
        {
            Drag();
        }
        else
        {
            DragSlide();
        }
    }

#if UNITY_ANDROID
    private void Drag()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                startTouchPosition = touch.position;
            }
            else if ((touch.phase == TouchPhase.Moved) && isDragging)
            {
                currentTouchPosition = touch.position;
                Vector3 newPosition = new Vector3(currentTouchPosition.x, currentTouchPosition.y, holdDistance);
                newPosition = Camera.main.ScreenToWorldPoint(newPosition);
                transform.position = newPosition;
                
                if (newTouchPosition != null)
                {
                    mouseSpeed = Vector2.Distance(currentTouchPosition, newTouchPosition);
                }
                newTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }
#endif

#if !UNITY_ANDROID
    private void Drag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            currentTouchPosition = Input.mousePosition;
            Vector3 newPosition = new Vector3(currentTouchPosition.x, currentTouchPosition.y, holdDistance);
            newPosition = Camera.main.ScreenToWorldPoint(newPosition);
            transform.position = newPosition;
            if (newTouchPosition != null)
            {
                mouseSpeed = Vector2.Distance(currentTouchPosition, newTouchPosition);
            }
            newTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
#endif

#if UNITY_ANDROID
    private void DragSlide()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                startTouchPosition = touch.position;
            }
            else if ((touch.phase == TouchPhase.Moved) && isDragging)
            {
                currentTouchPosition = touch.position;
                Vector3 newPosition = new Vector3(currentTouchPosition.x, currentTouchPosition.y, holdDistance);
                newPosition = Camera.main.ScreenToWorldPoint(newPosition);
                newPosition.z = transform.position.z;
                newPosition.y = transform.position.y;
                transform.position = newPosition;
               
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }
#endif

#if !UNITY_ANDROID
    private void DragSlide()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            currentTouchPosition = Input.mousePosition;
            Vector3 newPosition = new Vector3(currentTouchPosition.x, currentTouchPosition.y, holdDistance);
            newPosition = Camera.main.ScreenToWorldPoint(newPosition);
            newPosition.z = transform.position.z;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
#endif

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Sound.Play();
            this.transform.position = other.transform.position;
            secondCollider.active = true;
            outline.active = false;
            freeDrag = false;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Goal")
        {
            StartCoroutine(cardContoller.FinishWait());
        }

    }
}
