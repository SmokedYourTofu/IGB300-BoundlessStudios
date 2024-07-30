using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankRotate : MonoBehaviour
{
    private float startAngle = 0;
    private Quaternion originalRotation;
    public bool activateRotate;

    public void Start()
    {
        originalRotation = this.transform.rotation;

    }
    void Update()
    {
        if (activateRotate)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 vector = Input.mousePosition - screenPos;
            float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
            Quaternion newRotation = Quaternion.AngleAxis(angle - startAngle, this.transform.forward);
            newRotation.y = 0; //see comment from above 
            newRotation.eulerAngles = new Vector3(0, 0, newRotation.eulerAngles.z);
            this.transform.rotation = originalRotation * newRotation;
        }
    }
}
