using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankHandle : MonoBehaviour
{

    public GameObject handlePar;
    private CrankRotate CR;

    private void Start()
    {
        CR = handlePar.GetComponent<CrankRotate>(); 
    }

    private void OnMouseDown()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray myRay = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(myRay, out RaycastHit hit))
        {
            // Use the hit variable to determine what was clicked on.
            if (hit.transform.tag == "border1")
            {
                CR.activateRotate = true;
            }
        }
    }

    private void OnMouseDrag()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray myRay = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(myRay, out RaycastHit hit))
        {
            // Use the hit variable to determine what was clicked on.
            if (hit.transform.tag == "border1")
            {
                CR.activateRotate = true;
            }
            else
            {
                CR.activateRotate = false;
            }
        }
    }

    private void OnMouseUp()
    {
        CR.activateRotate = false;
    }
}
