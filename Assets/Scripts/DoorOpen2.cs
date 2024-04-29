using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen2 : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    private Quaternion startRotation;
    [SerializeField] private bool shouldRotate = false;
    [SerializeField] private bool rotateBack = false;
    [SerializeField] private float timer = 0f;
    [SerializeField] public float rotateInterval;

    void Start()
    {
        startRotation = transform.rotation;
        rotationSpeed = 160 / (rotateInterval / 4);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= rotateInterval && !shouldRotate)
        {
            shouldRotate = true;
            timer = 0f;
        }

        if (shouldRotate && !rotateBack)
        {
            // Rotate smoothly by 160 degrees over 5 seconds
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);

            if (transform.rotation.eulerAngles.y <= 0.3f)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                shouldRotate = false;
                rotateBack = true;
                //timer = 0f;
            }
        }
        else if (rotateBack && shouldRotate)
        {
            // Rotate back to the starting point over 5 seconds
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            if (transform.rotation.eulerAngles.y >= startRotation.eulerAngles.y)
            {
                transform.rotation = startRotation;
                shouldRotate = false;
                rotateBack = false;
                //timer = 0f;
            }
        }
    }
}


