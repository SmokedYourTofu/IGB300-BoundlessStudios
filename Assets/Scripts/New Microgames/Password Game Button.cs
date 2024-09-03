using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.XR;

public class PasswordGameButton : MonoBehaviour
{
#if UNITY_ANDROID
    public GameObject passwordController;
    private GameObject buttonText;
    private PasswordMicrogame psMicrogame;
    private string truePassword;
    private TMP_Text passwordText;
    public AudioSource[] passwordAudio = new AudioSource[2];
    private Vector2 movement;
    Rigidbody rigid;

    private bool isDragging = false;
    private bool inSpace = false;
    public GameObject passwordSpot;
    private Vector3 originPos;

    private float offset;

    private Material _mat;

    private bool gameDone = false;

    private void Awake()
    {
        buttonText = this.transform.GetChild(0).gameObject;
        psMicrogame = passwordController.GetComponent<PasswordMicrogame>();
        passwordText = buttonText.GetComponent<TMP_Text>();
        if (passwordText == null)
        {
            passwordText = buttonText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        }
    }

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        _mat = renderer.material;
        originPos = transform.position;

        movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        rigid = this.GetComponent<Rigidbody>();
        if (rigid != null)
        {
            rigid.velocity = movement;
        }
        offset = Vector3.Distance(transform.position, Camera.main.transform.position);
    }

    private void Update()
    {
        Drag();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.gameObject.tag == "border1")
        {
            movement = Vector2.Reflect(movement, new Vector2(0, 1));
            rigid.velocity = movement;
        }
        else if (other.gameObject.tag == "border2")
        {
            movement = Vector2.Reflect(movement, new Vector2(1, 0));
            rigid.velocity = movement;
        }
        else if (other.gameObject.tag == "passwordSpot")
        {
            inSpace = true;
            Debug.Log(inSpace);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "passwordSpot")
        {
            inSpace = false;
        }
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
                    transform.localPosition = newPosition;
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (isDragging)
                {
                    movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                }
                isDragging = false;
                rigid.velocity = movement;

                if (inSpace)
                {
                    truePassword = psMicrogame.realPassword;
                    rigid.velocity = Vector2.zero;

                    if (passwordText.text == truePassword && gameDone == false)
                    {
                        gameDone = true;
                        Vector3 position = passwordSpot.transform.position;
                        position.z = position.z - 0.1f;
                        this.transform.position = position;
                        psMicrogame.audioSources[1].Play();
                        psMicrogame.audioSources[2].Play();
                        _mat.SetColor("_Color", Color.green);
                        StartCoroutine(psMicrogame.FinishWait());
                    }
                    else
                    {
                        Vector3 position = passwordSpot.transform.position;
                        position.z = position.z - 0.1f;
                        this.transform.position = position;
                        Debug.Log("wrong password");
                        StartCoroutine(badPassword());
                    }
                }
                else if (transform.position.x > 7 || transform.position.x < -7 || transform.position.y > 405 || transform.position.y < 396.5)
                {
                    Vector3 position = originPos;
                    this.transform.position = position;
                }
            }
        }
    }

    public void onButtonHit()
    {
        truePassword = psMicrogame.realPassword;
        if (passwordText.text == truePassword)
        {
            psMicrogame.audioSources[1].Play();
            psMicrogame.audioSources[2].Play();
            _mat.SetColor("_Color", Color.green);
            StartCoroutine(psMicrogame.FinishWait());
        }
        else
        {
            Debug.Log("wrong password");
            psMicrogame.setupGame();
            psMicrogame.audioSources[0].Play();
        }
    }

    private IEnumerator badPassword()
    {
        _mat.SetColor("_Color", Color.red);
        psMicrogame.audioSources[0].Play();
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
#endif

#if !UNITY_ANDROID

    public GameObject passwordController;
    private GameObject buttonText;
    private PasswordMicrogame psMicrogame;
    private string truePassword;
    private TMP_Text passwordText;
    public AudioSource[] passwordAudio = new AudioSource[2];
    private Vector2 movement;
    private Rigidbody rigid;

    private bool isDragging = false;
    private bool inSpace = false;
    public GameObject passwordSpot;

    private float offset;

    private Material _mat;

    private bool gameDone = false;

    private void Awake()
    {
        Debug.Log("pc");
        buttonText = this.transform.GetChild(0).gameObject;
        psMicrogame = passwordController.GetComponent<PasswordMicrogame>();
        passwordText = buttonText.GetComponent<TMP_Text>();
        if (passwordText == null)
        {
            passwordText = buttonText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        }
    }

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        _mat = renderer.material;

        movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        rigid = this.GetComponent<Rigidbody>();
        if (rigid != null)
        {
            rigid.velocity = movement;
        }
        offset = Vector3.Distance(transform.position, Camera.main.transform.position);
    }

    private void Update()
    {
        Drag();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.gameObject.tag == "border1")
        {
            movement = Vector2.Reflect(movement, new Vector2(0, 1));
            rigid.velocity = movement;
        }
        else if (other.gameObject.tag == "border2")
        {
            movement = Vector2.Reflect(movement, new Vector2(1, 0));
            rigid.velocity = movement;
        }
        else if (other.gameObject.tag == "passwordSpot")
        {
            inSpace = true;
            Debug.Log(inSpace);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "passwordSpot")
        {
            inSpace = false;
        }
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
                transform.localPosition = newPosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            }
            isDragging = false;
            rigid.velocity = movement;

            if (inSpace)
            {
                truePassword = psMicrogame.realPassword;
                rigid.velocity = Vector2.zero;

                if (passwordText.text == truePassword && gameDone == false)
                {
                    gameDone = true;
                    Vector3 position = passwordSpot.transform.position;
                    position.z = position.z - 0.1f;
                    this.transform.position = position;
                    psMicrogame.audioSources[1].Play();
                    psMicrogame.audioSources[2].Play();
                    _mat.SetColor("_Color", Color.green);
                    StartCoroutine(psMicrogame.FinishWait());
                }
                else
                {
                    Vector3 position = passwordSpot.transform.position;
                    position.z = position.z - 0.1f;
                    this.transform.position = position;
                    Debug.Log("wrong password");
                    StartCoroutine(badPassword());
                }
            }
        }
    }

    public void onButtonHit()
    {
        truePassword = psMicrogame.realPassword;
        if (passwordText.text == truePassword)
        {
            psMicrogame.audioSources[1].Play();
            psMicrogame.audioSources[2].Play();
            _mat.SetColor("_Color", Color.green);
            StartCoroutine(psMicrogame.FinishWait());
        }
        else
        {
            Debug.Log("wrong password");
            psMicrogame.setupGame();
            psMicrogame.audioSources[0].Play();
        }
    }

    private IEnumerator badPassword()
    {
        _mat.SetColor("_Color", Color.red);
        psMicrogame.audioSources[0].Play();
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

#endif
}