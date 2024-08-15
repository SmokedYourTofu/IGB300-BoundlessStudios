using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class MovingVirus : MonoBehaviour
{
#if UNITY_ANDROID
    public GameObject VirusController;
    private VirusIdentity VRSMicrogame;
    private string trueVirus;
    private Vector2 movement;
    Rigidbody rigid;
    private Vector3 originPos;

    private bool isDragging = false;
    private bool inSpace = false;
    public GameObject virusSpot;

    private float offset;

    private Material _mat;

    public Vector3 startposition;

    //get controller script for game on start
    private void Awake()
    {
        VRSMicrogame = VirusController.GetComponent<VirusIdentity>();
    }

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        _mat = renderer.material;
        originPos = transform.position;

        //when the game starts, change virus velocity to get it moving around
        movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        rigid = this.GetComponent<Rigidbody>();
        if (rigid != null)
        {
            rigid.velocity = movement;
        }

        offset = Vector3.Distance(transform.position, Camera.main.transform.position);
        transform.position = startposition;
    }

    private void Update()
    {
        Drag();
    }

    void OnTriggerEnter(Collider other)
    {
        //if a virus hits a border bounce off
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
        //if the virus is in the checking spot, initiate that script
        else if (other.gameObject.tag == "passwordSpot")
        {
            inSpace = true;
            Debug.Log(inSpace);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if the virus is outside the checking spot, disable that script
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

            //when a player touches the screen send a ray to check if they are touching a virus
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    //if player is touching virus, allow them to move it
                    if (hit.collider.gameObject == gameObject)
                    {
                        isDragging = true;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                //if a player is attempting to drag a virus, move it wwith their touch
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
                //if a player lets go of a virus, allow it to move around again
                if (isDragging)
                {
                    movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                }
                isDragging = false;
                rigid.velocity = movement;

                if (inSpace)
                {
                    trueVirus = VRSMicrogame.realVirus;
                    rigid.velocity = Vector2.zero;

                    //if the virus is in the identifier slot and bears the sme tag as the proper virus finish the gaame
                    if (this.gameObject.tag == trueVirus)
                    {
                        Vector3 position = virusSpot.transform.position;
                        position.z = position.z - 0.1f;
                        this.transform.position = position;
                        VRSMicrogame.audioSources[1].Play();
                        VRSMicrogame.audioSources[2].Play();
                        _mat.SetColor("_Color", Color.green);
                        StartCoroutine(VRSMicrogame.FinishWait());
                    }
                    //if the virus is incorrect reset it's position
                    else
                    {
                        Vector3 position = originPos;
                        this.transform.position = position;
                        Debug.Log("wrong password");
                        StartCoroutine(badVirus());
                    }
                }
                else if (transform.position.x > 4.8 ||  transform.position.x < -9.8 || transform.position.y > 406.6 || transform.position.y < 395)
                {
                    Vector3 position = originPos;
                    this.transform.position = position;
                    StartCoroutine(badVirus());
                }
            }
        }
    }

    //indicaate the virus was incorrect by changing colour and move it back into the cage
    private IEnumerator badVirus()
    {
        _mat.SetColor("_Color", Color.red);
        VRSMicrogame.audioSources[0].Play();
        yield return new WaitForSeconds(1f);
        _mat.SetColor("_Color", Color.white);
    }
#endif

#if !UNITY_ANDROID

    public GameObject VirusController;
    private VirusIdentity VRSMicrogame;
    private string trueVirus;
    private Vector2 movement;
    Rigidbody rigid;
    private Vector3 originPos;

    private bool isDragging = false;
    private bool inSpace = false;
    public GameObject passwordSpot;

    private float offset;

    private Material _mat;

    //get controller script for game on start
    private void Awake()
    {
        VRSMicrogame = VirusController.GetComponent<VirusIdentity>();
    }

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        _mat = renderer.material;

        //when the game starts, change virus velocity to get it moving around
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
        //if a virus hits a border bounce off
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
        //if the virus is in the checking spot, initiate that script
        else if (other.gameObject.tag == "passwordSpot")
        {
            inSpace = true;
            Debug.Log(inSpace);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if the virus is outside the checking spot, disable that script
        if (other.gameObject.tag == "passwordSpot")
        {
            inSpace = false;
        }
    }

    private void Drag()
    {
        //when a player touches the screen send a ray to check if they are touching a virus
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //if player is touching virus, allow them to move it
                if (hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                }
            }
        }
        //if a player is attempting to drag a virus, move it wwith their touch
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
            //if a player lets go of a virus, allow it to move around again
            if (isDragging)
            {
                movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            }
            isDragging = false;
            rigid.velocity = movement;

            if (inSpace)
            {
                trueVirus = VRSMicrogame.realVirus;
                rigid.velocity = Vector2.zero;

                //if the virus is in the identifier slot and bears the sme tag as the proper virus finish the gaame
                if (this.gameObject.tag == trueVirus)
                    {
                        Vector3 position = passwordSpot.transform.position;
                        position.z = position.z - 0.1f;
                        this.transform.position = position;
                        VRSMicrogame.audioSources[1].Play();
                        VRSMicrogame.audioSources[2].Play();
                        _mat.SetColor("_Color", Color.green);
                        StartCoroutine(VRSMicrogame.FinishWait());
                    }
                    //if the virus is incorrect reset it's position
                    else
                    {
                        Vector3 position = originPos;
                        this.transform.position = position;
                        Debug.Log("wrong password");
                        StartCoroutine(badPassword());
                    }
            }
            else if (transform.position.x > 4.8 ||  transform.position.x < -9.8 || transform.position.y > 406.6 || transform.position.y < 395)
            {
                Vector3 position = originPos;
                this.transform.position = position;
                StartCoroutine(badVirus());
            }
        }
    }

    //indicaate the virus was incorrect by changing colour and move it back into the cage
    private IEnumerator badVirus()
    {
        _mat.SetColor("_Color", Color.red);
        VRSMicrogame.audioSources[0].Play();
        yield return new WaitForSeconds(1f);
        _mat.SetColor("_Color", Color.white);
    }

#endif
}
