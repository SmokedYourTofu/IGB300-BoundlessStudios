using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveSmashCollision : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Canvas Controlcanvas;
    [SerializeField] private GameObject drive;
    private Vector3 driveLocation;
    private Vector3 driveLocationUi;

    private void Awake()
    {
        canvas = this.transform.parent.transform.parent.GetComponent<Canvas>();
        Controlcanvas = GameObject.FindGameObjectWithTag("Controller").GetComponent<Canvas>();
        Controlcanvas.enabled = false;
        drive = GameObject.FindGameObjectWithTag("Drive");
        driveLocation = Camera.main.WorldToScreenPoint(drive.transform.position);
        driveLocationUi = new Vector3(driveLocation.x, Screen.height - driveLocation.y, driveLocation.z);

    }

    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 uiPos = new Vector3(screenPos.x, Screen.height - screenPos.y, screenPos.z);
        //Debug.Log(uiPos);
        if (uiPos.y < driveLocationUi.y + 100f && uiPos.y > driveLocationUi.y - 100f)
        {
            if (uiPos.x < driveLocationUi.x + 100f && uiPos.x > driveLocationUi.x - 100f)
            {
                //do score/points/animation or tally completed part of task

                Destroy(canvas);
                Controlcanvas.enabled = true;
            }
        }
    }
}
