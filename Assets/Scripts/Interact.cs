using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Interact : MonoBehaviour
{
    public float sphereCastRadius;

    private Ray ray;
    private RaycastHit raycastHit;
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
#endif

    // Where all interactions functions will be
}
