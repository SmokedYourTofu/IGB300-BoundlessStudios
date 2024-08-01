using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PopupScript : MonoBehaviour
{
#if UNITY_ANDROID
    public AudioSource[] passwordAudio = new AudioSource[2];
    Rigidbody rigid;

    private bool isDragging = false;

    private float offset;

    private Material _mat;
    public Color color;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        _mat = renderer.material;

        rigid = this.GetComponent<Rigidbody>();
        offset = Vector3.Distance(transform.position, Camera.main.transform.position);

        this.transform.localScale = new Vector3(Random.Range(5, 8), Random.Range(5, 8), 0.05f);
        this.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(398, 406), 1.1f);
        color = new Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));
        _mat.color = color;
    }

    private void Update()
    {
        Drag();
    }

    private void Drag()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        isDragging = true;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (isDragging)
                {
                    rigid.velocity = Vector2.zero;
                    Vector3 newPosition = new Vector3(touch.position.x, touch.position.y, offset);
                    newPosition = Camera.main.ScreenToWorldPoint(newPosition);
                    transform.position = newPosition;
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }

#endif

#if !UNITY_ANDROID

    public AudioSource[] passwordAudio = new AudioSource[2];
    private Vector2 movement;
    private Rigidbody rigid;

    private bool isDragging = false;

    private float offset;

    private Material _mat;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        _mat = renderer.material;

        rigid = this.GetComponent<Rigidbody>();
        offset = Vector3.Distance(transform.position, Camera.main.transform.position);

        this.transform.localScale = new Vector3(Random.Range(5, 8), Random.Range(5, 8), 0.05f);
        this.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(398, 406), 1.1f);
        color = new Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));
        _mat.color = color;
    }

    private void Update()
    {
        Drag();
    }

    private void Drag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (isDragging)
            {
                rigid.velocity = Vector2.zero;
                Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, offset);
                newPosition = Camera.main.ScreenToWorldPoint(newPosition);
                transform.position = newPosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

#endif
}