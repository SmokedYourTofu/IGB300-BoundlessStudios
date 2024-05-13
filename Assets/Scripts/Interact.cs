using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Interact : MonoBehaviour
{
    public float sphereCastRadius;

    private Ray ray;
    private RaycastHit raycastHit;


    public Camera camera_1;
    public Camera camera_2;
    public GameObject environment;
    public GameObject microgame1;
    public GameObject controls;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {
            //do a raycast to check for anything
            Debug.Log("interacted");
        }


#if UNITY_EDITOR 
        if (Input.GetMouseButtonDown(0)) {
            //do a raycast to check for anything
            Debug.Log("interacted");
        }
    }

    // Where all interactions functions will be

    //made a simple method just to access microgames so I can test them in editor (THIS IS NOT FINAL)
    public void accessMicrogame()
    {
        camera_2.gameObject.SetActive(true);
        microgame1.gameObject.SetActive(true);
        camera_1.gameObject.SetActive(false);
        environment.SetActive(false);
        controls.SetActive(false);
        this.gameObject.SetActive(false);
    }
#endif
}
