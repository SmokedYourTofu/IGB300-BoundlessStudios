using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingBumper : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject firstPos;
    public GameObject secondPos;

    [SerializeField] private bool down = true;
    private SpringJoint springJoint;

    void Start()
    {
        springJoint = GetComponent<SpringJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (down)
        {
            transform.position = Vector3.MoveTowards(transform.position, firstPos.transform.position, 1f * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, secondPos.transform.position, 1f * Time.deltaTime);
        }

        if (springJoint != null)
        {
            springJoint.anchor = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "bumperSpotDown")
        {
            down = false;
        }
        else if (other.tag == "bumperSpotUp")
        {
            down = true;
        }
    }
}
